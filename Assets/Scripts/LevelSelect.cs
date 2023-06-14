using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{

    [SerializeField] private Sprite _shurikenFull;

    void Start()
    {
        OpenDoorAndAddSmoke();
        HideClosedDoor();
        FillStars(3);
    }

    void FillStars(int stars){
        for (int i=0; i<stars; i++){
            this.transform.GetChild(i).gameObject.GetComponent<Image>().sprite= _shurikenFull;
        }
    }

    //ouvre la porte (à faire pour le plus haut niveau accessible)
    void OpenDoorAndAddSmoke(){
        this.transform.GetChild(4).transform.GetChild(0).localPosition += new Vector3(-30, 0, 0);
        this.transform.GetChild(4).transform.GetChild(1).localPosition += new Vector3(30, 0, 0);
        this.transform.GetChild(3).gameObject.SetActive(true);
    }

    //cache la porte rouge avec des chaine (à faire pour tous les niveaux disponibles)
    void HideClosedDoor(){
        this.transform.GetChild(4).transform.GetChild(2).gameObject.GetComponent<Image>().enabled = false;
    }
}
