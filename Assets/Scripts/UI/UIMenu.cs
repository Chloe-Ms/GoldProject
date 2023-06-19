using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    [SerializeField] UIHeroInfos _menuCharacter;
    [SerializeField] UIRoomInfos _menuRoom;
    [SerializeField] UITutoInfos _menuTuto; // peut etre nul
    HeroData[] _heroesData;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && _menuTuto.gameObject.activeSelf == true)
        {
            ChangeTutorialPage();
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void DisplayCharacter(int i)
    {
        _menuCharacter.gameObject.SetActive(true);
        _menuCharacter.ChangeData(i);
    }

    public void DisplayRoom(TrapData trapData)
    {
        _menuRoom.gameObject.SetActive(true);
        _menuRoom.Init(trapData);
    }

    public void DisplayTutorial() //peut etre nul
    {
        _menuTuto.gameObject.SetActive(true);
        _menuTuto.BeginReadingTutoData();
    }

    public void ChangeTutorialPage() // ça aussi
    {
        if (_menuTuto.CanChangeTutoData())
        {
            _menuTuto.ChangeTutoData();
        }
        else
        {
            _menuTuto.gameObject.SetActive(false);
            GameManager.Instance.ChangeNbMenuIn(-1);
        }
    }
}
