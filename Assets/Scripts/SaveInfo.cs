using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SaveInfo : MonoBehaviour {
    public GameObject sbj,save;
    public Toggle oculus;
    public InputField sbjname;
    public int level = 0;

    //Levels:
    //1: Distal cues only
    //2: Distal cues & Boundaries
    //3: Distal cues & Local Landmarks
    //4: Distl cues + Local Landmarks + Boudaries
    //5: Empty



    void Update()
    {
        if(sbjname.text != null)
        {
            save.GetComponent<Button>().interactable = true;
        }
    }


    public void OnClick() {
        PlayerPrefs.SetString("SubjID", sbjname.text);
        if (oculus.isOn)
        {
            PlayerPrefs.SetInt("Oculus", 1); //Exp with Oculus
        }
        else {
            PlayerPrefs.SetInt("Oculus", 0); //Exp without Oculus
        }
        //Choose randomly which enviroment to show as first block
        PlayerPrefs.SetInt("CurrentEnv", UsefulFunctions.RndEnvironment());
    }

    public void onCancelClick()
    {
        Application.Quit();
    }


}
