using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    [SerializeField] TextMeshPro _text;
    [SerializeField] SpriteRenderer _sprite;
    [SerializeField] Hero _hero;
    Vector3 _startPosition;
    [SerializeField]Vector3 _offset = Vector3.up;
    [SerializeField] float _timeUIDisplayed = 1f;
    [SerializeField] Sprite[] _images; // [0] = heal; [1] = nothing; [2] = damage

    private void Start()
    {
        _hero.OnTakeDamage += MoveUIDamage;
        _startPosition = _text.transform.position;
    }

    private void MoveUIDamage(int pvLost)
    {
        StartCoroutine(RoutineMoveUI(pvLost));
    }

    IEnumerator RoutineMoveUI(int pvLost)
    {
        pvLost = (-1 * pvLost);
        _text.gameObject.SetActive(true);
        _sprite.sprite = GetImageFromPvLost(pvLost);
        _text.text = pvLost.ToString();
        DOTween.To(() => _startPosition, x => _text.transform.position = x, _startPosition + _offset, _timeUIDisplayed);
        yield return new WaitForSeconds(_timeUIDisplayed);
        _text.gameObject.SetActive(false);
        _text.transform.position = _startPosition;
    }

     Sprite GetImageFromPvLost(int pvLost)
    {
        if (pvLost == 0)
        {
            return _images[1];
        } else if (pvLost > 0)
        {
            return _images[0];
        } else
        {
            return _images[2];
        }
    }
}
