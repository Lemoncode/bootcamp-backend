using System;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Lemoncode.Soccer.IntegrationTests.TestSupport.Builders
{
    public class StringContentBuilder
    {
        private string _contentString = null!;
        private Encoding _encoding = Encoding.UTF8;
        private string _mediaType = "application/json";

        public StringContentBuilder WithContent(Stream stream)
        {
            using var streamReader = new StreamReader(stream);
            _contentString = streamReader.ReadToEnd();
            return this;
        }

        public StringContentBuilder WithContent(string content)
        {
            _contentString = content;
            return this;
        }

        public StringContentBuilder WithPlaceholderReplacement(string placeholderName, string newValue)
        {
            if (_contentString is null) throw new InvalidOperationException("No content defined");
            _contentString = _contentString.Replace("{{" + placeholderName + "}}", newValue);
            return this;
        }

        public StringContentBuilder WithEncoding(Encoding encoding)
        {
            _encoding = encoding;
            return this;
        }

        public StringContentBuilder WithMediaType(string mediaType)
        {
            _mediaType = mediaType;
            return this;
        }

        public StringContent Build()
        {
            var stringContent = new StringContent(_contentString, _encoding, _mediaType);
            return stringContent;
        }
    }
}