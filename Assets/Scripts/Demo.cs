﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Demo : MonoBehaviour {

    public GameObject player, oculus_player, treasure, instruction_menu;
    int menu_score, score_ui;
    int score = 0;
    string _FileLocation, _data, subName;
    bool isSaving;
    public Text score_gained, gems_info;
    public static GameObject collectObj;
    public static Vector3 goal_position, last_position;
    public static float pause_t, trial_t, collision_t, distance = 0.0f;
    public static string _FileName, score_text = "";
    public static List<MainTrialInfo.InfoTrial> trialInfo = new List<MainTrialInfo.InfoTrial>();
    public static List<CompletePath.PathMapping> trialPath = new List<CompletePath.PathMapping>();
    public static List<DistanceAndFeedBack.FeedbackInfo> trialFeedback = new List<DistanceAndFeedBack.FeedbackInfo>();

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
        //Randomize gem position and assign it to the variable
        collectObj = UsefulFunctions.ChooseGem();
        //Randomize Player and Treasure position
        UsefulFunctions.RndPositionObj(player); //Randomize player and treasure chest position
        UsefulFunctions.MainInfoSaving(player); //Saves player and treasure position
        //Randomize number of collectable obj
        menu_score = Random.Range(1, 3);
        //Update score and gems UI
        gems_info.text = "GEMS: " + score.ToString() + "/" + menu_score.ToString();
        if(score_gained.text == string.Empty)
        {
            score_gained.text = "0";
        }
        else
        {
            score_gained = UsefulFunctions.tot_score;
        }
        treasure.GetComponent<Animator>().Stop();
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
        trial_t += Time.deltaTime; //updates time
        UsefulFunctions.PathTracing(player);
        if (score == menu_score)
        {
            if (UsefulFunctions.OnButtonPression() == true)
            {
                score_text = score_gained.text;
                UsefulFunctions.MainInfoSaving(player); //Saves last position
                last_position = player.transform.position;
                distance = Vector3.Distance(goal_position, last_position);
                //Saves all variables in files
                try
                {
                    isSaving = true;
                    _data = UsefulFunctions.SerializeObject(trialInfo);
                    UsefulFunctions.CreateXML(_FileLocation, _data);
                    Debug.Log("Main trial data Saved");
                    _data = UsefulFunctions.SerializeObject(trialPath);
                    UsefulFunctions.CreateXML(_FileLocation, _data);
                    Debug.Log("Overall path data Saved");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError(ex.ToString());
                }
                finally
                {
                    isSaving = false;
                }
            }
        }
    }

    //Object collision
    public void OnTriggerEnter(Collider col)
    {
        AudioSource audio = player.GetComponent<AudioSource>();
        audio.Play();
        score++;
        gems_info.text = "GEMS: " + score.ToString() + "/" + menu_score.ToString();
        UsefulFunctions.MainInfoSaving(collectObj);
        collision_t = trial_t;
        collectObj.SetActive(false);
        if (score != menu_score)
            collectObj = UsefulFunctions.ChooseGem();
    }

}
