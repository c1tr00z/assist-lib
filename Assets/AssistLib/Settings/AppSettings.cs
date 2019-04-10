using c1tr00z.AssistLib.UI;
using UnityEngine;

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

    [SerializeField] private UIFrameDBEntry _startFrame;

    public UIFrameDBEntry startFrame {
        get {
            return _startFrame;
        }
    }
}
