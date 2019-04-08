using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {

    private UIFrameItem _currentFrameItem;

    public UIFrameItem currentFrameItem {
        get {
            return _currentFrameItem;
        }
    }

    private UIFrame _currentFrame;

    public UIFrame currentFrame {
        get {
            return _currentFrame;
        }
    }

    private static UI _instance;

    public static UI instance {
        get {
            return _instance;
        }
    }

    void Awake() {
        _instance = this;
    }

    public void Show(UIFrameItem newFrame) {
        Show(newFrame, null);
    }

    public void Show(UIFrameItem newFrame, params object[] param) {
        if (_currentFrame != null) {
            Destroy(_currentFrame.gameObject);
        }
        _currentFrameItem = newFrame;
        _currentFrame = _currentFrameItem.LoadFrame();
        _currentFrame.transform.Reset(transform);
        //_currentFrame.transform.localScale = Vector3.one;
        var rectTransform = _currentFrame.transform as RectTransform;
        if (rectTransform != null) {
            rectTransform.localScale = Vector3.one;
            rectTransform.rect.Set(0, 0, 0, 0);
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            //Debug.LogError(rectTransform);
            //rectTransform.anchoredPosition = Vector2.zero;
        }

        if (param != null && param.Length > 0) {
            _currentFrame.SendMessage("OnShowParams", param, SendMessageOptions.DontRequireReceiver);
        } else {
            _currentFrame.SendMessage("OnShow", SendMessageOptions.DontRequireReceiver);
        }
        
    }
}
