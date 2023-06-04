using System;
using System.Data;
using UnityEngine;

public class HeroesManager : MonoBehaviour
{
    HeroData[] _heroesDataInCurrentLevel;
    Group _heroesInCurrentLevel = new Group();

    [SerializeField] float _spaceBetweenHeroes = 1f;
    [SerializeField] GameObject _heroPrefab;
    [SerializeField] HeroesSensibility _heroesSensibilities;
    [SerializeField] int _poisonDamageMultiplier = 2;
    int _nbHeroesLeft;
    public Group HeroesInCurrentLevel { 
        get => _heroesInCurrentLevel; 
        set => _heroesInCurrentLevel = value; 
    }
    public int NbHeroesLeft { 
        get => _nbHeroesLeft; 
        set => _nbHeroesLeft = value; 
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
        _heroesDataInCurrentLevel = GameManager.Instance.GetHeroesCurrentLevel();
        StartEditMode(level);
    }

    private void StartEditMode(int level)
    {
        RemoveHeroesGameObjects();
        _heroesInCurrentLevel.Init();
        _nbHeroesLeft = _heroesDataInCurrentLevel.Length;
        Debug.Log("HEROES \n");
        for (int i = 0; i < _heroesDataInCurrentLevel.Length; i++)
        {
            HeroData hero = _heroesDataInCurrentLevel[i];
            Debug.Log("Nom : " + hero.heroName + "\n" + "MaxHealth : " + hero.maxHealth);
        }
    }

    
    void InstantiateHeroesInLevel(int level)
    {
        for(int i = 0;i < _heroesDataInCurrentLevel.Length; i++)
        {
            GameObject go = Instantiate(_heroPrefab);
            go.transform.position = go.transform.position + new Vector3(i * _spaceBetweenHeroes, 0,0);
            Hero hero = go?.GetComponent<Hero>();
            hero?.LoadHeroData(_heroesDataInCurrentLevel[i]);
            if (hero != null)
            {
                _heroesInCurrentLevel.Heroes[i].OnHeroDeath += OnAnyHeroDeath;
                _heroesInCurrentLevel.Heroes.Add(hero);
            }
        }
    }

    private void OnAnyHeroDeath(Hero hero)
    {
        AbilityManager.DeactivateAbilities[hero.Role].Invoke(_heroesInCurrentLevel);
        _nbHeroesLeft--;
        if (_nbHeroesLeft <= 0)
        {
            GameManager.Instance.PlayerWin();
        }
        if (_heroesInCurrentLevel.AffectedByPlants)
        {
            ApplyDamageToEachHero(Effect.PLANTE);
        }
    }

    public void RemoveHeroesGameObjects()
    {
        if (_heroesInCurrentLevel != null)
        {
            for (int i = _heroesInCurrentLevel.Heroes.Count - 1; i >= 0; i--)
            {
                _heroesInCurrentLevel.Heroes[i].OnHeroDeath -= OnAnyHeroDeath;
                Destroy(_heroesInCurrentLevel.Heroes[i].gameObject);
            }
            _heroesInCurrentLevel.Heroes.Clear();
        }
    }

    public void ApplyDamageToEachHero(Effect effect)
    {
        if (!_heroesInCurrentLevel.IsInvulnerable)
        {
            foreach (Hero hero in _heroesInCurrentLevel.Heroes)
            {
                if (!hero.IsDead)
                {
                    Hero heroAttacked = hero;
                    if (heroAttacked.Isinvulnerable)
                    {
                        heroAttacked = _heroesInCurrentLevel.GetHeroWithRole(Role.PALADIN);
                    }
                    int damage = _heroesSensibilities.GetSensibility(effect, heroAttacked.Role);

                    if (_heroesInCurrentLevel.IsPoisoned)
                    {
                        damage *= _poisonDamageMultiplier;
                    }
                    heroAttacked.UpdateHealth(damage);
                }
                Debug.Log("Hero " + hero.Role + " " + hero.Health);
            }
        }
    }

    public void ApplyAbilities(Trap trap)
    {
        foreach (Hero hero in _heroesInCurrentLevel.Heroes)
        {
            if (!hero.IsDead)
            {
                AbilityManager.ActivateAbilities[hero.Role]?.Invoke(_heroesInCurrentLevel,trap);
            }
        }
    }

    public void RemoveAbilities(Trap trap)
    {
        foreach (Hero hero in _heroesInCurrentLevel.Heroes)
        {
            if (!hero.IsDead)
            {
                AbilityManager.DeactivateAbilities[hero.Role]?.Invoke(_heroesInCurrentLevel);
            }
        }
    }

    public int GetSensibility(Effect effect, Role role)
    {
        return _heroesSensibilities.GetSensibility(effect, role);
    }
}
