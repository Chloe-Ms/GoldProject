using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SwitchingSceneTransition : MonoBehaviour
{

    private float _screenWidth = Screen.width;
    [SerializeField] CanvasScaler _canvaScaler;
    [SerializeField] float _transitionTime = 1f;
    private float _yPosition;

    void Start()
    {
        _yPosition = this.GetComponent<RectTransform>().anchoredPosition.y;
        OpenClouds();
    }

    public void CloseClouds()
    {
        if (this.name == "LeftClouds")
        {
            this.transform.DOLocalMove(new Vector2(0, _yPosition), _transitionTime, true).onComplete = LoadSelectionScene;
        }
        if (this.name == "RightClouds")
        {
            this.transform.DOLocalMove(new Vector2(0, _yPosition), _transitionTime, true);
        }
    }

    private void OpenClouds()
    {
        if (this.name == "LeftClouds")
        {
            this.transform.DOLocalMove(new Vector2(_canvaScaler.referenceResolution.x * 1.5f, _yPosition), _transitionTime, true);
        }
        if (this.name == "RightClouds")
        {
            this.transform.DOLocalMove(new Vector2(-_canvaScaler.referenceResolution.x * 1.5f, _yPosition), _transitionTime, true);
        }
    }

    private void LoadSelectionScene()
    {
        if (SceneManager.GetActiveScene().name == "MenuScene")
        {
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}
