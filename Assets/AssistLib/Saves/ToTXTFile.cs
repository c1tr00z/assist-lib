using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;

public class ToTXTFile : ToFile {

    protected override void LoadSaves(string key, System.Action<Dictionary<string, object>, System.Exception> complete) {
        StringBuilder sb = new StringBuilder();
        using (StreamReader sr = new StreamReader(GetFileByName(key).ToString())) {
            String line;
            while ((line = sr.ReadLine()) != null) {
                sb.AppendLine(line);
            }
            sr.Close();
        }
        string allines = sb.ToString();
        if (complete != null) {
            if (string.IsNullOrEmpty(allines)) {
                complete(null, null);
            } else {
                complete(JSONUtuls.Deserialize(allines), null);
            }
        }
    }

    public override void Save(string key, string toSave, System.Action<bool, System.Exception> complete) {
        try {
            using (StreamWriter sw = new StreamWriter(GetFileByName(key).ToString())) {
                sw.Write(toSave);
                sw.Close();
            }
            if (complete != null) {
                complete(true, null);
            }
        } catch (Exception e) {
            complete(false, e);
        }
    }
}