using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class SaveInfoShorter : MonoBehaviour {
    public GameObject sbj,save;
    public InputField sbjname;
    public int level = 0;
    public Dropdown designLevels;

   void Awake()
    {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(sbjname.gameObject,null);
        UsefulFunctions.isShorterVersion = true;
        PlayerPrefs.SetInt("isMapping", 1);
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
        PlayerPrefs.SetInt("isMapping", 1); //Mapping task

        PlayerPrefs.SetString("Score", "0");
        ResetEnvVariables();
        DesignSettings(designLevels.value);
        Application.LoadLevel("TestTrial");

    }



    public void ResetEnvVariables()
    {
        ShorterFunctions.levelCount = 1;
        ShorterFunctions.levelID = -1;
        UsefulFunctions.endBlock = false;
    }


    public void DesignSettings(int value)
    {
        //Six possible designs
        switch(value)
        {
            //1: Geometry
            case 0:
                ShorterFunctions.geometry = "ShorterVersionGeometry";
                ShorterFunctions.levelID = 1;
                break;
            //2: Geometry and Boundaries
            case 1:
                ShorterFunctions.distal_geometry = "ShorterVersionGeometry&DistalCues";
                ShorterFunctions.levelID = 2;
                break;
        }
    }

    public void OnCancelClick()
    {
        Application.Quit();
    }

}
