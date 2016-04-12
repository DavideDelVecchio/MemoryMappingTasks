using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SaveInfo : MonoBehaviour {
    public GameObject sbj,save;
    public Toggle mapping;
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
        if (mapping.isOn)
        {
            PlayerPrefs.SetInt("isMapping", 1); //Mapping task
        }
        else {
            PlayerPrefs.SetInt("isMapping", 0); //Path Integration
        }

        PlayerPrefs.SetString("Score", "0");
        ResetEnvVariables();
        Application.LoadLevel(4);
    }



    public void ResetEnvVariables()
    {
        UsefulFunctions.env1 = 0;
        UsefulFunctions.env2 = 0;
        UsefulFunctions.env3 = 0;
        UsefulFunctions.endBlock = false;
    }

}
