using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject _levelContainer;
    [SerializeField] int _currentLevel;
    [SerializeField] GameObject _levelSelectScene;

    void Start()
    {
        UpdateDoors();
    }

    
    public void UpdateCurrentLevel(){
        _currentLevel += 1;
        UpdateDoors();
    }

    private void UpdateDoors(){
        for (int i=0; i<_currentLevel; i++){
            _levelContainer.transform.GetChild(i).GetComponent<LevelSelect>().HideClosedDoor();
        }
        _levelContainer.transform.GetChild(_currentLevel - 1).GetComponent<LevelSelect>().OpenDoorAndAddSmoke();
        if (_currentLevel > 1)
        {
            _levelContainer.transform.GetChild(_currentLevel - 2).GetComponent<LevelSelect>().CloseDoorAndRemoveSmoke();
        }
    }

    public void LevelSelectSetActive(bool state){
        _levelSelectScene.SetActive(state);
    }
}
