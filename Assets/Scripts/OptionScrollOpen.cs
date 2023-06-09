using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OptionScrollOpen : MonoBehaviour
{
    void Start()
    {
        DOTween.Init();
    }

    public void OpenScroll(){
        if (this.gameObject.name == "ScrollUp"){
            this.transform.position = new Vector2(540, 1100);
            this.transform.DOMove(new Vector2(540, 1500), 0.9f, true);
        }
        if (this.gameObject.name == "ScrollDown"){
            this.transform.position = new Vector2(540, 800);
            this.transform.DOMove(new Vector2(540, 400), 0.9f, true);
        }
        if (this.gameObject.name == "ScrollMiddle"){
            Debug.Log("non");
            this.transform.localScale = new Vector2(1, 0.2f );
            this.transform.DOScaleY(1f, 0.9f);
        }
    }
}
