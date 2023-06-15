using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField] GameObject _mainMenu;
    [SerializeField] GameObject _optionMenu;

    public void Play()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Option()
    {
        _optionMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void OptionMenu()
    {
        _optionMenu.SetActive(false);
        Debug.Log("OptionMenu");
    }

}
