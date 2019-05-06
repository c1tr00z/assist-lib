using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;

public abstract class ToFile : SaveMethod {

    public string savedGamesPath;
    public string savedGamesFileExtension;

    public string savesPath {
        get {
            return savedGamesPath;
        }
    }

    protected abstract void LoadSaves(string key, System.Action<Dictionary<string, object>, System.Exception> complete);

    public override void Load(string key, System.Action<Dictionary<string, object>, System.Exception> complete) {
        try {
            if (string.IsNullOrEmpty(key)) {
                if (complete != null) {
                    complete(null, new UnityException("Key cannot be null"));
                }
                return;
            }
            LoadSaves(key, complete);
        } catch (Exception e) {
            if (complete != null) {
                complete(null, e);
            }
        }
    }

    public override void LoadSavesList(System.Action<System.Collections.Generic.IEnumerable<string>, System.Exception> complete) {
        try {
            var filesNames = new List<string>();
            foreach (FileInfo f in files) {
                filesNames.Add(f.Name);
            }
            if (complete != null) {
                complete(filesNames.ToArray(), null);
            }
        } catch (System.Exception e) {
            if (complete != null) {
                complete(null, e);
            }
        }
    }

    public override void Create(string key, Action<bool, Exception> complete) {
        try {
            var fileName = key + "." + savedGamesFileExtension;
            var file = new FileInfo(directory.FullName + "/" + fileName);
            if (!file.Exists) {
                file.Create();
            }
            if (complete != null) {
                complete(true, null);
            }
        } catch (Exception e) {
            if (complete != null) {
                complete(false, e);
            }
        }
    }

    protected DirectoryInfo directory {
        get {
            var dir = new DirectoryInfo(Path.Combine(Application.persistentDataPath, savedGamesPath));
            if (!dir.Exists) {
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, savedGamesPath));
            }
            return dir;
        }
    }

    protected IEnumerable<FileInfo> files {
        get {
            return directory.GetFiles("*." + savedGamesFileExtension);
        }
    }

    protected FileInfo GetFileByName(string fileName) {
        fileName = fileName.Contains("." + savedGamesFileExtension) ? fileName : fileName + "." + savedGamesFileExtension;
        foreach (FileInfo f in files) {
            if (f.Name == fileName) {
                return f;
            }
        }
        var file = new FileInfo(directory.FullName + "/" + fileName);
        if (!file.Exists) {
            file.Create().Close();
        }
        return file;
    }
}
