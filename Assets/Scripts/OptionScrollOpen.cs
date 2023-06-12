using UnityEngine;
using DG.Tweening;

public class OptionScrollOpen : MonoBehaviour
{
    Vector2 _scrollOffset = new Vector2(0, 100);
    Vector2 _endPosition;
    [SerializeField] RectTransform _rectTransform;

    public void OpenScroll(){
        if (this.gameObject.name == "ScrollUp"){
            _endPosition = _rectTransform.anchoredPosition;
            _rectTransform.anchoredPosition = _endPosition - _scrollOffset;
            _rectTransform.DOAnchorPos(_endPosition, 0.9f, true);
        }
        if (this.gameObject.name == "ScrollDown")
        {
            _endPosition = _rectTransform.anchoredPosition;
            _rectTransform.anchoredPosition = _endPosition + _scrollOffset;
            _rectTransform.DOAnchorPos(_endPosition, 0.9f, true);
        }
        if (this.gameObject.name == "ScrollMiddle"){
            this.transform.localScale = new Vector2(1, 0.2f );
            this.transform.DOScaleY(1f, 0.9f);
        }
    }
}
