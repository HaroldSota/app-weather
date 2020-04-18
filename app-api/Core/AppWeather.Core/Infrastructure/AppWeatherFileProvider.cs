using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AppWeather.Core.Infrastructure
{
    /// <inheritdoc />
    public class AppWeatherFileProvider : PhysicalFileProvider, IAppWeatherFileProvider
    {
        #region [ Fileds ]

        protected string BaseDirectory { get; }

        #endregion

        #region [ Ctor ]

        public AppWeatherFileProvider(string rootPath)
            : base(rootPath)
        {
            BaseDirectory = rootPath;
        }

        #endregion

        #region [ IAppWeatherFileProvider: Implementation ]

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public string[] GetFiles(string directoryPath, string searchPattern = "")
        {
            if (string.IsNullOrEmpty(searchPattern))
                searchPattern = "*.*";

            return Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
        }

        #endregion
    }
}