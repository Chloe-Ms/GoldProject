using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    [SerializeField] UIHeroInfos _menuCharacter;
    HeroData[] _heroesData;

    private void Start()
    {
        
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
}
