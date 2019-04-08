using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour {

    [SerializeField]
    private GameObject _prefabSrc;

    private void Awake() {
        var clone = _prefabSrc.Clone();
        clone.name = _prefabSrc.name;
        clone.transform.SetParent(transform.parent);
        clone.transform.localPosition = transform.localPosition;
        clone.transform.localRotation = transform.localRotation;
        clone.transform.localScale = transform.localScale;
        Destroy(gameObject);
    }

}
