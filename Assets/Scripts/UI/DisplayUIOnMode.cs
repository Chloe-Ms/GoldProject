using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayUIOnMode : MonoBehaviour
{
    [SerializeField] GameObject[] _gameObjectsInEditMode;
    [SerializeField] GameObject[] _gameObjectsInPlayMode;
    void Start()
    {
        GameManager.Instance.OnEnterPlayMode += EnterPlayMode;
        GameManager.Instance.OnEnterEditorMode += EnterEditMode;
    }

    private void EnterPlayMode(int obj)
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

    private void EnterEditMode(int obj)
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
