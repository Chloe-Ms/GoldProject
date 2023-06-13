using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputRoom : MonoBehaviour
{
    private float _thresholdHold = 0.5f;
    private float _timeInput = 0f;
    private bool _hasInput = false;
    private bool _isCalledOnce = false;
    void Update()
    {
        if (_hasInput)
        {
            _timeInput += Time.deltaTime;
            if (_timeInput >= _thresholdHold && !_isCalledOnce)
            {
                _isCalledOnce = true;
                _timeInput = 0f;
                StartHold();
            }
        }
        /*if (Input.GetMouseButtonDown(0))
        {
            OnRoomInput();
        }*/
        if (Input.GetMouseButtonUp(0))
        {
            if (_timeInput < _thresholdHold && !_isCalledOnce && _hasInput)
            {
                _isCalledOnce = true;
                Tap();
            }

            _timeInput = 0f;
            _hasInput = false;
        }
    }

    public void OnRoomInput()
    {
        _hasInput = true;
        _isCalledOnce = false;
        Debug.Log("COUCU");
    }

    private void Tap()
    {
        Debug.Log("TAP");
    }
    private void StartHold()
    {
        Debug.Log("HOLD");
    }
}
