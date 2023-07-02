using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class UIUpdatePlayMode : MonoBehaviour
{
    [SerializeField] List<UIUpdatePlayModeCharacter> _listCharacter;
    [SerializeField] HeroesManager _heroesManager; //Dependant :(

    private static UIUpdatePlayMode _instance;
    public static UIUpdatePlayMode Instance
    {
        get => _instance;
        private set => _instance = value;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple instances of UI Manager in the scene.");
            Destroy(gameObject);
        }
    }
    public void Init()
    {
        if (GameManager.Instance.GetHeroesCurrentLevel().Length <= _listCharacter.Count)
        {
            for (int i = 0; i < _listCharacter.Count; i++)
            {
                if ( i < _heroesManager.HeroesInCurrentLevel.Heroes.Count)
                {
                    _listCharacter[i].gameObject.SetActive(true);
                    _listCharacter[i].Init(_heroesManager.HeroesInCurrentLevel.Heroes[i]);
                } else
                {
                    _listCharacter[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void UpdateHero(Hero hero,int damage)
    {
        if (hero != null) 
        {
            int index = _heroesManager.HeroesInCurrentLevel.GetHeroIndexWithRole(hero.Role);
            _listCharacter[index].UpdateHealth(hero.Health,damage);
        }
    }
}
