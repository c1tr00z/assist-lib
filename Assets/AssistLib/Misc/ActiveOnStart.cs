using UnityEngine;
using System.Collections;

public class ActiveOnStart : MonoBehaviour {

    [SerializeField] private bool _active;

    void Awake() {
        gameObject.SetActive(_active);
    }
	
}
