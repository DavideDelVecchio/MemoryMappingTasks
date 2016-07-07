using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class SaveInfo : MonoBehaviour {
    public GameObject sbj,save;
    public Toggle mapping;
    public InputField sbjname;
    public int level = 0;
    public Dropdown designLevels;

    //Levels:
    //1: Distal cues only
    //2: Distal cues & Boundaries
    //3: Distal cues & Local Landmarks

   void Awake()
    {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(sbjname.gameObject,null);
        
    }

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
        DesignSettings(designLevels.value);
    }



    public void ResetEnvVariables()
    {
        UsefulFunctions.env1 = 0;
        UsefulFunctions.env2 = 0;
        UsefulFunctions.env3 = 0;
        UsefulFunctions.endBlock = false;
    }


    public void DesignSettings(int value)
    {
        //Six possible designs
        switch(value)
        {
            //1: Empty - Boundary - Local cues
            case 0:
                UsefulFunctions.level1 = "DistalCuesEnv";
                UsefulFunctions.level2 = "Distal&BoundaryEnv";
                UsefulFunctions.level3 = "Distal&LocalCuesEnv";
                break;
            //2: Empty - Local cues - Boundary
            case 1:
                UsefulFunctions.level1 = "DistalCuesEnv";
                UsefulFunctions.level3 = "Distal&BoundaryEnv";
                UsefulFunctions.level2 = "Distal&LocalCuesEnv";
                break;
            //3: Boundary - Empty - Local cues
            case 2:
                UsefulFunctions.level2 = "DistalCuesEnv";
                UsefulFunctions.level1 = "Distal&BoundaryEnv";
                UsefulFunctions.level3 = "Distal&LocalCuesEnv";
                break;
            //4: Boundary - Local cues - Empty
            case 3:
                UsefulFunctions.level3 = "DistalCuesEnv";
                UsefulFunctions.level1 = "Distal&BoundaryEnv";
                UsefulFunctions.level2 = "Distal&LocalCuesEnv";
                break;
            //5: Local cues - Empty - Boundary
            case 4:
                UsefulFunctions.level2 = "DistalCuesEnv";
                UsefulFunctions.level3 = "Distal&BoundaryEnv";
                UsefulFunctions.level1 = "Distal&LocalCuesEnv";
                break;
            //6: Local cues - Boundary - Empty
            case 5:
                UsefulFunctions.level3 = "DistalCuesEnv";
                UsefulFunctions.level2 = "Distal&BoundaryEnv";
                UsefulFunctions.level1 = "Distal&LocalCuesEnv";
                break;
        }
    }

}
