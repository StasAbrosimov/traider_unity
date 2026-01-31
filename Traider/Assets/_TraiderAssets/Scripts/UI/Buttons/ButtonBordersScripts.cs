using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonBordersScripts : MonoBehaviour
{
    [SerializeField]
    private RectTransform _left, _right, _top, _bottom;

    [SerializeField]
    private float _verticalShift, _horizontalShift, _defaultValue;

    [SerializeField]
    private RectTransform _contentContainer;

    [SerializeField]
    private Vector3 _shrinckContentCoef, _defaultContentSize;

    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
    }

    public void OnItemPreset()
    {
        if (_button != null && _button.interactable)
        {
            AddShift(_verticalShift, _horizontalShift);
        }
        else
        {
            ResetLayout();
        }
    }

    public void OnItemReleased()
    {
        ResetLayout();
    }

    private void AddShift(float vshift, float hshift)
    {
        _top.sizeDelta = new Vector2(_top.sizeDelta.x, _top.sizeDelta.y + hshift);
        _bottom.sizeDelta = new Vector2(_bottom.sizeDelta.x, _bottom.sizeDelta.y + hshift);

        _left.sizeDelta = new Vector2(_left.sizeDelta.x + vshift, _left.sizeDelta.y);
        _right.sizeDelta = new Vector2(_right.sizeDelta.x + vshift, _right.sizeDelta.y);

        if(_contentContainer != null)
        {
            _contentContainer.localScale = new Vector3(_contentContainer.localScale.x * _shrinckContentCoef.x,
                                                       _contentContainer.localScale.y * _shrinckContentCoef.y,
                                                       _contentContainer.localScale.z * _shrinckContentCoef.z);
        }
    }

    private void ResetLayout()
    {
        _top.sizeDelta = new Vector2(_top.sizeDelta.x, _defaultValue);
        _bottom.sizeDelta = new Vector2(_bottom.sizeDelta.x, _defaultValue);

        _left.sizeDelta = new Vector2(_defaultValue, _left.sizeDelta.y);
        _right.sizeDelta = new Vector2(_defaultValue, _right.sizeDelta.y);

        if (_contentContainer != null)
        {
            _contentContainer.localScale = new Vector3(_defaultContentSize.x,
                                                       _defaultContentSize.y,
                                                       _defaultContentSize.z);
        }
    }
}
