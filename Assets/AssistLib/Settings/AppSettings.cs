using UnityEngine;
using System.Collections;

public class AppSettings : DBEntry {

    [SerializeField] private SceneItem _startScene;

    public static AppSettings instance {
        get {
            return DB.Get<AppSettings>("AppSettings");
        }
    }

    public SceneItem startScene {
        get {
            return _startScene;
        }
    }

    [SerializeField] private UIFrameItem _startFrame;

    public UIFrameItem startFrame {
        get {
            return _startFrame;
        }
    }
}
