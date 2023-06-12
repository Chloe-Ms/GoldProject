using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOff : MonoBehaviour
{
    public GameObject ButtonOn;
    public GameObject ButtonOff;

    public void Clicked()
    {
        ButtonOn.SetActive(false);
        ButtonOff.SetActive(true);
    }

    public void ReClicked()
    {
        ButtonOn.SetActive(true);
        ButtonOff.SetActive(false);
    }
}
