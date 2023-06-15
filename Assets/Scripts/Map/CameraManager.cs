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
    [SerializeField] private float _speedZoom = 3f;
    [SerializeField] private float _speedMove = .5f;
    private float _timer = 0f;
    private bool _isZooming = false;
    private bool _isZoomed = false;
    private bool _isInPlayMode = false;
    private GameObject _groupParentGO;

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

    private void Start()
    {
        //_groupParentGO = 
        GameManager.Instance.OnEnterEditorMode += SetCameraEdit;
        GameManager.Instance.OnEnterPlayMode += SetCameraPlay;
    }

    private void SetCameraPlay(int obj)
    {
        _camera.orthographicSize = _zoomSizePlayMode;
    }

    private void SetCameraEdit(int obj)
    {
        _camera.orthographicSize = _initZoomSize;
        _camera.transform.position = _initPosition;
    }

    private void MoveCamera(Vector3 position)
    {
        
        position.z = _initPosition.z;
        transform.DOMove(position, _speedMove);
    }

    private void Update()
    {
        if (_isInPlayMode)
        {
            _camera.transform.position = GameManager.Instance.GetHeroesParentGameObject().transform.position;
        }
        else
        {
            if (MapManager.Instance.SelectedSlot != null)
            {
                MoveCamera(MapManager.Instance.SelectedSlot.transform.position);
                if (!_isZoomed && !_isZooming)
                {
                    _isZooming = true;
                    _timer = 0;
                }
            }
            else
            {
                MoveCamera(_initPosition);

                if (_isZoomed && !_isZooming)
                {
                    _isZooming = true;
                    _timer = 0;
                }
            }
        }
        
        if (_isZooming)
        {
            _timer += Time.deltaTime;
            if (!_isZoomed)
            {
                _camera.orthographicSize = Mathf.Lerp(_initZoomSize, _zoomSizeEditMode, (_timer * _speedZoom) / (_initZoomSize - _zoomSizeEditMode));
                if (_timer >= _speedZoom)
                {
                    _camera.orthographicSize = _zoomSizeEditMode;
                    _timer = 0;
                    _isZooming = false;
                    _isZoomed = true;
                }
            }
            else
            {
                _camera.orthographicSize = Mathf.Lerp(_zoomSizeEditMode, _initZoomSize, (_timer * _speedZoom) / (_initZoomSize - _zoomSizeEditMode));
                if (_timer >= _speedZoom)
                {
                    _camera.orthographicSize = _initZoomSize;
                    _timer = 0;
                    _isZooming = false;
                    _isZoomed = false;
                }
            }
        }
    }
}
