using System.Collections.Generic;
using UnityEngine;

public class HeroesManager : MonoBehaviour
{
    [SerializeField] Group[] _groupPerLevel;
    [SerializeField] float _spaceBetweenHeroes = 1f;
    List<Hero> _heroes;

    private void Awake()
    {
        _heroes = new List<Hero>();
    }

    private void Start()
    {
        GameManager.Instance.OnEnterEditorMode += OnEnterEditorMode;
        GameManager.Instance.OnEnterEditorMode += OnEnterPlayMode;
    }

    private void OnEnterPlayMode(int level)
    {
        InstantiateHeroesInLevel(level);
    }

    private void OnEnterEditorMode(int level)
    {
        if (_groupPerLevel.Length >= level)
        {
            for (int i = 0; i < _groupPerLevel[level - 1].ListHeroesInGroup.Length; i++)
            {
                Hero hero = _groupPerLevel[level - 1].ListHeroesInGroup[i]?.GetComponent<Hero>();
                Debug.Log("Nom : "+hero.HeroName+"\n"+ "MaxHealth : " + hero.MaxHealth);
            }
        }
    }

    private void OnValidate()
    {
        for (int i = 0; i < _groupPerLevel.Length; i++)
        {
            _groupPerLevel[i].name = "Level " + (i + 1);
        }
    }
    void InstantiateHeroesInLevel(int level)
    {
        for (int i = _heroes.Count - 1; i >= 0; i--)
        {
            Destroy(_heroes[i].gameObject);
        }
        _heroes.Clear();
        if (_groupPerLevel.Length >= level)
        {
            for(int i = 0;i < _groupPerLevel[level - 1].ListHeroesInGroup.Length; i++)
            {
                GameObject go = Instantiate(_groupPerLevel[level - 1].ListHeroesInGroup[i]);
                go.transform.position = go.transform.position + new Vector3(i * _spaceBetweenHeroes, 0,0);
                Hero hero = go?.GetComponent<Hero>();
                if (hero != null)
                {
                    _heroes.Add(hero);
                }
            }
        }
    }
}
