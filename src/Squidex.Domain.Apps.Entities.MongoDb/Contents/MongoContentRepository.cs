﻿// ==========================================================================
//  MongoContentRepository.cs
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex Group
//  All rights reserved.
// ==========================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OData.UriParser;
using MongoDB.Driver;
using Squidex.Domain.Apps.Core.Contents;
using Squidex.Domain.Apps.Entities.Apps;
using Squidex.Domain.Apps.Entities.Contents;
using Squidex.Domain.Apps.Entities.Contents.Repositories;
using Squidex.Domain.Apps.Entities.MongoDb.Contents.Visitors;
using Squidex.Domain.Apps.Entities.Schemas;
using Squidex.Infrastructure;
using Squidex.Infrastructure.MongoDb;

namespace Squidex.Domain.Apps.Entities.MongoDb.Contents
{
    public partial class MongoContentRepository : MongoRepositoryBase<MongoContentEntity>, IContentRepository
    {
        private readonly IAppProvider appProvider;

        public MongoContentRepository(IMongoDatabase database, IAppProvider appProvider)
            : base(database)
        {
            Guard.NotNull(appProvider, nameof(appProvider));

            this.appProvider = appProvider;
        }

        protected override string CollectionName()
        {
            return "States_Contents";
        }

        protected override async Task SetupCollectionAsync(IMongoCollection<MongoContentEntity> collection)
        {
            await collection.Indexes.CreateOneAsync(
                Index
                    .Ascending(x => x.Id)
                    .Ascending(x => x.Version));

            await collection.Indexes.CreateOneAsync(
                Index
                    .Ascending(x => x.Id)
                    .Descending(x => x.Version));

            await collection.Indexes.CreateOneAsync(
                Index
                    .Ascending(x => x.SchemaId)
                    .Descending(x => x.IsLatest)
                    .Descending(x => x.LastModified));

            await collection.Indexes.CreateOneAsync(Index.Ascending(x => x.ReferencedIds));
            await collection.Indexes.CreateOneAsync(Index.Ascending(x => x.Status));
            await collection.Indexes.CreateOneAsync(Index.Text(x => x.DataText));
        }

        public async Task<IResultList<IContentEntity>> QueryAsync(IAppEntity app, ISchemaEntity schema, Status[] status, ODataUriParser odataQuery)
        {
            IFindFluent<MongoContentEntity, MongoContentEntity> find;
            try
            {
                find = Collection.Find(odataQuery, schema.Id, schema.SchemaDef, status);
            }
            catch (NotSupportedException)
            {
                throw new ValidationException("This odata operation is not supported.");
            }
            catch (NotImplementedException)
            {
                throw new ValidationException("This odata operation is not supported.");
            }

            var contentItems = find.Take(odataQuery).Skip(odataQuery).Sort(odataQuery, schema.SchemaDef).ToListAsync();
            var contentCount = find.CountAsync();

            await Task.WhenAll(contentItems, contentCount);

            foreach (var entity in contentItems.Result)
            {
                entity.ParseData(schema.SchemaDef);
            }

            return ResultList.Create<IContentEntity>(contentItems.Result, contentCount.Result);
        }

        public async Task<IResultList<IContentEntity>> QueryAsync(IAppEntity app, ISchemaEntity schema, Status[] status, HashSet<Guid> ids)
        {
            var find = Collection.Find(x => ids.Contains(x.Id) && x.IsLatest);

            var contentItems = find.ToListAsync();
            var contentCount = find.CountAsync();

            await Task.WhenAll(contentItems, contentCount);

            foreach (var entity in contentItems.Result)
            {
                entity.ParseData(schema.SchemaDef);
            }

            return ResultList.Create<IContentEntity>(contentItems.Result, contentCount.Result);
        }

        public async Task<IReadOnlyList<Guid>> QueryNotFoundAsync(Guid appId, Guid schemaId, IList<Guid> contentIds)
        {
            var contentEntities =
                await Collection.Find(x => contentIds.Contains(x.Id) && x.AppId == appId).Only(x => x.Id)
                    .ToListAsync();

            return contentIds.Except(contentEntities.Select(x => Guid.Parse(x["id"].AsString))).ToList();
        }

        public async Task<IContentEntity> FindContentAsync(IAppEntity app, ISchemaEntity schema, Guid id, long version)
        {
            var contentEntity =
                await Collection.Find(x => x.Id == id && x.Version >= version).SortBy(x => x.Version)
                    .FirstOrDefaultAsync();

            contentEntity?.ParseData(schema.SchemaDef);

            return contentEntity;
        }

        public async Task<IContentEntity> FindContentAsync(IAppEntity app, ISchemaEntity schema, Guid id)
        {
            var contentEntity =
                await Collection.Find(x => x.Id == id && x.IsLatest)
                    .FirstOrDefaultAsync();

            contentEntity?.ParseData(schema.SchemaDef);

            return contentEntity;
        }
    }
}
