using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class ElementList : MonoBehaviour
{
    [SerializeField] private RoomList list;
    [SerializeField] private RectTransform _startPosition;
    [SerializeField] private GameObject _background;
    [SerializeField] private GameObject _parent;
    [SerializeField] private GameObject _scrollBar;
    [SerializeField, Range(0.1f, 2f)] private float _margin = 2f;
    [SerializeField] private Vector3 _scale = new Vector3(0.2f, 0.2f, 1f);
    [ShowNonSerializedField] private List<GameObject> _elements = new List<GameObject>();

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
        int i = 0;
        foreach (RoomData room in list.RoomData)
        {
            instanciateBackground = Instantiate(_background);
            instanciateBackground.name = "Room_" + _elements.Count;
            instanciateBackground.GetComponent<RectTransform>().localPosition = new Vector2(_startPosition.transform.position.x + _margin * _elements.Count, _startPosition.transform.position.y);
            instanciateBackground.AddComponent<Button>();
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
            i++;
        }
        //GetComponent<ScrollRect>().content = _parent.GetComponent<RectTransform>();
        GetComponent<ScrollRect>().horizontalScrollbar = _scrollBar.GetComponent<Scrollbar>();
    }

    private void Awake()
    {
        GenerateRoom();
    }

    public void SetDataOnSelectedRoom(RoomData data)
    {
        if (MapManager.Instance.SelectedSlot != null)
            EditorManager.Instance.SetDataOnSelectedRoom(data);
    }

    private void OnApplicationQuit()
    {
        ClearElements();
    }

    // private void OnDisable()
    // {
    //     ClearElements();
    // }
}
