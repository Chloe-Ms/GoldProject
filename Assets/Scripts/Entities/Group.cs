using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
    private List<Hero> _heroes;
    private int _nbLeversLeft;
    private bool _isPoisoned;

    public List<Hero> Heroes {
        get => _heroes;
        set => _heroes = value; 
    }
    public bool IsPoisoned {
        get => _isPoisoned;
        set => _isPoisoned = value; 
    }

    private void Awake()
    {
        _heroes = new List<Hero>();
    }

    public void Init()
    {
        _isPoisoned = false;
    }
}
