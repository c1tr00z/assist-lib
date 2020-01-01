using System.Collections.Generic;

namespace c1tr00z.AssistLib.Json {
    public interface IJsonDeserializable {
        void Deserialize(Dictionary<string, object> json);
    }
}