using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TestTrial : MonoBehaviour
{

    public GameObject player, oculus_player, treasure, instruction_menu;
    int menu_score, score_ui;
    int score, dummy = 0;
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
        //If status is ok activates the right player GameObject, checking if Oculus toggle was on or off
        switch (PlayerPrefs.GetInt("Oculus"))//PlayerPrefs.GetInt("Oculus")
        {
            //Default case
            case 0:
                player.SetActive(true);
                oculus_player.SetActive(false);
                player.GetComponent<CharacterController>().enabled = false;
                player.GetComponent<MouseLook>().enabled = false;
                Debug.Log("Activate player prefab");
                break;
            //Oculus case
            case 1:
                player.SetActive(false);
                oculus_player.SetActive(true);
                oculus_player.GetComponent<OVRGamepadController>().enabled = false;
                oculus_player.GetComponent<OVRPlayerController>().enabled = false;
                Debug.Log("Activate Oculus OVR Player");
                break;
        }
        //Randomize gem position and assign it to the variable
        collectObj = UsefulFunctions.ChooseGem();
        //Randomize Player and Treasure position
        UsefulFunctions.RndPositionObj(player); //Randomize player and treasure chest position
        //Randomize number of collectable obj
        menu_score = 3;
        //Update score and gems UI
        gems_info.text = "GEMS: " + score.ToString() + "/" + menu_score.ToString();
        score_gained.text = "0";
        treasure.GetComponent<Animator>().enabled = false;
    }

    // Use this for initialization
    void Start()
    {
        //Show exp instructions
        instruction_menu.SetActive(true);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        trial_t += Time.deltaTime; //updates time
        if (score == menu_score)
        {
            if (UsefulFunctions.OnButtonPression() == true)
            {
                score_text = score_gained.text;
                Application.LoadLevel(6); //Feedback level
            }
        }
        if(treasure.GetComponent<Animator>().isActiveAndEnabled)
        {
            dummy++;
            if (dummy == 80)
            {
                treasure.SetActive(false);
                player.transform.rotation = Quaternion.Euler(new Vector3(0f, player.transform.rotation.eulerAngles.y, player.transform.rotation.eulerAngles.z));
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

