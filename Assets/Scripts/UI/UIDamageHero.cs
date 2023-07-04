using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDamageHero : MonoBehaviour
{
    [SerializeField] GeneralData _generalData;
    [SerializeField] TextMeshPro[] _textsDamage;
    [SerializeField] TextMeshPro _textEffect;
    [SerializeField] List<TextLanguage> _textLanguages;
    [SerializeField] float _offset = 10f;
    [SerializeField] float _duration = 1f;
    [SerializeField] float _durationState = 1f;
    Vector3[] _initPositions;
    // Start is called before the first frame update
    void Start()
    {
        _initPositions = new Vector3[_textsDamage.Length];
        for (int i = 0; i < _textsDamage.Length; i++)
        {
            _textsDamage[i].gameObject.SetActive(false);
            _initPositions[i] = _textsDamage[i].rectTransform.localPosition;
            Renderer renderer = _textsDamage[i].gameObject.GetComponent<Renderer>();
            renderer.sortingOrder = 85;
        }
        _textEffect.gameObject.gameObject.GetComponent<Renderer>().sortingOrder = 85;
    }

    public void AddDamage(int damage,Effect effect)
    {
        int index = GetTextDamageAvailable();
        if (index >= _textsDamage.Length)
        {
            Debug.LogWarning("Not enough slot damage available");
        } else
        {
            _textsDamage[index].rectTransform.localPosition = _initPositions[index];
            //_textsDamage[index].transform.localPosition = _initPositions[index];
            _textsDamage[index].color = _generalData.TrapList.GetColorFromEffect(effect);
            _textsDamage[index].gameObject.SetActive(true);
            _textsDamage[index].text = damage.ToString();
            _textsDamage[index].transform.DOMoveY(_textsDamage[index].transform.position.y + _offset, _duration).
                OnComplete(() => {
                    _textsDamage[index].color = Color.black;
                    _textsDamage[index].gameObject.SetActive(false);
                });
        }
    }

    int GetTextDamageAvailable()
    {
        int index = 0;
        while (index < _textsDamage.Length && _textsDamage[index].gameObject.activeSelf)
        {
            index++;
        }
        return index;
    }

    public void AddState(State state)
    {
        _textEffect.text = _textLanguages[(int)state].GetStringInLanguage(GameManager.Instance.LanguageChosen);
        _textEffect.gameObject.SetActive(true);
        _textEffect.transform.DOPunchScale(new Vector3(0.15f, 0.15f, 0.15f), _durationState,5,1).
            OnComplete(() =>
            {
                _textEffect.gameObject.SetActive(false);
            });
        /*_textEffect.transform.DOShakeScale(_durationState).
            OnComplete(() =>
            {
                _textEffect.gameObject.SetActive(false);
            });*/
    }
}

public enum State
{
    DmgReduction = 0,
    Dodge = 1,
    Protected = 2,
    Immune = 3,
    Heal = 4
}