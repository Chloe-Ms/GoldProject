using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

public class UIHeroSensibilities : MonoBehaviour
{
    [SerializeField] GameObject _imageIconPrefab;
    [SerializeField] GameObject _imagePositiveList;
    [SerializeField] GameObject _imageNeutralList;
    [SerializeField] GameObject _imageNegativeList;

    public void ChangeDataForHero(HeroData heroData)
    {
        ClearLists();
        int indexNbTraps = 0;
        int indexTrapList = 0;
        List<TrapData> trapList = GameManager.Instance.GeneralData.TrapList.TrapData;
        int[] nbOfTraps = GameManager.Instance.GetListTrapsCurrentLevel();
        while (indexTrapList < trapList.Count)
        {
            if (trapList[indexTrapList].RoomType == RoomType.NORMAL)
            {
                Effect effect = trapList[indexTrapList].Effect;
                if (indexNbTraps < nbOfTraps.Length && nbOfTraps[indexNbTraps] != 0 && effect != Effect.NONE)
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
                indexNbTraps++;
            }
            indexTrapList++;
        }
        /*foreach (TrapData trap in trapList)
        {
            if (trap.RoomType == RoomType.NORMAL)
            {
                if (index < nbOfTrap.Length && nbOfTrap[index] == 0)
                {
                    index++;
                }
        }*/
        /*foreach (Effect effect in Enum.GetValues(typeof(Effect)))
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
        }*/
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
