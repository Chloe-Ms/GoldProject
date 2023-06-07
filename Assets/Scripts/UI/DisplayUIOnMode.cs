using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayUIOnMode : MonoBehaviour
{
    [SerializeField] GameObject[] _gameObjectsInEditMode;
    [SerializeField] GameObject[] _gameObjectsInPlayMode;

    public void EnterPlayMode()
    {
        for(int i = 0;  i < _gameObjectsInEditMode.Length; i++)
        {
            _gameObjectsInEditMode[i].SetActive(false);
        }
        for (int i = 0; i < _gameObjectsInPlayMode.Length; i++)
        {
            _gameObjectsInPlayMode[i].SetActive(true);
        }
    }

    public void EnterEditMode()
    {
        for (int i = 0; i < _gameObjectsInEditMode.Length; i++)
        {
            _gameObjectsInEditMode[i].SetActive(true);
        }
        for (int i = 0; i < _gameObjectsInPlayMode.Length; i++)
        {
            _gameObjectsInPlayMode[i].SetActive(false);
        }
    }
}
