using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITutoInfos : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] TextMeshProUGUI _description;
    TutorialData _tutoData;
    private int _page;

    public void BeginReadingTutoData()
    {
        _page = 0;
        _tutoData = GameManager.Instance.GetTutorialData();
        _name.text = _tutoData.Name.GetStringInLanguage(GameManager.Instance.LanguageChosen);
        _description.text = _tutoData.Description[_page].GetStringInLanguage(GameManager.Instance.LanguageChosen);
    }

    public bool CanChangeTutoData()
    {
        if (_page + 1 >= _tutoData.Description.Length)
        {
            return false;
        }
        return true;
    }

    public void ChangeTutoData()
    {
        _page++;
        _description.text = _tutoData.Description[_page].GetStringInLanguage(GameManager.Instance.LanguageChosen);
        //son
    }
}
