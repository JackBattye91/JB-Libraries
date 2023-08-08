﻿using Newtonsoft.Json;

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

    public class EnumIntConverter<T> : JsonConverter {
        public override bool CanConvert(Type objectType) {
            return objectType.IsEnum;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) {
            int value = serializer.Deserialize<int>(reader);
            T? obj = (T?)typeof(T).GetEnumValues().GetValue(value);
            return obj;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {
            serializer.Serialize(writer, (int?)value);
        }
    }
    public class EnumNameConverter<T> : JsonConverter<T> {
        public override T? ReadJson(JsonReader reader, Type objectType, T? existingValue, bool hasExistingValue, JsonSerializer serializer) {
            string? value = serializer.Deserialize<string>(reader);
            string[] enumNamesList = objectType.GetEnumNames();
            T[] enumValuesList = (T[])objectType.GetEnumValues();

            for(int n = 0; n < enumNamesList.Length; n++) {
                if (enumNamesList[n] == value) {
                    return enumValuesList[n];
                }
            }

            return default;
        }

        public override void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer) {
            if (value is T) {
                string? name = typeof(T).GetEnumName(value);
                serializer.Serialize(writer, name);
            }
        }
    }
}
