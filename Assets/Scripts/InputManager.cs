using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputActionReference _touchPress;
    [SerializeField] private InputActionReference _touchPosition;
    [SerializeField] GameObject _gameObject;
    public event Action OnStartTouchEvent;
    public event Action OnEndTouchEvent;

    void Awake()
    {
        _touchPress.action.performed += OnTouchStarted;
    }

    void OnDestroy()
    {
        _touchPress.action.performed -= OnTouchStarted;
    }
    private void OnTouchEnded(InputAction.CallbackContext obj)
    {
        Debug.Log("Touch ended " + _touchPosition);
        OnEndTouchEvent?.Invoke();
    }

    private void OnTouchStarted(InputAction.CallbackContext obj)
    {
    }

    private void Update()
    {
        if (_touchPress.action.WasPerformedThisFrame())
        {
            Vector2 value = _touchPosition.action.ReadValue<Vector2>();

            Vector3 position = Camera.main.ScreenToWorldPoint(value);
            Debug.LogWarning("position " + position + " value " + value);
            position.z = 0;
            _gameObject.transform.position = position;
        }
    }
}
