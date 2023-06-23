using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIBGEffectImage : MonoBehaviour
{
    [SerializeField] Image _spriteBgEffectUI;
    [SerializeField] RectTransform _rectTransform;
    [SerializeField] Animator _animator;
    [SerializeField] GeneralData _generalData;
    [SerializeField] int _direction;
    Vector2 _scrollOffset;
    [SerializeField] float _durationAnim = 0.5f;
    [SerializeField] float _interval = 0.2f;
    Vector2 _endPosition;

    void Start()
    {
        GameManager.Instance.OnEffectApplied += OnEffectApplied;
        _scrollOffset = new Vector2(Screen.width / 2,0);
    }

    private void OnEffectApplied(Effect effect)
    {
        _spriteBgEffectUI.sprite = _generalData.TrapList.GetImageBgFromEffect(effect);
        if (_spriteBgEffectUI.sprite == null)
        {
            _animator.runtimeAnimatorController = null;
            _spriteBgEffectUI.color = new Color(1,1,1,0);
        } else
        {
            _animator.runtimeAnimatorController = _generalData.TrapList.GetAnimationBgFromEffect(effect);
            _spriteBgEffectUI.color = new Color(1, 1, 1, 1);
            _endPosition = _rectTransform.anchoredPosition;
            _rectTransform.anchoredPosition = _endPosition + (_direction * _scrollOffset);
            //_rectTransform.DOAnchorPos(_endPosition, _durationAnim);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_rectTransform.DOAnchorPos(_endPosition, _durationAnim)).
                AppendInterval(_interval).
                Append(_rectTransform.DOAnchorPos(_endPosition + (_direction * _scrollOffset), _durationAnim)).
                OnComplete(() => { 
                    _spriteBgEffectUI.color = new Color(1, 1, 1, 0);
                    _rectTransform.anchoredPosition = _endPosition;
                });
        }
    }
}
