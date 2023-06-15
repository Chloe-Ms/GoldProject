using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHero : MonoBehaviour
{
    [SerializeField] GeneralData _generalData;
    [SerializeField] Image _image;
    public void ChangeData(Effect effect, int sensibility)
    {
        _image.sprite = _generalData.TrapList.GetSpriteFromEffect(effect);
    }
}
