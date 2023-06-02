using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;

    public static CameraManager Instance
    {
        get { return _instance; }
    }

    private float _zoomSize = 2.5f;
    private float _initZoomSize;
    private Vector3 _initPosition;
    private Camera _camera;

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
        transform.position = position;
        _camera.orthographicSize = _zoomSize;
    }

    private void Update()
    {
        if (MapManager.Instance.SelectedSlot != null)
            MoveCamera(MapManager.Instance.SelectedSlot.transform.position);
        else
            ResetCamera();
    }
}
