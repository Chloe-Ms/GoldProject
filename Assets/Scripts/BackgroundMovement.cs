using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    Rigidbody2D _rb;
    [SerializeField] float _horizontalSpeed;
    [SerializeField] float _verticalSpeed;

    [SerializeField] Transform _sideNeighbour;
    [SerializeField] Transform _upNeighbour;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = new Vector2(_horizontalSpeed, -_verticalSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.x >= 10.75f)
        {
            this.transform.position = _sideNeighbour.position + new Vector3(-10.75f, 0, 0);
        }
        if (this.transform.position.y <= -19.15f)
        {
            this.transform.position = _upNeighbour.position + new Vector3(0, 19.15f, 0);
        }
    }
}
