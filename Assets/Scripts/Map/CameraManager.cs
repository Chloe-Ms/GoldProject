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
    [SerializeField] private float _durationMove = .5f;
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
        Debug.Log("CAMERA PLAY MODE");
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
        transform.DOMove(position, _durationMove);
    }

    private void FollowGroupHeroes()
    {
        Vector3 position = _groupParentGO.transform.position;
        position.z = _initPosition.z;
        _camera.transform.position = position;
    }

    public void Zoom()
    {
        //Debug.Log("Zoom");
        _isInPlayMode = true;
        _isZooming = true;
        this._startZoomSize = _camera.orthographicSize;
    }

    public void DezoomPlayMode()
    {
        //Debug.Log("Dezoom");
        _isInPlayMode = true;
        _isDezooming = true;
        this._startZoomSize = _camera.orthographicSize;
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
                if (!_isZoomed && !_isZooming)
                {
                    _isZooming = true;
                    _isDezooming = false;
                    _timer = 0;
                    _isDezoomed = false;
                    this._startZoomSize = _camera.orthographicSize;
                }
            }
            else //Aucune salle sélectionnée
            {
                MoveCamera(_initPosition);
                if (!_isDezoomed && !_isDezooming)
                {
                    _isDezooming = true;
                    _isZooming = false;
                    _timer = 0;
                    _isZoomed = false;
                    this._startZoomSize = _camera.orthographicSize;
                }
            }
        }
        
        if (_isZooming || _isDezooming)
        {
            //Debug.Log($"IS ZOOMING {_isZooming} OR DEZOOMING {_isDezooming} {_isDezoomed} {_isZoomed}");
            _timer += Time.deltaTime;
            if (_isZooming) //De l'init au normal
            {
                float endSize;
                if (_isInPlayMode)
                {
                    endSize = _zoomSizePlayMode;
                }
                else
                {
                    endSize = _zoomSizeEditMode;
                }
                _camera.orthographicSize = Mathf.Lerp(this._startZoomSize, endSize, 
                    (_timer * _speedZoom) / Mathf.Abs(this._startZoomSize - endSize));

                //Debug.Log($"{Mathf.Abs(this._startZoomSize - endSize) / _speedZoom}");
                if (_timer >= Mathf.Abs(this._startZoomSize - endSize) / _speedZoom)
                {
                    _camera.orthographicSize = endSize;
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
    }
}
