using System;
using UnityEngine;

public class UIHeroSensibilities : MonoBehaviour
{
    [SerializeField] GameObject _imageIconPrefab;
    [SerializeField] GameObject _imagePositiveList;
    [SerializeField] GameObject _imageNeutralList;
    [SerializeField] GameObject _imageNegativeList;

    public void ChangeDataForHero(HeroData heroData)
    {
        ClearLists();
        foreach (Effect effect in Enum.GetValues(typeof(Effect)))
        {
            if (effect != Effect.NONE)
            {
                int sensibility = GameManager.Instance.GetHeroesSensibility(effect, heroData.role);
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
