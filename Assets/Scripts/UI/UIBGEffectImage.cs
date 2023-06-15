using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIBGEffectImage : MonoBehaviour
{
    [SerializeField] Image _spriteBgEffectUI;
    [SerializeField] RectTransform _rectTransform;
    [SerializeField] GeneralData _generalData;
    [SerializeField] Vector2 _scrollOffset = new Vector2(100,0);
    [SerializeField] float _durationAnim = 0.5f;
    Vector2 _endPosition;

    void Start()
    {
        GameManager.Instance.OnEffectApplied += OnEffectApplied;
    }

    private void OnEffectApplied(Effect obj)
    {
        _spriteBgEffectUI.sprite = _generalData.TrapList.GetImageBgFromEffect(obj);
        if (_spriteBgEffectUI.sprite == null)
        {
            _spriteBgEffectUI.color = new Color(1,1,1,0);
        } else
        {
            _spriteBgEffectUI.color = new Color(1, 1, 1, 1);
            _endPosition = _rectTransform.anchoredPosition;
            _rectTransform.anchoredPosition = _endPosition + _scrollOffset;
            _rectTransform.DOAnchorPos(_endPosition, _durationAnim, true);
        }
    }
}
