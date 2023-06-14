using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{

    [SerializeField] private Sprite _shurikenFull;
    [SerializeField] private Sprite _playButton;
    [SerializeField] private Sprite _retryButton;



    void FillStars(int stars){
        for (int i=0; i<stars; i++){
            this.transform.GetChild(i).gameObject.GetComponent<Image>().sprite= _shurikenFull;
        }
    }

    //ouvre la porte (à faire pour le plus haut niveau accessible)
    public void OpenDoorAndAddSmoke(){
        this.transform.GetChild(4).transform.GetChild(0).localPosition += new Vector3(-30, 0, 0); //porte gauche
        this.transform.GetChild(4).transform.GetChild(1).localPosition += new Vector3(30, 0, 0); //porte droite
        this.transform.GetChild(3).gameObject.SetActive(true); //fumée
        this.transform.GetChild(4).transform.GetChild(3).localPosition += new Vector3(30, 0, 0); // bouton
        this.transform.GetChild(4).transform.GetChild(3).GetComponent<Image>().sprite = _playButton; //bouton
    }

    public void CloseDoorAndRemoveSmoke(){
        this.transform.GetChild(4).transform.GetChild(0).localPosition -= new Vector3(-30, 0, 0);
        this.transform.GetChild(4).transform.GetChild(1).localPosition -= new Vector3(30, 0, 0);
        this.transform.GetChild(3).gameObject.SetActive(false);
        this.transform.GetChild(4).transform.GetChild(3).localPosition -= new Vector3(30, 0, 0);
        this.transform.GetChild(4).transform.GetChild(3).GetComponent<Image>().sprite = _retryButton;
    }

    //cache la porte rouge avec des chaine (à faire pour tous les niveaux disponibles)
    public void HideClosedDoor(){
        this.transform.GetChild(4).transform.GetChild(2).gameObject.GetComponent<Image>().enabled = false;
        this.transform.GetChild(4).GetComponent<Button>().enabled = true;
        this.transform.GetChild(4).transform.GetChild(3).gameObject.SetActive(true);
        this.transform.GetChild(4).transform.GetChild(3).GetComponent<Image>().sprite = _retryButton;
    }
}
