using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] InputActionReference _touchPut;
    [SerializeField] InputActionReference _primaryTouchPosition;
    [SerializeField] InputActionReference _secondaryTouchPosition;
    [SerializeField] InputActionReference _secondaryTouchContact;
    [SerializeField] InputActionReference _primaryTouchContact;
    [SerializeField] InputActionReference _touchPress;
    [SerializeField] InputActionReference _primaryDelta;
    [SerializeField] InputActionReference _secondaryDelta;
    [SerializeField] float _cameraSpeed = 4f;
    [SerializeField] CameraManager _cameraManager;
    Camera _camera;

    Coroutine _holdRoutine;
    Coroutine _zoomRoutine;

    bool _hasHoldInputStarted = false;

    public event Action OnTapEvent;
    public event Action OnHoldStartedEvent;
    public event Action OnHoldEndedEvent;

    private void Start()
    {
        _camera = Camera.main;
    }
    private void OnEnable()
    {  
        _touchPut.action.performed += OnTouchTap;
        _touchPress.action.performed += OnHoldStarted;
        _touchPress.action.canceled += OnHoldEnded;
/*        _secondaryTouchContact.action.started += ZoomStart;
        _secondaryTouchContact.action.canceled += ZoomEnd;
        _primaryTouchContact.action.canceled += ZoomEnd;*/
    }

/*    private void ZoomStart(InputAction.CallbackContext obj)
    {
        Debug.Log("Zoom");
        _zoomRoutine = StartCoroutine(ZoomDetection());
    }

    private void ZoomEnd(InputAction.CallbackContext obj)
    {
        if (_zoomRoutine != null)
        {
            Debug.Log("ZoomEnd");
            //_cameraManager.InitZoomSize = Camera.main.orthographicSize;
            StopCoroutine(_zoomRoutine);
            _zoomRoutine = null;
        }
    }*/

    IEnumerator ZoomDetection()
    {
        float previousDistance = Vector2.Distance(_primaryTouchPosition.action.ReadValue<Vector2>(),
                _secondaryTouchPosition.action.ReadValue<Vector2>());
        float distance = 0f;
        while(true)
        {
            /*Vector2 primaryPrevPos = _primaryTouchPosition.action.ReadValue<Vector2>() - _primaryDelta.action.ReadValue<Vector2>();
            Vector2 secondaryPrevPos = _secondaryTouchPosition.action.ReadValue<Vector2>() - _secondaryDelta.action.ReadValue<Vector2>();
            float prevMagnitude = (primaryPrevPos - secondaryPrevPos).magnitude;
            float currentMagnitude = (_primaryTouchPosition.action.ReadValue<Vector2>() - _secondaryTouchPosition.action.ReadValue<Vector2>()).magnitude;

            float difference = currentMagnitude - prevMagnitude;*/

            distance = Vector2.Distance(_primaryTouchPosition.action.ReadValue<Vector2>(),
                _secondaryTouchPosition.action.ReadValue<Vector2>());
            float size = 0f;
            //Zoom out
            if (distance - previousDistance  > .2f)
            {
                size = _camera.orthographicSize - distance * _cameraSpeed;
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + size * 0.05f, 2f, 10f);
            }
            //Zoom in
            else if (distance - previousDistance < .2f)
            {
                size = _camera.orthographicSize + distance * _cameraSpeed;
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + size * 0.05f, 2f, 10f);
            }
            Debug.Log(Camera.main.orthographicSize);
            //Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - (difference *0.01f), 2f, 10f);
            //previousDistance = distance;
            yield return null;
        }
    }
    private void OnDisable()
    {
        _touchPut.action.performed -= OnTouchTap;
    }

    private void OnTouchTap(InputAction.CallbackContext obj)
    {
        Vector2 value = _primaryTouchPosition.action.ReadValue<Vector2>();
        Vector3 position = Camera.main.ScreenToWorldPoint(value);
        position.z = 0;

        OnTapEvent?.Invoke();
    }
    private void OnHoldStarted(InputAction.CallbackContext obj)
    {
        Vector2 value = _primaryTouchPosition.action.ReadValue<Vector2>();
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
            Vector2 value = _primaryTouchPosition.action.ReadValue<Vector2>();
            Vector3 position = Camera.main.ScreenToWorldPoint(value);
            position.z = 0;
            yield return new WaitForFixedUpdate();
        }
    }
}
