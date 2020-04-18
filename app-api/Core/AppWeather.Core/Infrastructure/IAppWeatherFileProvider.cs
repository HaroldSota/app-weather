using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppWeather.Core.Infrastructure
{
    public interface IAppWeatherFileProvider : IFileProvider
    {
        bool DirectoryExists(string path);

        string[] GetFiles(string directoryPath, string searchPattern = "");
    }
}
