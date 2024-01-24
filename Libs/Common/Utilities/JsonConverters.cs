using Newtonsoft.Json;

namespace JB.Common.Utilities
{
    public class ListConverter<TInterface, TClass> : JsonConverter where TClass : TInterface
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            IList<TClass>? classList = serializer.Deserialize<IList<TClass>>(reader);
            IList<TInterface> interfaceList = new List<TInterface>();

            if (classList == null)
            {
                return null;
            }

            foreach (TClass classObj in classList)
            {
                interfaceList.Add(classObj);
            }

            return interfaceList;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return reader.ReadAsDateTime() ?? existingValue;
        }

        public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString("yyyy-MM-ddThh:mm:ss"));
        }
    }
}
