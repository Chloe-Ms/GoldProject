using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private float _zoomSize = 2.5f;
    private Camera _camera;
    private float _initZoomSize;
    private Vector3 _initPosition;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _initZoomSize = _camera.orthographicSize;
        _initPosition = transform.position;
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
