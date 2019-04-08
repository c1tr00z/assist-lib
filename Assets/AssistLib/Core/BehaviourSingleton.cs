using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviourSingleton<T> : MonoBehaviour where T: MonoBehaviour {

    private static T _instance;

    public static T instance {
        get {
            TryFind();
            return _instance;
        }
    }

    protected virtual void Awake() {
        if (_instance == null || _instance.Equals(default(MonoBehaviour))) {
            _instance = GetThis();
        } else if (!_instance.Equals(GetThis())) {
            Destroy(gameObject);
        }
    }

    private static void TryFind() {
        if (_instance == null) {
            _instance = FindObjectOfType<T>();
        }
    }

    protected virtual T GetThis() {
        return this as T;
    }

}
