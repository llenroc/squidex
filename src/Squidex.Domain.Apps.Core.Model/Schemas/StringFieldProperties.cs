﻿// ==========================================================================
//  StringFieldProperties.cs
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex Group
//  All rights reserved.
// ==========================================================================

using System.Collections.Immutable;
using Squidex.Infrastructure;

namespace Squidex.Domain.Apps.Core.Schemas
{
    [TypeName(nameof(StringField))]
    public sealed class StringFieldProperties : FieldProperties
    {
        private int? minLength;
        private int? maxLength;
        private string pattern;
        private string patternMessage;
        private string defaultValue;
        private ImmutableList<string> allowedValues;
        private StringFieldEditor editor;

        public int? MinLength
        {
            get
            {
                return minLength;
            }
            set
            {
                ThrowIfFrozen();

                minLength = value;
            }
        }

        public int? MaxLength
        {
            get
            {
                return maxLength;
            }
            set
            {
                ThrowIfFrozen();

                maxLength = value;
            }
        }

        public string DefaultValue
        {
            get
            {
                return defaultValue;
            }
            set
            {
                ThrowIfFrozen();

                defaultValue = value;
            }
        }

        public string Pattern
        {
            get
            {
                return pattern;
            }
            set
            {
                ThrowIfFrozen();

                pattern = value;
            }
        }

        public string PatternMessage
        {
            get
            {
                return patternMessage;
            }
            set
            {
                ThrowIfFrozen();

                patternMessage = value;
            }
        }

        public ImmutableList<string> AllowedValues
        {
            get
            {
                return allowedValues;
            }
            set
            {
                ThrowIfFrozen();

                allowedValues = value;
            }
        }

        public StringFieldEditor Editor
        {
            get
            {
                return editor;
            }
            set
            {
                ThrowIfFrozen();

                editor = value;
            }
        }

        public override T Accept<T>(IFieldPropertiesVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override Field CreateField(long id, string name, Partitioning partitioning)
        {
            return new StringField(id, name, partitioning, this);
        }
    }
}
