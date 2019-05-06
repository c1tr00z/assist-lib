using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleDBEntry : DBEntry {

    [SerializeField] private int _priority;

    public int priority {
        get {
            return _priority;
        }
    }
}
