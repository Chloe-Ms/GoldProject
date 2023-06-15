using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutoManager : MonoBehaviour
{
    [SerializeField] Image _imageTuto;
    [SerializeField] GameObject _arrowLeft;
    [SerializeField] GameObject _arrowRight;
    [SerializeField] private List<Sprite> _sprites;
    private int _index = 0;

    private void Start()
    {
        _index = 0;
        CheckLeft();
        CheckRight();
        Debug.Log("index "+_index);
        if (_sprites.Count > 0)
        {
            _imageTuto.sprite = _sprites[_index];
        }
    }

    public void NextPage()
    {
        if (_index < _sprites.Count - 1)
        {
            _index++;
        }
        CheckLeft();
        CheckRight();
        _imageTuto.sprite = _sprites[_index];
    }

    public void PreviousPage()
    {
        if (_index > 0)
        {
            _index--;
        }
        CheckLeft();
        CheckRight();
        _imageTuto.sprite = _sprites[_index];
    }

    private void CheckRight()
    {
        _arrowRight.SetActive(_index != _sprites.Count - 1 && _sprites.Count != 0);//on affiche pas quand c'est le last
    }

    private void CheckLeft()
    {
        _arrowLeft.SetActive(_index != 0);
    }
}
