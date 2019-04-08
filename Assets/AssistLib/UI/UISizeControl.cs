using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class UISizeControl : MonoBehaviour {

    [SerializeField] private float _width;
    [SerializeField] private float _height;

    [SerializeField] private float _x;
    [SerializeField] private float _y;

    private Transform _transform;

    void OnEnable() {
        _transform = transform;
    }

	void Update () {
        if (_width > 0 && _width * Screen.width != _transform.localScale.x) {
            var oldSize = _transform.localScale;
            _transform.localScale = new Vector3(Screen.width * _width, oldSize.y, oldSize.z);
        }

        if (_height > 0 && _height * Screen.height != _transform.localScale.y) {
            var oldSize = _transform.localScale;
            _transform.localScale = new Vector3(oldSize.x, Screen.height * _height, oldSize.z);
        }

        if (_x * Screen.width != _transform.localPosition.x) {
            var oldPosition = _transform.localPosition;
            _transform.localPosition = new Vector3(_x * Screen.width, oldPosition.y, oldPosition.z);
        }

        if (_y * Screen.height != _transform.localPosition.y) {
            var oldPosition = _transform.localPosition;
            _transform.localPosition = new Vector3(oldPosition.x, _x * Screen.height, oldPosition.z);
        }
	}
}
