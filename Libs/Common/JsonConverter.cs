using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JB.Common {
    public class JsonListConverter<Tinterface, Tclass> : JsonConverter where Tclass : Tinterface {
        public override bool CanConvert(Type objectType) {
            return true;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) {
            IList<Tclass>? classList = serializer.Deserialize<IList<Tclass>>(reader);
            IList<Tinterface> interfaceList = new List<Tinterface>();

            if (classList == null) {
                return null;
            }

            foreach(Tclass classObj in classList) {
                interfaceList.Add(classObj);
            }

            return interfaceList;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {
            serializer.Serialize(writer, value);
        }
    }
}
