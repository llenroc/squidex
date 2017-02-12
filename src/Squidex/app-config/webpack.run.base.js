﻿// ReSharper disable InconsistentNaming
// ReSharper disable PossiblyUnassignedProperty

      var webpack = require('webpack'),
     webpackMerge = require('webpack-merge'),
HtmlWebpackPlugin = require('html-webpack-plugin'),
     commonConfig = require('./webpack.config.js'),
          helpers = require('./helpers');

module.exports = webpackMerge(commonConfig, {
    /**
     * The entry point for the bundle
     * Our Angular.js app
     *
     * See: https://webpack.js.org/configuration/entry-context/
     */
    entry: {
        'polyfills': './app/polyfills.ts',
           'vendor': './app/vendor.ts',
              'app': './app/main.ts'
    },

    plugins: [
        /**
         * Shares common code between the pages.
         *
         * See: https://webpack.js.org/plugins/commons-chunk-plugin/
         */
        new webpack.optimize.CommonsChunkPlugin({
            name: ['app', 'vendor', 'polyfills']
        }),

        /**
         * Simplifies creation of HTML files to serve your webpack bundles.
         * This is especially useful for webpack bundles that include a hash in the filename
         * which changes every compilation.
         *
         * See: https://github.com/ampedandwired/html-webpack-plugin
         */
        new HtmlWebpackPlugin({
            template: 'wwwroot/index.html'
        })
    ]
});