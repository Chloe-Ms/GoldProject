using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Tutorial Data", menuName = "GoldProject/Tutorial Data", order = 5)]
public class TutorialData : ScriptableObject
{
    [SerializeField] private string _name;
    [ResizableTextArea, SerializeField] private string[] _description;

    public string Name
    {
        get { return _name; }
    }


    public string[] Description
    {
        get => _description;
    }
}

