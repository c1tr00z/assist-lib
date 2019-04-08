using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface ISaveble {

    Hash ToJSON();
    void FromJSON(Hash hash);
}
