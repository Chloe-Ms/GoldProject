using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCloudMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float _speed;
    private Vector2 _initPosition;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _initPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.parent.gameObject.name == "CloudsBottom")
        {
            _rb.velocity = new Vector2(_speed, 0);
            if (this.transform.position.x >= 1280){
                this.transform.position = new Vector2(-200,_initPosition.y);
            }
        }
        else
        {
            _rb.velocity = new Vector2(-_speed, 0);
            if (this.transform.position.x <= -200){
                this.transform.position = new Vector2(1280, _initPosition.y);
            }
        }
    }
}
