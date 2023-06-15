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
    private float _startZoomSize;
    private Vector3 _initPosition;
    private Camera _camera;
    [SerializeField] private float _speedZoom = 3f;
    [SerializeField] private float _speedMove = .5f;
    private float _timer = 0f;
    private bool _isZooming = false;
    private bool _isZoomed = false;
    private bool _isDezoomed = true;
    private bool _isInPlayMode = false;
    private bool _isDezooming = false;
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
        this._startZoomSize = this._initZoomSize;
    }

    private void Start()
    {
        _groupParentGO = GameManager.Instance.GetHeroesParentGameObject();
        GameManager.Instance.OnEnterEditorMode += SetCameraEdit;
        GameManager.Instance.OnEnterPlayMode += SetCameraPlay;
    }

    private void SetCameraPlay(int obj)
    {
        _isInPlayMode = true;
        _camera.orthographicSize = _zoomSizePlayMode;
        FollowGroupHeroes();
    }

    private void SetCameraEdit(int obj)
    {
        _isInPlayMode = false;
        _camera.orthographicSize = _initZoomSize;
        _camera.transform.position = _initPosition;
    }

    private void MoveCamera(Vector3 position)
    {
        position.z = _initPosition.z;
        transform.DOMove(position, _speedMove);
    }

    private void FollowGroupHeroes()
    {
        Vector3 position = _groupParentGO.transform.position;
        position.z = _initPosition.z;
        _camera.transform.position = position;
    }

    private void Update()
    {
        if (_isInPlayMode)
        {
            FollowGroupHeroes();
        }
        else
        {
            if (MapManager.Instance.SelectedSlot != null) //Selection d'une salle
            {
                MoveCamera(MapManager.Instance.SelectedSlot.transform.position);
                MoveCamera(_initPosition);
                if (!_isZoomed && !_isZooming)
                {
                    Debug.Log("ZOOM");
                    _isZooming = true;
                    _isDezooming = false;
                    _timer = 0;
                    _isDezoomed = false;
                    this._startZoomSize = _camera.orthographicSize;
                }
                //_camera.orthographicSize = _zoomSizeEditMode;
                /*_isZooming = true;
                _timer = 0;*/
            }
            else //Aucune salle sélectionnée
            {
                MoveCamera(_initPosition);
                if (!_isDezoomed && !_isDezooming)
                {
                    Debug.Log("DEZOOM");
                    _isDezooming = true;
                    _isZooming = false;
                    _timer = 0;
                    _isZoomed = false;
                    this._startZoomSize = _camera.orthographicSize;
                }
                //_camera.orthographicSize = _initZoomSize;
                /*if (_isZoomed && !_isZooming)
                {
                    Debug.Log("deZOOM");
                    _isZooming = true;
                    _isZoomed = false;
                    _timer = 0;
                }*/
            }
        }
        
        if (_isZooming || _isDezooming)
        {
            _timer += Time.deltaTime;
            if (_isZooming) //De l'init au normal
            {
                _camera.orthographicSize = Mathf.Lerp(this._startZoomSize, _zoomSizeEditMode, (_timer * _speedZoom) / Mathf.Abs(this._startZoomSize - _zoomSizeEditMode));
                if (_timer >= Mathf.Abs(this._startZoomSize - _zoomSizeEditMode)/ _speedZoom)
                {
                    _camera.orthographicSize = _zoomSizeEditMode;
                    _timer = 0;
                    _isZooming = false;
                    _isZoomed = true;
                }
            }
            if (_isDezooming)
            {
                _camera.orthographicSize = Mathf.Lerp(this._startZoomSize, _initZoomSize, (_timer * _speedZoom) / Mathf.Abs(_initZoomSize - this._startZoomSize));
                if (_timer >= Mathf.Abs(_initZoomSize - this._startZoomSize) / _speedZoom)
                {
                    _camera.orthographicSize = _initZoomSize;
                    _timer = 0;
                    _isDezooming = false;
                    _isDezoomed = true;
                }
            }
        }

        /*if (_isInPlayMode)
        {
            FollowGroupHeroes();
        }
        else
        {
            if (MapManager.Instance.SelectedSlot != null)
            {
                Debug.Log("SELECTED");
                MoveCamera(MapManager.Instance.SelectedSlot.transform.position);
                if (!_isZoomed && !_isZooming)
                {
                    Debug.Log("ZOOm");
                    _isZooming = true;
                    _timer = 0;
                }
            }
            else
            {
                MoveCamera(_initPosition);

                if (_isZoomed && !_isZooming)
                {
                    Debug.Log("deZOOM");
                    _isZooming = true;
                    _isZoomed = false;
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
        }*/
    }
}
