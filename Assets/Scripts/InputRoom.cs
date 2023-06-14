using UnityEngine;

public class InputRoom : MonoBehaviour
{
    private float _thresholdHold = 0.5f;
    private float _timeInput = 0f;
    private bool _hasInput = false;
    private bool _isCalledOnce = false;
    private RectTransform _rect;
    private ElementList _elementList;
    private TrapData _trapdata;
    private UIMenu _uiMenu;

    private void Start()
    {
        if (transform.childCount > 1)
        {
            _rect = transform.GetChild(1)?.GetComponent<RectTransform>();
        }
    }

    public void Init(ElementList elementList,TrapData trap,UIMenu uiMenu)
    {
        _elementList = elementList;
        _trapdata = trap;
        _uiMenu = uiMenu;
    }
    void Update()
    {
        if (_hasInput)
        {
            _timeInput += Time.deltaTime;
            if (_timeInput >= _thresholdHold && !_isCalledOnce)
            {
                _isCalledOnce = true;
                _timeInput = 0f;
                StartHold();
            }
        }

        if (Input.GetMouseButtonDown(0) && HasClickedOnRoom(Input.mousePosition))
        {
            OnRoomInput();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_timeInput < _thresholdHold && !_isCalledOnce && _hasInput)
            {
                _isCalledOnce = true;
                Tap();
            }

            _timeInput = 0f;
            _hasInput = false;
        }
    }

    public void OnRoomInput()
    {
        _hasInput = true;
        _isCalledOnce = false;
    }

    private void Tap()
    {
        if (_elementList != null)
        {
            _elementList.SetDataOnSelectedTrap(_trapdata);
        }
    }
    private void StartHold()
    {
        GameManager.Instance.NbMenuIn++;
        if (_uiMenu != null)
        {
            _uiMenu.DisplayRoom(_trapdata);
        }
    }

    private bool HasClickedOnRoom(Vector2 mousePos)
    {
        float radius = 0f;
        if (_rect != null)
        {
            radius = Vector2.Distance(_rect.transform.position, transform.position);
        }
        return Vector2.Distance(transform.position,mousePos) <= radius;
    }
}
