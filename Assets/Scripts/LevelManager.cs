using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] GameObject _levelContainer;
    private int _currentLevelMax = 0;
    private int _previousLevel = -1;
    [SerializeField] GameObject _levelSelectScene;

    public int CurrentLevelMax { 
        get => _currentLevelMax; 
        set => _currentLevelMax = value; 
    }

    void Start()
    {
        GameManager.Instance.OnWin += ChangeNbCurrentLevelMax;
    }

    public void ChangeNbCurrentLevelMax()
    {
        if (_currentLevelMax == GameManager.Instance.Level)
        {
            _currentLevelMax = GameManager.Instance.Level + 1;
        }
    }

    public void UpdateDoors(){
        int currentLevel = _currentLevelMax;
        LevelSelect currentLevelSelect = _levelContainer.transform.GetChild(currentLevel).GetComponent<LevelSelect>();

        LevelSelect roomLevelSelect = null;
        for (int i = 0; i <= currentLevel; i++)
        {
            roomLevelSelect = _levelContainer.transform.GetChild(i).GetComponent<LevelSelect>();
            roomLevelSelect.HideClosedDoor();
            if (i < currentLevel && roomLevelSelect.IsOpen)
            {
                roomLevelSelect.CloseDoorAndRemoveSmoke();
                roomLevelSelect.IsOpen = false;
            }
        }
        if (!currentLevelSelect.IsOpen)
        {
            currentLevelSelect.OpenDoorAndAddSmoke();
            currentLevelSelect.IsOpen = true;
        }
        _previousLevel = GameManager.Instance.Level;
    }

    public void LevelSelectSetActive(bool state){
        _levelSelectScene.SetActive(state);
    }

    public void LoadData(GameData data)
    {
        GameManager.Instance.Level = data.level;
        _currentLevelMax = data.levelMax;
        UpdateDoors();
    }

    public void SaveData(ref GameData data)
    {
        data.level = GameManager.Instance.Level;
        data.levelMax = _currentLevelMax;
    }
}
