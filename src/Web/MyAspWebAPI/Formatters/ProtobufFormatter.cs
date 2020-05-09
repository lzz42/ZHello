using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using ProtoBuf;

namespace MyAspWebAPI.Formatters
{
    public class ProtobufFormatter : OutputFormatter
    {
        public string ContentType { get; private set; }

        public ProtobufFormatter()
        {
            if (string.IsNullOrEmpty(ContentType))
                ContentType = "application/proto";
            SupportedMediaTypes.Add(Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse(ContentType));
        }

        public ProtobufFormatter(string contentType) : this()
        {
            ContentType = contentType;
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var response = context.HttpContext.Response;
            Serializer.Serialize(response.Body, context.Object);
            return Task.FromResult(0);
        }
    }
}