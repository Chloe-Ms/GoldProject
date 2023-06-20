using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class NicocoInk : MonoBehaviour
{
    
    void Start()
    {
        DG.Tweening.Sequence _sequence = DOTween.Sequence();
        _sequence.Append(this.GetComponent<Image>().DOFade(1, 2));
        _sequence.Append(this.GetComponent<Image>().DOFade(0, 2));
        _sequence.AppendCallback(LoadMainMenu);
    }

    
    void LoadMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
