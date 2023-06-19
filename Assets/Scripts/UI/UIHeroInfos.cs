using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHeroInfos : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] TextMeshProUGUI _description;
    [SerializeField] UIHeroSensibilities _heroesSensibilities;
    HeroData[] _heroesData;

    public void ChangeData(int i)
    {
        _heroesData = GameManager.Instance.GetHeroesCurrentLevel();
        ChangeDataForHero(i);
    }

    private void ChangeDataForHero(int index)
    {
        HeroData heroData = _heroesData[index];
        _image.sprite = heroData.sprite;
        _name.text = heroData.heroName;
        _description.text = heroData.description.GetStringInLanguage(GameManager.Instance.LanguageChosen);
        _heroesSensibilities?.ChangeDataForHero(_heroesData[index]);
    }
}
