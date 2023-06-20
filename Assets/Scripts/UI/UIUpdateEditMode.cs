using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class UIUpdateEditMode : MonoBehaviour
{
    private static UIUpdateEditMode _instance;

    public static UIUpdateEditMode Instance
    {
        get => _instance;
        private set => _instance = value;
    }

    [SerializeField] TextMeshProUGUI _nbActionsLeft;
    [SerializeField] TextMeshProUGUI _nbActionsTotal;
    [SerializeField] Color _noActionsLeftTextColor;
    [SerializeField] Color _ActionsLeftTextBaseColor;
    [SerializeField] UIHeroes[] _heroesUIEditMode;
    private Vector3 _initPos;
    private bool _posHasBeenInitiated = false;

    

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple instances of Game Manager in the scene.");
            Destroy(gameObject);
        }
    }
    public void Init(int nbActions)
    {
        DOTween.KillAll();
        _nbActionsLeft.color = _ActionsLeftTextBaseColor;
        _nbActionsLeft.fontSize = 100;
        _nbActionsTotal.text = nbActions.ToString();
        _nbActionsLeft.text = nbActions.ToString();
        for (int i = 0;i < _heroesUIEditMode.Length;i++)
        {
            if (GameManager.Instance.GetHeroesCurrentLevel().Length > i)
            {
                _heroesUIEditMode[i].gameObject.SetActive(true);
                _heroesUIEditMode[i].ChangeData(i);
            } else
            {
                _heroesUIEditMode[i].gameObject.SetActive(false);

            }
        }
    }

    public void UpdateNbActionsLeft(int nbActions)
    {
        Debug.Log(_initPos);
        if (!_posHasBeenInitiated)
        {
            _initPos = _nbActionsLeft.gameObject.transform.position;
            _posHasBeenInitiated = true;
        }
        DOTween.KillAll();
        _nbActionsLeft.gameObject.transform.position = _initPos;
        _nbActionsLeft.text = nbActions.ToString();
        DG.Tweening.Sequence _nbLeftSequence = DOTween.Sequence();

        if(nbActions == 0)
        {
            _nbActionsLeft.color = _noActionsLeftTextColor;
            _nbActionsLeft.fontSize = 125;
            _nbLeftSequence.Append(_nbActionsLeft.gameObject.transform.DOShakePosition(0.5f, 25f, 20));
            _nbLeftSequence.Append(_nbActionsLeft.gameObject.transform.DOLocalMoveY(10f, 0.5f, true).SetLoops(-1, LoopType.Yoyo));
        }
        else
        {
            DOTween.KillAll();
            _nbActionsLeft.gameObject.transform.DOShakePosition(0.2f, 10f, 10);
            _nbActionsLeft.color = _ActionsLeftTextBaseColor;
            _nbActionsLeft.fontSize = 100;
        }
    }
}
