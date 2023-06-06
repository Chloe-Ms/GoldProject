using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField] GameObject _mainMenu;
    [SerializeField] GameObject _optionMenu;

    public void Play()
    {
        SceneManager.LoadScene("SampleScene");
        Debug.Log("Play");
    }

    public void Option()
    {
        _mainMenu.SetActive(false);
        _optionMenu.SetActive(true);
        Debug.Log("Option");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void OptionMenu()
    {
        _mainMenu.SetActive(true);
        _optionMenu.SetActive(false);
        Debug.Log("OptionMenu");
    }




}
