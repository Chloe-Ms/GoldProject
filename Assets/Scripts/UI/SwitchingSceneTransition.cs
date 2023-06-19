using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SwitchingSceneTransition : MonoBehaviour
{
    [SerializeField] private bool _inMenuScene;

    public void SwitchTransition()
    {
        if (_inMenuScene)
        {
            if (this.name == "LeftClouds")
            {
                this.transform.DOLocalMove(new Vector2(0, 37.4f), 1f, true).onComplete = LoadSelectionScene;
            }
            if (this.name == "RightClouds")
            {
                this.transform.DOLocalMove(new Vector2(0, -65), 1f, true);
            }
        }
    }

    private void LoadSelectionScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
