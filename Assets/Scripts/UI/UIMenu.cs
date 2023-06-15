using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    [SerializeField] UIHeroInfos _menuCharacter;
    [SerializeField] UIRoomInfos _menuRoom;
    HeroData[] _heroesData;

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
}
