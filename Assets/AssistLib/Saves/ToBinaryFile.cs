using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;

public class ToBinaryFile : ToFile {

    [SerializeField]
    private int _keyCode;

    protected override void LoadSaves(string key, Action<Dictionary<string, object>, Exception> complete) {
        StringBuilder sb = new StringBuilder();
        string allLines = null;
        //GetFileByName(key).Open(FileMode.OpenOrCreate);
        using (BinaryReader br = new BinaryReader((GetFileByName(key).Open(FileMode.OpenOrCreate)))) {
            if (br.BaseStream.Length > 0) {
                allLines = br.ReadString();
                br.ReadInt32();
            }
        }
        if (complete != null) {
            if (string.IsNullOrEmpty(allLines)) {
                complete(null, null);
            } else {
                complete(JSONUtuls.Deserialize(allLines), null);
            }
        }
    }

    public override void Save(string key, string toSave, System.Action<bool, System.Exception> complete) {
        using (BinaryWriter bw = new BinaryWriter(GetFileByName(key).Open(FileMode.Create))) {
            bw.Write(toSave);
            bw.Write(_keyCode);
            bw.Close();
        }
    }
}
