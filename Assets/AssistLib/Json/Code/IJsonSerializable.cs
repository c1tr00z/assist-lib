using System.Collections.Generic;

namespace c1tr00z.AssistLib.Json {
    public interface IJsonSerializable {
        void Serialize(Dictionary<string, object> json);
    }
}