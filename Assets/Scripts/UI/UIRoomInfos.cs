using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomInfos : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] TextMeshProUGUI _description;

    public void Init(TrapData data)
    {
        _image.sprite = data.SpriteUI;
        _name.text = data.Name;
        _description.text = data.Description;
    }
}
