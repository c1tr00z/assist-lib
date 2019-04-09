using System.Collections.Generic;

public interface ISaveble {

    Dictionary<string, object> ToJSON();
    void FromJSON(Dictionary<string, object> json);
}
