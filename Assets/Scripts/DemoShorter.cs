using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DemoShorter : MonoBehaviour {

    public GameObject player, treasure,feedbackEndTrial, tornadoFeedback, tornado, paused_menu;
    public GameObject[] tornados,gems,randomPositions;
    int menu_score,score_ui;
    int dummy = 0;
    public static int score = 0;
    string _FileLocation, _data, subName;
    bool isSaving, startTrial,showFeedback,tilted,startRot,isPaused,checkForButton;
    public Text score_gained, gems_info;
    public Transform cameraTransform;
    public Material[] skies; 
    public static GameObject collectObj, pmenu;
    public static Vector3 goal_position, last_position;
    public static Quaternion goal_rotation, last_rotation;
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
        ShorterFunctions.levelCount++;
        //Player settings
        player.SetActive(true);
        //player.GetComponent<CharacterController>().enabled = false;
        //player.GetComponent<MouseLook>().enabled = false;
        Debug.Log("Activate player prefab");
        //Randomize gem position and assign it to the variable
        UsefulFunctions.isShorterVersion = true;
        collectObj = UsefulFunctions.RandomizeGem(gems);
        ShorterFunctions.RandomizeGemPosition(collectObj,randomPositions);
        ShorterFunctions.SaveMainInfo(player); //Saves player and treasure position
        //Randomize number of collectable obj
        menu_score = 1;
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
        startRot = false;
        checkForButton = false;
        feedbackEndTrial.SetActive(false);
        if (UsefulFunctions.changeSky)
        {
            if (Camera.main != null)
            {
                int index = UsefulFunctions.SetSkyIndex(UsefulFunctions.skyIndex);
                Camera.main.GetComponent<Skybox>().material = skies[index];
                UsefulFunctions.changeSky = false;
            }
        }
        else
        {
            if(UsefulFunctions.skyIndex != - 1)
                Camera.main.GetComponent<Skybox>().material = skies[UsefulFunctions.skyIndex];
        }
        PlayerPrefs.SetInt("isMapping", 1);
    }

    // Use this for initialization
    void Start ()
    {
        //Define file location
        _FileLocation = Application.dataPath + "/SubTrialInfo";
        //Saves the treasure position
        goal_position = collectObj.transform.position;
        goal_rotation = collectObj.transform.rotation;
        BackgroundMusicController.singleton.onSwitchPression(UsefulFunctions.changeMusic);
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        trial_t += Time.deltaTime; //updates time
        isPaused = UsefulFunctions.isGamePaused();
        if (isPaused)
        {
            pmenu = paused_menu;
            pmenu.SetActive(true);
            pause_t = trial_t;
            //player.GetComponent<CharacterController>().enabled = false;
            //player.GetComponent<MouseLook>().enabled = false;
        }
        UsefulFunctions.PathTracing(player);
        if (score == menu_score && checkForButton)
        {
            feedbackEndTrial.GetComponent<Animator>().SetBool("hasToGoBack", showFeedback);
            if (UsefulFunctions.OnButtonPression() == true)
            {
                score_text = score_gained.text;
                ShorterFunctions.SaveMainInfo(player); //Saves last position
                last_position = player.transform.position;
                last_rotation = player.transform.rotation;
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
                Application.LoadLevel("FeedbackShorter"); //Load feedback
            }
        }
        //if(startTrial)
        //{
        //    dummy++;
        //    if (dummy == 80)
        //    {
        //        treasure.SetActive(false);
        //        player.GetComponent<MouseLook>().enabled = true;
        //        player.GetComponent<CharacterController>().enabled = true;
        //        tilted = true;
        //        player.GetComponent<Animator>().SetBool("Tilted", tilted);
        //        startRot = true;
        //    }
        //}
        //if (player.transform.rotation.eulerAngles.x == 0.0f && startRot)
        //{
        //    player.GetComponent<Animator>().enabled = false;
        //    startRot = false;
        //}


        /*if (Input.GetButton("Y"))
            UsefulFunctions.RndEnvironment();*/
    }

    //Object collision
    public void OnTriggerEnter(Collider col)
    {
        AudioSource audio = player.GetComponent<AudioSource>();
        audio.Play();
        score++;
        gems_info.text = "GEMS: " + score.ToString() + "/" + menu_score.ToString();
        ShorterFunctions.SaveMainInfo(collectObj);
        collision_t = trial_t;
        collectObj.SetActive(false);
        if (score != menu_score)
        {
            //collectObj = UsefulFunctions.ChooseGem(gems);
            collectObj = UsefulFunctions.RandomizeGem(gems);
        }
        else
        {
            player.GetComponent<CharacterController>().enabled = false;
            tornado.SetActive(true);
            foreach (GameObject g in tornados)
                g.SetActive(true);
            tornadoFeedback.SetActive(true);
            StartCoroutine(WaitForIt(3.5f));
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

    IEnumerator WaitForIt(float seconds)
    {
        Debug.Log("Waiting START..." + Time.time);
        yield return new WaitForSeconds(seconds);
        //UsefulFunctions.RndPositionObj(player);
        //player.transform.position = UsefulFunctions.MappingRandomize(player); NOT WORKING
        player.transform.position = new Vector3(550f, player.transform.position.y, 750f);
        player.transform.rotation = Quaternion.Euler(ShorterFunctions.rotVectorPlayer);
        //cameraTransform.rotation = Quaternion.Euler(ShorterFunctions.rotVectorPlayer);
        Debug.Log("Waiting END..." + Time.time);
        tornadoFeedback.SetActive(false);
        tornado.SetActive(false);
        foreach (GameObject g in tornados)
        {
            g.SetActive(false);
        }
        feedbackEndTrial.SetActive(true);
        showFeedback = true;
        player.GetComponent<CharacterController>().enabled = true;
        checkForButton = true;
    }

}
