using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class SaveMethod : DBEntry {

    public abstract void Save(string key, string Hash, System.Action<bool, Exception> complete);

    public abstract void Load(string key, System.Action<Dictionary<string, object>, Exception> complete);

    public abstract void LoadSavesList(Action<IEnumerable<string>, Exception> complete);

    public abstract void Create(string key, System.Action<bool, Exception> complete);
}
