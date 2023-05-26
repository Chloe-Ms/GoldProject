using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;

    public static MapManager Instance
    {
        get { return _instance; }
    }

    private int _buyableRoomCount = 5;
    private int _currentRoomCount = 0;

    public int BuyableRoomCount
    {
        get { return _buyableRoomCount - _currentRoomCount; }
    }

    [SerializeField] private TMP_Text _roomText;

    private List<GameObject> _slots = new List<GameObject>();

    public List<GameObject> SlotsList
    {
        get { return _slots; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;
    }

    private void Update()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        _roomText.text = $"You have {BuyableRoomCount} rooms buyable";
    }
}