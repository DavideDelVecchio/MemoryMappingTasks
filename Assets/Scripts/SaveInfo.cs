using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SaveInfo : MonoBehaviour {
    public GameObject sbj,save;
    public Toggle oculus;
    public InputField sbjname;
    public Dropdown env_order;
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

        PlayerPrefs.SetInt("EnvOrder", env_order.value);
        UsefulFunctions.env1 = 0;
        UsefulFunctions.env2 = 0;
        UsefulFunctions.env3 = 0;
        UsefulFunctions.RndEnvironment();
    }


}
