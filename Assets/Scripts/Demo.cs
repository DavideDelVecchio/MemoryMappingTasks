using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Demo : MonoBehaviour {

    public GameObject player, oculus_player, treasure;
    int menu_score, score_ui;
    int score = 0;
    string _FileLocation, _data, subName;
    public Text score_gained, gems_info;
    public static GameObject collectObj;
    public static Vector3 goal_position, last_position;
    public static string _FileName, score_text = "";

    //Called before any other function
    void Awake()
    {
        //Increase trial counter
        UsefulFunctions.current_trial++;
        //If status is ok activates the right player GameObject, checking if Oculus toggle was on or off
        switch (PlayerPrefs.GetInt("Oculus"))
        {
            //Default case
            case 0:
                player.SetActive(true);
                oculus_player.SetActive(false);
                Debug.Log("Activate player prefab");
                break;
            //Oculus case
            case 1:
                player.SetActive(false);
                oculus_player.SetActive(true);
                Debug.Log("Activate Oculus OVR Player");
                break;
        }
        //Randomize Player and Treasure position
        UsefulFunctions.RndPositionObj(player); //Randomize player and treasure chest position
        UsefulFunctions.MainInfoSaving(player); //Saves player and treasure position
        //Randomize gem position and assign it to the variable
        collectObj = UsefulFunctions.ChooseGem();
        //Randomize number of collectable obj
        menu_score = Random.Range(1, 3);
        //Update score and gems UI
        gems_info.text = "GEMS: " + score.ToString() + "/" + menu_score.ToString();
        score_gained.text = "0";
    }

    // Use this for initialization
    void Start ()
    {
        //Define file location
        _FileLocation = Application.dataPath + "/SubTrialInfo";
        //Saves the treasure position
        goal_position = treasure.transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {

    }
}
