using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;


public class GemsCollection : MonoBehaviour {

    public GameObject player, treasure,start_menu,pause_menu,feedback_menu, feedback_text_def, fd;
    public static GameObject collectObj;
    public static int currentEnv,score;
    public static Vector3 goal_position, last_position;
    int menu_score, score_ui; //barPercentage = 0;
    public static float pause_t, trial_t, collision_t, distance = 0.0f;
    float old, duration = 0.0f;
    bool checkButtonPressed, isSaving;
    bool isNewTrial, waitForButton = false;
    public static string _FileName, score_text = "";
    string _FileLocation, _data, subName;
    public static List<MainTrialInfo.InfoTrial> trialInfo = new List<MainTrialInfo.InfoTrial>();
    public static List<CompletePath.PathMapping> trialPath = new List<CompletePath.PathMapping>();
    public static DistanceAndFeedBack.FeedbackInfo feedbackInfo;
    public static List<DistanceAndFeedBack.FeedbackInfo> trialFeedback = new List<DistanceAndFeedBack.FeedbackInfo>();
    public Text score_gained, gems_info,feedback_text;
    public GameObject loadingBarR, loadingBarY, loadingBarG;
    public Text textIndicator, percentage;
    float speed = 30;
    public float currentAmount;
    public int error, barPercentage = 0;




    // Use this for initialization
    void Start () {
        if (UsefulFunctions.current_trial == UsefulFunctions.tot_trials)
        {
            Application.LoadLevel(4);
            Debug.Log("Experiment ended");
        }
        else {
            UsefulFunctions.current_trial++;
            if (UsefulFunctions.current_trial == 1)
            {
                currentEnv = PlayerPrefs.GetInt("CurrentEnv");
                //start_menu.SetActive(true);
            }
            else if (UsefulFunctions.current_trial > 1)
            {
                ReinitializeVariables();
                player = GameObject.FindGameObjectWithTag("Player");
                treasure = GameObject.FindGameObjectWithTag("Treasure");
                UsefulFunctions.gems = GameObject.FindGameObjectsWithTag("Gems");
            }
            //Set random number of obj to be collected
            menu_score = UnityEngine.Random.Range(1, 3);
            //Update score and gems info
            gems_info.text = "GEMS: " + score.ToString() + "/" + menu_score.ToString();
            score_gained.text = "0";
            //Randomize gem
            collectObj = UsefulFunctions.ChooseGem(collectObj); 
            UsefulFunctions.RndPositionObj(player); //Randomize player and treasure chest position
            UsefulFunctions.MainInfoSaving(player); //Saves player and treasure position
            //Define file location
            _FileLocation = Application.dataPath + "/SubTrialInfo";
            //Saves the treasure position
            goal_position = treasure.transform.position;
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
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
                //Check env state and randomize obj number
                //currentEnv = UsefulFunctions.RndEnvironment();
                menu_score = menu_score = UnityEngine.Random.Range(1, 3);
            }
        }
        //Checks if game is paused
        if(UsefulFunctions.isGamePaused())
        {
            pause_menu.SetActive(true);
            pause_t = trial_t;
            //Disable joystick movements
            player.GetComponent<CharacterController>().enabled = false;
            player.GetComponent<MouseLook>().enabled = false;
        }
    }

    //Object collision
    //Collision
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
            collectObj = UsefulFunctions.ChooseGem(collectObj);
    }


    //If resume button is pressed it restarts trial time from where it was before pausing
    public void ResumeButtonPressed()
    {
        if(pause_menu.activeSelf)
        {
            trial_t = pause_t;
            pause_menu.SetActive(false);
            //Player can move again
            player.GetComponent<CharacterController>().enabled = true;
            player.GetComponent<MouseLook>().enabled = true;
            Debug.Log("Game resumed");
        }
        else if (feedback_menu.activeSelf)
        {
            feedback_menu.SetActive(false);
            Debug.Log("Next Trial");
            waitForButton = true;
        }

    }

    //If exit button is pressed it saves all the data collected until now
    public void ExitButtonPressed()
    {
        //Saves data until that moment
        UsefulFunctions.MainInfoSaving(player); //Saves last position
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

        //Loads last page
        Application.LoadLevel(4);
        Debug.Log("Game ended...Saving data...");
    }

    //Reinitializes variables in the scene for new trial
    public void ReinitializeVariables()
    {
        trial_t = 0.0f;
        collision_t = 0.0f;
        score = 0;
        menu_score = 0;
        _data = null;
        waitForButton = false;
        trialInfo.Clear();
        trialPath.Clear();
        Debug.Log("Variables reinitialized.");
    }
}
