using UnityEngine;
using System.Collections;

public class DBEntryResource : MonoBehaviour {

    [SerializeField] private DBEntry _dbEntry;

    [SerializeField] private string _key;

    public DBEntry parent {
        get {
            return _dbEntry;
        }
    }

    public string key {
        get {
            return _key;
        }
    }

    public void SetParent(DBEntry newParent, string newKey) {
        _dbEntry = newParent;
        _key = newKey;
    }
}
