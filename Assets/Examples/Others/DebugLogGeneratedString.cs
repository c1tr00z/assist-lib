using System;
using System.Collections;
using System.Collections.Generic;
using c1tr00z.AssistLib.Localization;
using UnityEngine;
using Random = UnityEngine.Random;


public class DebugLogGeneratedString : MonoBehaviour {
    [SerializeField] private int _count = 5;
    private void Start() {
        for (int i = 0; i < _count; i++) {
            Debug.Log(StringUtils.RandomString(Random.Range(5, 25), true, false));
        }
    }
}
