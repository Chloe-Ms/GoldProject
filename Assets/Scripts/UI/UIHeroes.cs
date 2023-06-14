using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHeroes : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] TextMeshProUGUI _text;
    HeroData[] _heroesData;
    [SerializeField] UIHeroSensibilities _heroesSensibilities;

    public void ChangeData(int i)
    {
        _heroesData = GameManager.Instance.GetHeroesCurrentLevel();
        ChangeDataForHero(i);
    }

    private void ChangeDataForHero(int index)
    {
        if (_heroesData[index].headSprite != null)
        {
            _image.sprite = _heroesData[index].headSprite;
        } else
        {
            _image.color = _heroesData[index].color;
        }
        
        _text.text = _heroesData[index].maxHealth.ToString();

        _heroesSensibilities?.ChangeDataForHero(_heroesData[index]);
    }
}
