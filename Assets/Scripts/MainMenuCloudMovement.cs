using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCloudMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float _speed;
    private Vector2 _initPosition;
    private RectTransform _rectTransform;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _initPosition = this.transform.position;
        _rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.parent.gameObject.name == "CloudsBottom")
        {
            _rb.velocity = new Vector2(_speed, 0);
            if (_rectTransform.anchoredPosition.x >= 240){
                _rectTransform.anchoredPosition = new Vector2(-240, 0);
            }
        }
        else
        {
            _rb.velocity = new Vector2(-_speed, 0);
            if (_rectTransform.anchoredPosition.x >= 240){
                _rectTransform.anchoredPosition = new Vector2(-240, 0);
            }
        }
    }
}