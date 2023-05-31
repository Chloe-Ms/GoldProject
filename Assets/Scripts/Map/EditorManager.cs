using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorManager : MonoBehaviour
{
    private static EditorManager _instance;

    public static EditorManager Instance
    {
        get { return _instance; }
    }

    [SerializeField] private GameObject _editorMenu;
    [SerializeField] private GameObject _editorOpener;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;
    }

    private void Start()
    {
        CloseEditorMenu();
    }

    public void OpenEditorMenu()
    {
        MapManager.Instance.EditorState = EditorState.Edit;
        _editorMenu.SetActive(true);
        _editorOpener.SetActive(false);
    }

    public void CloseEditorMenu()
    {
        MapManager.Instance.EditorState = EditorState.Select;
        _editorMenu.SetActive(false);
        _editorOpener.SetActive(true);
    }

    public void SetDataOnSelectedRoom(RoomData data)
    {
        if (MapManager.Instance.SelectedSlot != null)
            MapManager.Instance.SelectedSlot.SetData(data);
    }
}
