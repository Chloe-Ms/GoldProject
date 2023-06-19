using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using TMPro;

public class ElementList : MonoBehaviour
{
    private static ElementList _instance;
    [SerializeField] private RoomList list;
    [SerializeField] private RectTransform _startPosition;
    [SerializeField] private GameObject _background;
    [SerializeField] private GameObject _text;
    [SerializeField] private Sprite _textBackground;
    [SerializeField] private GameObject _parent;
    [SerializeField, Range(0.1f, 2f)] private float _margin = 2f;
    [SerializeField] private Vector3 _scale = new Vector3(0.2f, 0.2f, 1f);
    [ShowNonSerializedField] private List<GameObject> _elements = new List<GameObject>();
    [ShowNonSerializedField] private List<UIElement> _uiElements = new List<UIElement>();
    [SerializeField] private UIMenu _uiMenu;

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

    private void ClearElements()
    {
        //Debug.Log(_elements.Count);
        if (_elements.Count > 0)
        {
            foreach (var element in _elements)
            {
                DestroyImmediate(element);
            }
            _elements.Clear();
            //Debug.Log("ClearElements");
        }
    }

    private void ClearUIElements()
    {
        if (_uiElements.Count > 0)
        {
            foreach (var element in _uiElements)
            {
                DestroyImmediate(element.Element);
            }
            _uiElements.Clear();
        }
    }

    private void GenerateElement(TrapList listOfTrap, int[] nbOfTrap)
    {
        GameObject instanciateElement = null;
        GameObject instanciateBackground = null;
        GameObject instanciateTop = null;
        GameObject instanciateTextBackground = null;
        GameObject instanciateText = null;
        Vector2 position = Vector2.zero;
        int index = 0;

        ClearElements();
        foreach (TrapData trap in listOfTrap.TrapData)
        {
            if (trap.EnglishName == "Entrance" || trap.EnglishName == "Boss Room")
                continue;
            if (index < nbOfTrap.Length && nbOfTrap[index] == 0) {
                index++;
                continue;
            }
            instanciateBackground = Instantiate(_background);
            instanciateBackground.name = "Room_" + _uiElements.Count;
            instanciateBackground.GetComponent<RectTransform>().localPosition = new Vector2(_startPosition.transform.position.x + _margin * _uiElements.Count, _startPosition.transform.position.y);
            position = instanciateBackground.GetComponent<RectTransform>().localPosition;
            instanciateBackground.AddComponent<Button>();
            instanciateBackground.AddComponent<InputRoom>();
            instanciateBackground.transform.SetParent(_parent.transform);
            instanciateElement = new GameObject();
            instanciateElement.name = "Element_" + _elements.Count;
            instanciateElement.AddComponent<RectTransform>();
            //instanciateElement.GetComponent<RectTransform>().localPosition = new Vector2(_startPosition.transform.position.x + _margin * _elements.Count, _startPosition.transform.position.y);
            instanciateElement.GetComponent<RectTransform>().localPosition = new Vector2(position.x, position.y);
            instanciateElement.AddComponent<CanvasRenderer>();
            instanciateElement.AddComponent<Image>();
            instanciateElement.GetComponent<Image>().sprite = trap.Sprite;
            instanciateElement.transform.SetParent(instanciateBackground.transform);
            instanciateTop = new GameObject();
            instanciateTop.name = "Top";
            instanciateTop.AddComponent<RectTransform>();
            instanciateTop.transform.SetParent(instanciateBackground.transform);
            instanciateTop.GetComponent<RectTransform>().localPosition = new Vector2(0, 50);
            instanciateTextBackground = new GameObject();
            instanciateTextBackground.AddComponent<RectTransform>();
            instanciateTextBackground.GetComponent<RectTransform>().localPosition = new Vector2(position.x + 54f, position.y + 22f);
            instanciateTextBackground.GetComponent<RectTransform>().localScale = new Vector2(0.65f, 0.65f);
            instanciateTextBackground.AddComponent<Image>();
            instanciateTextBackground.GetComponent<Image>().sprite = _textBackground;
            instanciateTextBackground.transform.SetParent(instanciateElement.transform);
            instanciateText = Instantiate(_text);
            instanciateText.GetComponent<RectTransform>().localPosition = new Vector2(position.x + 70f, position.y + 60f);
            instanciateText.transform.SetParent(instanciateElement.transform);
            _uiElements.Add(new UIElement(instanciateBackground, instanciateText, trap, nbOfTrap[index]));
            _elements.Add(instanciateBackground);
            instanciateBackground.GetComponent<InputRoom>().Init(this, trap, _uiMenu, _uiElements[_uiElements.Count - 1]);
            instanciateBackground.GetComponent<RectTransform>().localScale = _scale;
            index++;
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

    // public void RemoveBossRoom()
    // {
    //     GameObject haveToRemove = _elements[_elements.Count - 1];

    //     _elements.RemoveAt(_elements.Count - 1);
    //     DestroyImmediate(haveToRemove);
    // }

    private void OnApplicationQuit()
    {
        ClearElements();
    }

    // public void InitList()
    // {
    //     GenerateElement(GameManager.Instance.GeneralData.TrapList);
    // }

    public void InitList(int[] nbOfTrap)
    {
        GenerateElement(GameManager.Instance.GeneralData.TrapList, nbOfTrap);
    }

    public void ChangeUIElementValue(TrapData trap, int value)
    {
        foreach (UIElement element in _uiElements)
        {
            if (element.TrapData == trap)
            {
                Debug.Log($"element nb = {element.Nb} value = {value}");
                element.Nb += value;
                if (element.Nb == 0)
                    element.Element.transform.GetChild(0).GetComponent<Image>().color = Color.gray;
                else
                    element.Element.transform.GetChild(0).GetComponent<Image>().color = Color.white;
                element.UpdateText();
                break;
            }
        }
    }
}

[System.Serializable]
public class UIElement
{
    private GameObject _element;
    private TrapData _trapData;
    private TMP_Text _text;
    private int _maxNb;
    private int _nb;

    public GameObject Element {
        get => _element;
    }

    public TrapData TrapData {
        get => _trapData;
    }

    public int MaxNb {
        get => _maxNb;
    }

    public int Nb {
        get => _nb;
        set => _nb = value;
    }

    public UIElement(GameObject element, GameObject text, TrapData trapData, int maxNb)
    {
        _element = element;
        _text = text.GetComponent<TMP_Text>();
        _trapData = trapData;
        _maxNb = maxNb;
        _nb = maxNb;
        UpdateText();
    }

    public void UpdateText()
    {
        _text.text = _nb.ToString();
    }
}