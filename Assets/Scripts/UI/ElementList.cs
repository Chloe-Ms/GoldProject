using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class ElementList : MonoBehaviour
{
    private static ElementList _instance;
    [SerializeField] private RoomList list;
    [SerializeField] private RectTransform _startPosition;
    [SerializeField] private GameObject _background;
    [SerializeField] private GameObject _parent;
    [SerializeField, Range(0.1f, 2f)] private float _margin = 2f;
    [SerializeField] private Vector3 _scale = new Vector3(0.2f, 0.2f, 1f);
    [ShowNonSerializedField] private List<GameObject> _elements = new List<GameObject>();

    /*private void Start()
    {
        GameManager.Instance.OnEnterEditorMode += InitOnEditMode;
    }

    private void InitOnEditMode(int obj)
    {
        InitList();
    }*/

    public static ElementList Instance
    {
        get { return _instance; }
    }

    [Button("Clear Elements")]
    private void ClearElements()
    {
        Debug.Log(_elements.Count);
        if (_elements.Count > 0)
        {
            foreach (var element in _elements)
            {
                DestroyImmediate(element);
            }
            _elements.Clear();
            Debug.Log("ClearElements");
        }
    }

    [Button("Generate List Of Rooms")]
    private void GenerateRoom()
    {
        GameObject instanciateElement = null;
        GameObject instanciateBackground = null;

        ClearElements();
        foreach (RoomData room in list.RoomData)
        {
            instanciateBackground = Instantiate(_background);
            instanciateBackground.name = "Room_" + _elements.Count;
            instanciateBackground.GetComponent<RectTransform>().localPosition = new Vector2(_startPosition.transform.position.x + _margin * _elements.Count, _startPosition.transform.position.y);
            instanciateBackground.AddComponent<Button>();
            instanciateBackground.AddComponent<InputRoom>();
            instanciateBackground.transform.SetParent(_parent.transform);
            instanciateElement = new GameObject();
            instanciateElement.name = "Element_" + _elements.Count;
            instanciateElement.AddComponent<RectTransform>();
            instanciateElement.GetComponent<RectTransform>().localPosition = new Vector2(_startPosition.transform.position.x + _margin * _elements.Count, _startPosition.transform.position.y);
            instanciateElement.AddComponent<CanvasRenderer>();
            instanciateElement.AddComponent<Image>();
            instanciateElement.GetComponent<Image>().sprite = room.Sprite;
            instanciateElement.transform.SetParent(instanciateBackground.transform);
            _elements.Add(instanciateBackground);
            instanciateBackground.GetComponent<RectTransform>().localScale = _scale;
            instanciateBackground.GetComponent<Button>().onClick.AddListener(() => { SetDataOnSelectedRoom(room); });
        }
        GetComponent<ScrollRect>().content = _parent.GetComponent<RectTransform>();
    }

    private void GenerateElement(RoomList listofRoom)
    {
        GameObject instanciateElement = null;
        GameObject instanciateBackground = null;

        ClearElements();
        foreach (RoomData room in listofRoom.RoomData)
        {
            instanciateBackground = Instantiate(_background);
            instanciateBackground.name = "Room_" + _elements.Count;
            instanciateBackground.GetComponent<RectTransform>().localPosition = new Vector2(_startPosition.transform.position.x + _margin * _elements.Count, _startPosition.transform.position.y);
            instanciateBackground.AddComponent<Button>();
            instanciateBackground.AddComponent<InputRoom>();
            instanciateBackground.transform.SetParent(_parent.transform);
            instanciateElement = new GameObject();
            instanciateElement.name = "Element_" + _elements.Count;
            instanciateElement.AddComponent<RectTransform>();
            instanciateElement.GetComponent<RectTransform>().localPosition = new Vector2(_startPosition.transform.position.x + _margin * _elements.Count, _startPosition.transform.position.y);
            instanciateElement.AddComponent<CanvasRenderer>();
            instanciateElement.AddComponent<Image>();
            instanciateElement.GetComponent<Image>().sprite = room.Sprite;
            instanciateElement.transform.SetParent(instanciateBackground.transform);
            _elements.Add(instanciateBackground);
            instanciateBackground.GetComponent<RectTransform>().localScale = _scale;
            instanciateBackground.GetComponent<Button>().onClick.AddListener(() => { SetDataOnSelectedRoom(room); });
        }
        GetComponent<ScrollRect>().content = _parent.GetComponent<RectTransform>();
    }

    private void GenerateElement(TrapList listOfTrap)
    {
        GameObject instanciateElement = null;
        GameObject instanciateBackground = null;

        ClearElements();
        foreach (TrapData trap in listOfTrap.TrapData)
        {
            if (trap.Name == "Entrance" || trap.Name == "Boss Room")
                continue;
            instanciateBackground = Instantiate(_background);
            instanciateBackground.name = "Room_" + _elements.Count;
            instanciateBackground.GetComponent<RectTransform>().localPosition = new Vector2(_startPosition.transform.position.x + _margin * _elements.Count, _startPosition.transform.position.y);
            instanciateBackground.AddComponent<Button>();
            instanciateBackground.AddComponent<InputRoom>();
            instanciateBackground.transform.SetParent(_parent.transform);
            instanciateElement = new GameObject();
            instanciateElement.name = "Element_" + _elements.Count;
            instanciateElement.AddComponent<RectTransform>();
            instanciateElement.GetComponent<RectTransform>().localPosition = new Vector2(_startPosition.transform.position.x + _margin * _elements.Count, _startPosition.transform.position.y);
            instanciateElement.AddComponent<CanvasRenderer>();
            instanciateElement.AddComponent<Image>();
            instanciateElement.GetComponent<Image>().sprite = trap.Sprite;
            instanciateElement.transform.SetParent(instanciateBackground.transform);
            _elements.Add(instanciateBackground);
            instanciateBackground.GetComponent<RectTransform>().localScale = _scale;
            instanciateBackground.GetComponent<Button>().onClick.AddListener(() => { SetDataOnSelectedTrap(trap); });
        }
        GetComponent<ScrollRect>().content = _parent.GetComponent<RectTransform>();
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;
    }

    private void PrintAllDirection()
    {
        foreach (RoomData room in list.RoomData)
        {
            Debug.Log($"{room.name} is in {(int)room.Directions}");
        }
    }

    public void SetDataOnSelectedRoom(RoomData data)
    {
        if (MapManager.Instance.SelectedSlot != null)
            EditorManager.Instance.SetDataOnSelectedRoom(data);
    }

    public void SetDataOnSelectedTrap(TrapData data)
    {
        if (MapManager.Instance.SelectedSlot != null)
            EditorManager.Instance.SetDataOnSelectedTrap(data);
    }

    public void RemoveBossRoom()
    {
        GameObject haveToRemove = _elements[_elements.Count - 1];

        _elements.RemoveAt(_elements.Count - 1);
        DestroyImmediate(haveToRemove);
    }

    private void OnApplicationQuit()
    {
        ClearElements();
    }

    public void InitList()
    {
        GenerateElement(GameManager.Instance.GeneralData.TrapList);
    }
}
