using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lemoncode.Soccer.IntegrationTests.TestSupport.Builders
{
    public class FileStreamBuilder
    {
        private string _fileName = null!;

        public FileStreamBuilder WithFileResourceName(string fileName)
        {
            _fileName = fileName;
            return this;
        }

        public Stream Build()
        {
            if (_fileName is null) throw new InvalidOperationException("No file resource name defined");

            var assembly = GetType().Assembly;

            var resourceName =
                assembly
                    .GetManifestResourceNames()
                    .SingleOrDefault(x => x.Contains(_fileName));
            if (resourceName is null)
            {
                throw new KeyNotFoundException($"Could not find resource name with name {_fileName}");
            }

            var stream = assembly.GetManifestResourceStream(resourceName) ??
                         throw new FileNotFoundException($"Could not load {_fileName} into a stream");
            return stream;
        }
    }
}