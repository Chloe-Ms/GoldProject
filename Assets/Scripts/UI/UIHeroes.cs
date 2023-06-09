using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHeroes : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] GameObject _imageIconPrefab;
    HeroData[] _heroesData;
    [SerializeField] GameObject _imagePositiveList;
    [SerializeField] GameObject _imageNeutralList;
    [SerializeField] GameObject _imageNegativeList;

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
        ClearLists();
        foreach (Effect effect in Enum.GetValues(typeof(Effect)))
        {
            if (effect != Effect.NONE)
            {
                int sensibility = GameManager.Instance.GetHeroesSensibility(effect, _heroesData[index].role);
                GameObject parentObject = null; ;
                if (sensibility != -1)
                {
                    switch (sensibility)
                    {
                        case 1:
                            parentObject = _imagePositiveList;
                            break;
                        case 0:
                            parentObject = _imageNeutralList;
                            break;
                        case -2:
                            parentObject = _imageNegativeList;
                            break;
                    }
                    
                    GameObject go = Instantiate(_imageIconPrefab, parentObject.transform);
                    go.GetComponent<UIHero>()?.ChangeData(effect, sensibility);

                }
            }
        }
    }

    private void ClearLists()
    {
        ClearList(_imagePositiveList);
        ClearList(_imageNeutralList);
        ClearList(_imageNegativeList);
    }

    public void ClearList(GameObject listParent)
    {
        foreach (Transform child in listParent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
