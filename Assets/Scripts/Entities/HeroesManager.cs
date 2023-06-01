using System.Collections.Generic;
using UnityEngine;

public class HeroesManager : MonoBehaviour
{
    HeroData[] _heroesInCurrentLevel;
    List<Hero> _heroes;

    [SerializeField] float _spaceBetweenHeroes = 1f;
    [SerializeField] GameObject _heroPrefab;
    [SerializeField] HeroesSensibility _heroesSensibilities;


    private void Awake()
    {
        _heroes = new List<Hero>();
    }

    private void Start()
    {
        GameManager.Instance.OnEnterEditorMode += OnChangeLevel;
        GameManager.Instance.OnEnterPlayMode += StartPlayMode;
    }

    private void StartPlayMode(int level)
    {
        InstantiateHeroesInLevel(level);
    }

    private void OnChangeLevel(int level)
    {
        _heroesInCurrentLevel = GameManager.Instance.GetHeroesCurrentLevel();
        StartEditMode(level);
    }

    private void StartEditMode(int level)
    {
        RemoveHeroesGameObjects();

        for (int i = 0; i < _heroesInCurrentLevel.Length; i++)
        {
            HeroData hero = _heroesInCurrentLevel[i];
            Debug.Log("Nom : " + hero.heroName + "\n" + "MaxHealth : " + hero.maxHealth);
        }
    }

    
    void InstantiateHeroesInLevel(int level)
    {
        if (_heroesInCurrentLevel.Length >= level)
        {
            for(int i = 0;i < _heroesInCurrentLevel.Length; i++)
            {
                GameObject go = Instantiate(_heroPrefab);
                go.transform.position = go.transform.position + new Vector3(i * _spaceBetweenHeroes, 0,0);
                Hero hero = go?.GetComponent<Hero>();
                hero?.LoadHeroData(_heroesInCurrentLevel[i]);
                if (hero != null)
                {
                    _heroes.Add(hero);
                }
            }
        }
    }
    public void RemoveHeroesGameObjects()
    {
        for (int i = _heroes.Count - 1; i >= 0; i--)
        {
            Destroy(_heroes[i].gameObject);
        }
        _heroes.Clear();
    }

    public void ApplyDamageToEachHero(int damage)
    {
        foreach (Hero hero in _heroes)
        {
            if (! hero.IsDead)
                hero.TakeDamage(damage);
        }
    }

    /*public void ApplyEffectToEachHero(Effect effect)
    {

    }*/
}
