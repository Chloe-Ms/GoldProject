using UnityEngine;

public class EditorManager : MonoBehaviour
{
    private static EditorManager _instance;

    public static EditorManager Instance
    {
        get { return _instance; }
    }

    [SerializeField] private GameObject _editorMenu;
    [SerializeField] private Vector3 _menuPosition;
    [SerializeField] private Vector3 _vectorOffset = new Vector3(0f, 175f, 0f);

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;
    }

    private void Start()
    {
        _menuPosition = _editorMenu.transform.position;
        CloseEditorMenu();
    }

    public void OpenEditorMenu()
    {
        MapManager.Instance.EditorState = EditorState.Edit;
        _editorMenu.transform.position = _menuPosition;
    }

    public void CloseEditorMenu()
    {
        MapManager.Instance.EditorState = EditorState.Select;
        if (_editorMenu.transform.position.magnitude == _menuPosition.magnitude) {
            _editorMenu.transform.position = new Vector3(_menuPosition.x, _menuPosition.y - _menuPosition.y, _menuPosition.z);
        }
    }

    public void SetDataOnSelectedRoom(RoomData data)
    {
        if (MapManager.Instance.SelectedSlot != null)
            MapManager.Instance.SetDataOnSelectedRoom(data);
    }

    public void SetDataOnSelectedTrap(TrapData data)
    {
        if (MapManager.Instance.SelectedSlot != null)
        {
            MapManager.Instance.SetDataOnSelectedTrap(data);
        }
            
    }
}
