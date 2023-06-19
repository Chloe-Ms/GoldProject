using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Tutorial Data", menuName = "GoldProject/Tutorial Data", order = 5)]
public class TutorialData : ScriptableObject
{
    [SerializeField] private TextLanguage _name;
    [SerializeField] private TextLanguage[] _description;

    public TextLanguage Name
    {
        get { return _name; }
    }


    public TextLanguage[] Description
    {
        get => _description;
    }
}

