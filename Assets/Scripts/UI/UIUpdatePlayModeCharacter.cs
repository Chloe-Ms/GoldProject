using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdatePlayModeCharacter : MonoBehaviour
{
    [SerializeField] Image _heroHead;
    [SerializeField] TextMeshProUGUI _health;
    [SerializeField] TextMeshProUGUI _maxHealth;
    [SerializeField] TextMeshProUGUI _feedbackPV;
    [SerializeField] float _offsetYDamage = 0.5f;
    [SerializeField, Min(0.1f)] float _durationMovementDamage = 1f;
    float _startPosY;

    private void Start()
    {
        if (_feedbackPV != null)
        {
            _startPosY = _feedbackPV.transform.position.y;
        }
    }
    public void Init(Hero hero)
    {
        _heroHead.sprite = hero.HeadSprite;
        _maxHealth.text = hero.MaxHealth.ToString();
        _health.text = hero.MaxHealth.ToString();
        _feedbackPV.gameObject.SetActive(false);
    }
    public void UpdateHealth(int health,int damageTaken)
    {
        _health.text = health.ToString();
        StartFeedBack(damageTaken);
    }

    public void StartFeedBack(int damageTaken)
    {
        _feedbackPV.gameObject.SetActive(true);
        _feedbackPV.text = damageTaken.ToString();
        _feedbackPV.transform.DOMoveY(_startPosY+ _offsetYDamage,_durationMovementDamage)
            .OnComplete(() =>
            {
                _feedbackPV.gameObject.SetActive(false);
                _feedbackPV.transform.position = new Vector3(
                    _feedbackPV.transform.position.x, 
                    _startPosY, 
                    _feedbackPV.transform.position.z);
            });
        _feedbackPV.DOFade(0, _durationMovementDamage).SetEase(Ease.InExpo).OnComplete(() =>
        {
            _feedbackPV.alpha = 1;
        });
    }
}
