using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SaveInfo : MonoBehaviour {
    public GameObject sbj, objs,save;
    public Dropdown env;
    public Toggle typePath, typeMap, oculus;
    public InputField sbjname, objsnum;
    public int level = 0;

    //Levels:
    //1: Distal cues only
    //2: Distal cues & Boundaries
    //3: Distal cues & Local Landmarks
    //4: Distl cues + Local Landmarks + Boudaries
    //5: Empty



    void Update()
    {
        if(sbjname.text != null && (typePath.isOn == true || typeMap.isOn == true))
        {
            save.GetComponent<Button>().interactable = true;
        }

    }


    public void OnClick() {
        PlayerPrefs.SetString("SubjID", sbjname.text);
        PlayerPrefs.SetInt("ObjNum", int.Parse(objsnum.text));
        switch (env.value)
        {
            //Level 1: Distal cues only
            case 0:
                //Application.LoadLevel(2);
                break;
            //Level 2: Distal cues & Boundaries
            case 1:
                break;
            //Level 3: Distal cues & Local Landmarks
            case 2:
                break;
            //Level 4: Distal cues + Local Landmarks + Boundaries
            case 3:
                break;
            //Level 5: Empty
            case 4:
                break;
        }         
    }

    public void onCancelClick()
    {
        Application.Quit();
    }


}
