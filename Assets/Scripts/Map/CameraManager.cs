using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;

    public static CameraManager Instance
    {
        get { return _instance; }
    }

    [SerializeField] private float _zoomSizeEditMode = 3f;
    [SerializeField] private float _zoomSizePlayMode = 3f;
    private float _initZoomSize;
    private Vector3 _initPosition;
    private Camera _camera;
    [SerializeField] private float _timeToZoom = .7f;
    [SerializeField] private float _timeToMove = .5f;
    private float _timer = 0f;
    private bool _isZooming = false;
    private bool _isZoomed = false;

    public Camera Camera
    {
        get { return _camera; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;
        this._camera = GetComponent<Camera>();
        this._initZoomSize = _camera.orthographicSize;
        this._initPosition = transform.position;
    }

    private void ResetCamera()
    {
        ResetCameraZoom();
        ResetCameraPosition();
    }

    private void ResetCameraZoom()
    {
        _camera.orthographicSize = _initZoomSize;
    }

    private void ResetCameraPosition()
    {
        transform.position = _initPosition;
    }

    private void MoveCamera(Vector3 position)
    {
        
        position.z = _initPosition.z;
        transform.DOMove(position, _timeToZoom);
    }

    private void ZoomCamera(float zoom)
    {
        _camera.orthographicSize = zoom;
    }

    private void Update()
    {
        /*if (_isZooming)
        {
            _timer += Time.time;
        }*/
        if (MapManager.Instance.SelectedSlot != null)
        {
            MoveCamera(MapManager.Instance.SelectedSlot.transform.position);
            ZoomCamera(_zoomSizeEditMode);
        }
        else
        {
            ResetCamera();
        }
    }
}
