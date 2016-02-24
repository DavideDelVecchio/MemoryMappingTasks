using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class GemsCollection : MonoBehaviour {

    public GameObject player, treasure,feedbackCamera;
    public static GameObject collectObj;
    public static int currentEnv;
    int menu_score,score = 0;
    public static float pause_t, trial_t, collision_t = 0.0f;
    float old, duration = 0.0f;
    bool checkButtonPressed, isSaving;
    bool isNewTrial = false;
    public static string _FileName = "";
    string _FileLocation, _data, subName;
    public static List<MainTrialInfo.InfoTrial> trialInfo = new List<MainTrialInfo.InfoTrial>();
    public static List<CompletePath.PathMapping> trialPath = new List<CompletePath.PathMapping>();
    public Text score_gained, total_score;
    /*
    public Text countdown;
    int togo;
    Color transparentColor = Color.clear;
    Color opaqueColor = Color.black;
    */


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
            }
            else if (UsefulFunctions.current_trial > 1)
            {
                ReinitializeVariables();
                player = GameObject.FindGameObjectWithTag("Player");
                treasure = GameObject.FindGameObjectWithTag("Treasure");
                UsefulFunctions.gems = GameObject.FindGameObjectsWithTag("Gems");
            }
            //Set random number of obj to be collected
            menu_score = UsefulFunctions.RndObjNumber();
            total_score.text = menu_score.ToString();
            score_gained.text = score.ToString();
            //Randomize gem
            collectObj = UsefulFunctions.ChooseGem(); 
            //Checks if the trial is with or without Oculus to activate the right player prefab
            UsefulFunctions.ActivatePlayerPrefab();
            UsefulFunctions.RndPositionObj(player); //Randomize player and treasure chest position
            UsefulFunctions.MainInfoSaving(player); //Saves player and treasure position
            //Define file location
            _FileLocation = Application.dataPath + "/SubTrialInfo";
            //Store in variables prefabs info from menu
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
                //Feedback
                //Check env state and randomize obj number
                currentEnv = UsefulFunctions.RndEnvironment();
                menu_score = UsefulFunctions.RndObjNumber();
            }
        }
        /*
        if (Input.GetKey(KeyCode.Escape))
        {
            pause_menu.SetActive(true);
            pause_t = trial_t;
        }*/
    }

    //Object collision
    //Collision
    public void OnTriggerEnter(Collider col)
    {
        //Debug.Log("Loaded level: " + Application.loadedLevel);
        AudioSource audio = player.GetComponent<AudioSource>();
        audio.Play();
        score++;
        score_gained.text = score.ToString();
        UsefulFunctions.MainInfoSaving(collectObj);
        /*
        if (togo == 0)
        {
            countdown.color = Color.green;
        }
        else {
            togo--;
        }
        countdown.text = togo.ToString();*/
        collision_t = trial_t;
        /*
        if (Application.loadedLevel == 3 || Application.loadedLevel == 4)
        {
            duration = 1.0f;
            duration *= 0.5f;
            if (duration <= 0)
            {
                duration = Time.deltaTime / 2;
            }
            StartCoroutine(WaitForIt(duration));
            UsefulFunctions.RndJustPlayer(player);
            //Increase the mapping trial counter
            UsefulFunctions.mappingTrials++;
        }*/
        collectObj.SetActive(false);
        if (score != menu_score)
            collectObj = UsefulFunctions.ChooseGem();
        /*
        collectObj = gems[current_gem];
        UsefulFunctions.RandomizeObjPosition(collectObj);
        //Increase the path integration trial counter
        UsefulFunctions.pathTrials++;*/
    }

    public void ReinitializeVariables()
    {
        trial_t = 0.0f;
        collision_t = 0.0f;
        score = 0;
        menu_score = 0;
        _data = null;
        trialInfo.Clear();
        trialPath.Clear();
        Debug.Log("Variables reinitialized.");
    }


}
