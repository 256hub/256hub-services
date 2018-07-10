using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hub256.Common.Services
{
    public class JsonNullDateTimeConverter : IsoDateTimeConverter
    {
        private readonly string _stringError;

        public JsonNullDateTimeConverter(string dateTimeFormat, string stringError)
        {
            DateTimeFormat = dateTimeFormat;
            _stringError = stringError;
        }

        public override object ReadJson(JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer)
        {
            return reader.Value.ToString().Contains(_stringError)
                ? DateTime.MinValue
                : base.ReadJson(reader, objectType, existingValue, serializer);
        }
    }
}
