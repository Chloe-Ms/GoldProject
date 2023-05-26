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
    [SerializeField, Range(150f, 250f)] private float _margin = 150f;
    private List<GameObject> _elements = new List<GameObject>();

    [Button("Clear Elements")]
    private void ClearElements()
    {
        if (_elements.Count > 0)
        {
            foreach (var element in _elements)
            {
                DestroyImmediate(element);
            }
            _elements.Clear();
        }
    }

    [Button("Generate List Of Rooms")]
    private void GenerateRoom()
    {
        GameObject instanciateElement = null;
        GameObject instanciateBackground = null;

        if (_elements.Count > 0)
        {
            foreach (var element in _elements)
            {
                DestroyImmediate(element);
            }
            _elements.Clear();
        }
        foreach (var room in list.RoomData)
        {
            instanciateBackground = Instantiate(_background);
            instanciateBackground.name = "Room_" + _elements.Count;
            instanciateBackground.GetComponent<RectTransform>().anchoredPosition = _startPosition.anchoredPosition;
            instanciateBackground.GetComponent<RectTransform>().localPosition = new Vector2(_startPosition.transform.position.x, _startPosition.transform.position.y  - _margin * _elements.Count);
            instanciateBackground.transform.SetParent(transform);
            instanciateElement = new GameObject();
            instanciateElement.name = "Element_" + _elements.Count;
            instanciateElement.AddComponent<RectTransform>();
            instanciateElement.GetComponent<RectTransform>().localPosition = new Vector2(_startPosition.transform.position.x, _startPosition.transform.position.y  - _margin * _elements.Count);
            instanciateElement.AddComponent<CanvasRenderer>();
            instanciateElement.AddComponent<Image>();
            instanciateElement.GetComponent<Image>().sprite = room.Sprite;
            instanciateElement.transform.SetParent(instanciateBackground.transform);
            _elements.Add(instanciateBackground);
        }
    }
}
