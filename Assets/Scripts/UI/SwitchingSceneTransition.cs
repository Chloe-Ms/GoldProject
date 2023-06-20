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
    [SerializeField] LevelManager _levelManager;
    [SerializeField] float _transitionTime = 1f;
    private float _yPosition;
    private float _yPositionChild0;
    private float _yPositionChild1;

    void Start()
    {
        _yPosition = this.GetComponent<RectTransform>().anchoredPosition.y;
        _yPositionChild0 = this.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.y;
        _yPositionChild1 = this.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.y;
        OpenClouds();
    }

    public void CloseCloudsAndLoadScene()
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

    public void CloseCloudsAndLoadLevel(int level)
    {

        this.transform.GetChild(0).transform.DOLocalMoveX(0, _transitionTime, true);
        this.transform.GetChild(1).transform.DOLocalMoveX(0, _transitionTime, true).OnComplete(() => LoadLevel(level));

    }

    public void CloseCloudsAndGoToSelectionLevel()
    {

        this.transform.GetChild(0).transform.DOLocalMoveX(0, _transitionTime, true);
        this.transform.GetChild(1).transform.DOLocalMoveX(0, _transitionTime, true).OnComplete(BackToSelection);

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

    private void LoadLevel(int level)
    {
        GameManager.Instance.LoadLevel(level);
        _levelManager.LevelSelectSetActive(false);
        this.transform.GetChild(0).transform.DOLocalMoveX(_canvaScaler.referenceResolution.x * 1.5f, _transitionTime, true);
        this.transform.GetChild(1).transform.DOLocalMoveX(-_canvaScaler.referenceResolution.x * 1.5f, _transitionTime, true);
    }

    private void BackToSelection()
    {
        _levelManager.LevelSelectSetActive(true);
        this.transform.GetChild(0).transform.DOLocalMoveX(_canvaScaler.referenceResolution.x * 1.5f, _transitionTime, true);
        this.transform.GetChild(1).transform.DOLocalMoveX(-_canvaScaler.referenceResolution.x * 1.5f, _transitionTime, true);
    }
}
