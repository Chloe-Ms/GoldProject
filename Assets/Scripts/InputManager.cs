using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] InputActionReference _touchPut;
    [SerializeField] InputActionReference _touchPosition;
    [SerializeField] InputActionReference _touchPress;
    
    Coroutine _holdRoutine;
    bool _hasHoldInputStarted = false;

    public event Action OnTapEvent;
    public event Action OnHoldStartedEvent;
    public event Action OnHoldEndedEvent;

    private void OnEnable()
    {  
        _touchPut.action.performed += OnTouchTap;
        _touchPress.action.performed += OnHoldStarted;
        _touchPress.action.canceled += OnHoldEnded;
    }

    private void OnDisable()
    {
        _touchPut.action.performed -= OnTouchTap;
    }

    private void OnTouchTap(InputAction.CallbackContext obj)
    {
        Vector2 value = _touchPosition.action.ReadValue<Vector2>();
        Vector3 position = Camera.main.ScreenToWorldPoint(value);
        position.z = 0;

        OnTapEvent?.Invoke();
    }
    private void OnHoldStarted(InputAction.CallbackContext obj)
    {
        Vector2 value = _touchPosition.action.ReadValue<Vector2>();
        Vector3 position = Camera.main.ScreenToWorldPoint(value);
        _holdRoutine = StartCoroutine(StartDrag());
        OnHoldStartedEvent?.Invoke();
    }

    private void OnHoldEnded(InputAction.CallbackContext obj)
    {
        if (_hasHoldInputStarted && _holdRoutine != null)
        {
            StopCoroutine(_holdRoutine);
            _holdRoutine = null;
            OnHoldEndedEvent?.Invoke();
        }
    }

    IEnumerator StartDrag()
    {
        while (true)
        {
            Vector2 value = _touchPosition.action.ReadValue<Vector2>();
            Vector3 position = Camera.main.ScreenToWorldPoint(value);
            position.z = 0;
            yield return new WaitForFixedUpdate();
        }
    }
}
