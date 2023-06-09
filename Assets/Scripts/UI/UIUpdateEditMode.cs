using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

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
    [SerializeField] UIHeroes[] _heroesUIEditMode;

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
        _nbActionsLeft.text = nbActions.ToString();
    }
}
