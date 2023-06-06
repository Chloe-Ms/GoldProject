using TMPro;
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
    }

    public void UpdateNbActionsLeft(int nbActions)
    {
        _nbActionsLeft.text = nbActions.ToString();
    }
}
