using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Demo : MonoBehaviour {

    public GameObject player, oculus_player, treasure,feedbackEndTrial,tornado;
    int menu_score, score_ui;
    int dummy = 0;
    public static int score = 0;
    string _FileLocation, _data, subName;
    bool isSaving, startTrial,showFeedback,tilted;
    public Text score_gained, gems_info;
    public static GameObject collectObj;
    public static Vector3 goal_position, last_position;
    public static float pause_t, trial_t, collision_t, distance = 0.0f;
    public static string _FileName, score_text = "";
    public static DistanceAndFeedBack.FeedbackInfo feedbackInfo;
    public static List<MainTrialInfo.InfoTrial> trialInfo = new List<MainTrialInfo.InfoTrial>();
    public static List<CompletePath.PathMapping> trialPath = new List<CompletePath.PathMapping>();
    public static List<DistanceAndFeedBack.FeedbackInfo> trialFeedback = new List<DistanceAndFeedBack.FeedbackInfo>();

    //Called before any other function
    void Awake()
    {
        //If reloads the trial, reinitialize the variables
        if (UsefulFunctions.current_trial > 0)
            ReinitializeVariables();
        //Increase trial counter
        UsefulFunctions.old_trial = UsefulFunctions.current_trial;
        UsefulFunctions.sameTrial = false;
        UsefulFunctions.current_trial++;
        //If status is ok activates the right player GameObject, checking if Oculus toggle was on or off
        switch (PlayerPrefs.GetInt("Oculus"))
        {
            //Default case
            case 0:
                player.SetActive(true);
                oculus_player.SetActive(false);
                player.GetComponent<MouseLook>().enabled = false;
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
        collectObj = UsefulFunctions.ChooseGem(collectObj);
        //Randomize Player and Treasure position
        UsefulFunctions.RndPositionObj(player); //Randomize player and treasure chest position
        UsefulFunctions.MainInfoSaving(player); //Saves player and treasure position
        //Randomize number of collectable obj
        menu_score = Random.Range(1, 3);
        //Update score and gems UI
        gems_info.text = "GEMS: " + score.ToString() + "/" + menu_score.ToString();
        if (UsefulFunctions.current_trial == 0 || UsefulFunctions.current_trial == 1)
        {
            score_gained.text = "0";
        }
        else {
            score_gained.text = PlayerPrefs.GetString("Score");
        }
        startTrial = true;
        showFeedback = false;
        tilted = false;
        feedbackEndTrial.SetActive(false);
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
	void FixedUpdate ()
    {
        trial_t += Time.deltaTime; //updates time
        UsefulFunctions.PathTracing(player);
        if (score == menu_score)
        {
            feedbackEndTrial.GetComponent<Animator>().SetBool("hasToGoBack", showFeedback);
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
                Application.LoadLevel(6); //Load feedback
            }
        }
        if(startTrial)
        {
            if (dummy == 80)
            {
                treasure.SetActive(false);
                tilted = true;
                player.GetComponent<MouseLook>().enabled = true;
                startTrial = false;
                player.GetComponent<Animator>().SetBool("Tilted", tilted);
            }
            dummy++;
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
        {
            collectObj = UsefulFunctions.ChooseGem(collectObj);
        }
        else
        {
            
            feedbackEndTrial.SetActive(true);
            showFeedback = true;
        }
    }

    //Reinitializes variables in the scene for new trial
    public void ReinitializeVariables()
    {
        trial_t = 0.0f;
        collision_t = 0.0f;
        dummy = 0;
        score = 0;
        menu_score = 0;
        _data = null;
        trialInfo.Clear();
        trialPath.Clear();
        Debug.Log("Variables reinitialized.");
    }

}
