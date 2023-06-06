using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class HeroesManager : MonoBehaviour
{
    HeroData[] _heroesDataInCurrentLevel;
    Group _heroesInCurrentLevel = new Group();

    [SerializeField] float _spaceBetweenHeroes = 1f;
    [SerializeField] GameObject _heroPrefab;
    [SerializeField] GameObject _groupGO;
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
    public GameObject GroupParent { 
        get => _groupGO;
    }

    private void Start()
    {
        GameManager.Instance.OnEnterPlayMode += StartPlayMode;
    }

    private void StartPlayMode(int level)
    {
        InstantiateHeroesInLevel(level);
    }

    public void OnChangeLevel(int level)
    {
        _heroesDataInCurrentLevel = GameManager.Instance.GetHeroesCurrentLevel();
        int[] maxHealth = GameManager.Instance.MaxHealthCurrentLevel();
        for (int i = 0; i < maxHealth.Length; i++) {
            _heroesDataInCurrentLevel[i].maxHealth = maxHealth[i];
        }
        
        Debug.Log("Heroes dans le niveau " + _heroesDataInCurrentLevel.Length);
        StartEditMode(level);
    }

    private void StartEditMode(int level)
    {
        RemoveHeroesGameObjects();
        _heroesInCurrentLevel.Init();
        _nbHeroesLeft = _heroesDataInCurrentLevel.Length;
    }

    
    void InstantiateHeroesInLevel(int level)
    {
        for(int i = 0;i < _heroesDataInCurrentLevel.Length; i++)
        {
            GameObject go = Instantiate(_heroPrefab);
            go.transform.position = go.transform.position + new Vector3(i * _spaceBetweenHeroes, 0,0);
            go.transform.parent = _groupGO.transform;
            Hero hero = go?.GetComponent<Hero>();
            hero?.LoadHeroData(_heroesDataInCurrentLevel[i]);
            if (hero != null)
            {
                hero.OnHeroDeath += OnAnyHeroDeath;
                _heroesInCurrentLevel.Heroes.Add(hero);
            }
        }
        UIUpdatePlayMode.Instance.Init();
    }

    private void OnAnyHeroDeath(Hero hero)
    {
        if (AbilityManager.ActivateAbilities.ContainsKey(hero.Role))
        {
            AbilityManager.DeactivateAbilities[hero.Role].Invoke(_heroesInCurrentLevel);
        }
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
            Debug.Log("DAMAGE on group");
            foreach (Hero hero in _heroesInCurrentLevel.Heroes)
            {
                Debug.Log("Before Hero " + hero.Role + " " + hero.Health);
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
                Debug.Log("After Hero " + hero.Role + " " + hero.Health);
            }
        }
    }

    public void ApplyAbilities(Room room)
    {
        foreach (Hero hero in _heroesInCurrentLevel.Heroes)
        {
            if (!hero.IsDead)
            {
                if (AbilityManager.ActivateAbilities.ContainsKey(hero.Role))
                {
                    AbilityManager.ActivateAbilities[hero.Role]?.Invoke(_heroesInCurrentLevel,room);
                    Debug.Log("APPLY ABILITY : " + hero.Role);
                }
            }
        }
    }

    public void RemoveAbilities(Room room)
    {
        foreach (Hero hero in _heroesInCurrentLevel.Heroes)
        {
            if (!hero.IsDead)
            {
                if (AbilityManager.ActivateAbilities.ContainsKey(hero.Role))
                {
                    AbilityManager.DeactivateAbilities[hero.Role]?.Invoke(_heroesInCurrentLevel);
                }
            }
        }
    }

    public int GetSensibility(Effect effect, Role role)
    {
        return _heroesSensibilities.GetSensibility(effect, role);
    }
}
