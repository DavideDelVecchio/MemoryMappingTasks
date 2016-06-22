using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TestTrial : MonoBehaviour
{

    public GameObject player, treasure, instruction_menu, feedbackEndTrial,tornadoFeedback, tornado;
    public GameObject[] tornados,gems;
    int menu_score, score_ui;
    int score, dummy = 0;
    bool showFeedback, tilted,startRot,checkForButton = false;
    float time_anim;
    public Text score_gained, gems_info;
    public static GameObject collectObj;
    public static Vector3 goal_position, last_position;
    public static float pause_t, trial_t, collision_t, distance = 0.0f;
    public static string _FileName, score_text = "";

    //Called before any other function
    void Awake()
    {
        UsefulFunctions.old_trial = UsefulFunctions.current_trial;
        //Player settings
        player.SetActive(true);
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<MouseLook>().enabled = false;
        Debug.Log("Activate player prefab");
        //Randomize gem position and assign it to the variable
        //collectObj = UsefulFunctions.ChooseGem(gems);
        //Randomize Player and Treasure position
        //UsefulFunctions.RndPositionObj(player); //Randomize player and treasure chest position
        player.transform.position = UsefulFunctions.RandomizingGameObject(player);
        collectObj = UsefulFunctions.RandomizeGem(gems);
        //Randomize number of collectable obj
        menu_score = 2;
        //Update score and gems UI
        gems_info.text = "GEMS: " + score.ToString() + "/" + menu_score.ToString();
        score_gained.text = "0";
        treasure.GetComponent<Animator>().enabled = false;
        feedbackEndTrial.SetActive(false);
    }

    // Use this for initialization
    void Start()
    {
        //Show exp instructions
        instruction_menu.SetActive(true);
        BackgroundMusicController.singleton.onSwitchPression(UsefulFunctions.changeMusic);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        trial_t += Time.deltaTime; //updates time
        if (score == menu_score && checkForButton)
        {
            if(time_anim == (trial_t + 5))
            {
                foreach (GameObject g in tornados)
                    g.GetComponent<ParticleSystem>().enableEmission = false;
            }
            feedbackEndTrial.GetComponent<Animator>().SetBool("hasToGoBack", showFeedback);
            if (UsefulFunctions.OnButtonPression() == true)
            {
                score_text = score_gained.text;
                UsefulFunctions.SetSkyIndex(UsefulFunctions.skyIndex);
                if(!UsefulFunctions.isShorterVersion)
                {
                    Application.LoadLevel("Feedback"); //Feedback level
                }
                else
                {
                    Application.LoadLevel("FeedbackShorter"); //Feedback level
                }
                
            }
        }
        if (treasure.GetComponent<Animator>().isActiveAndEnabled)
        {
            dummy++;
            if (dummy == 80)
            {
                treasure.SetActive(false);
                player.GetComponent<MouseLook>().enabled = true;
                tilted = true;
                player.GetComponent<Animator>().SetBool("Tilted", tilted);
                startRot = true;
            }
        }
        if (player.transform.rotation.eulerAngles.x == 0.0f && startRot)
        {
            player.GetComponent<Animator>().enabled = false;
            startRot = false;
        }

        /*if (Input.GetButton("Y"))
             UsefulFunctions.RndEnvironment();
         
         if (Input.GetButton("Y"))
             collectObj = UsefulFunctions.RandomizeGem(gems);

         if (Input.GetButton("X"))
             player.transform.position = UsefulFunctions.RandomizingGameObject(player);

         if (Input.GetButton("Y"))
         {
             int index = UsefulFunctions.SetSkyIndex(UsefulFunctions.skyIndex);
             Camera.main.GetComponent<Skybox>().material = skies[index];
         }*/
    }

    //Object collision
    public void OnTriggerEnter(Collider col)
    {
        AudioSource audio = player.GetComponent<AudioSource>();
        audio.Play();
        score++;
        gems_info.text = "GEMS: " + score.ToString() + "/" + menu_score.ToString();
        collision_t = trial_t;
        collectObj.SetActive(false);
        if (score != menu_score)
        {
            //collectObj = UsefulFunctions.RandomizeGem(gems);
            collectObj = UsefulFunctions.RandomizeGem(gems);
        }
        else
        {
            if(PlayerPrefs.GetInt("isMapping") == 1)
            {
                player.GetComponent<CharacterController>().enabled = false;
                tornado.SetActive(true);
                foreach (GameObject g in tornados)
                    g.SetActive(true);
                tornadoFeedback.SetActive(true);
                StartCoroutine(WaitForIt(3.5f));
            }
            else
            {
                feedbackEndTrial.SetActive(true);
                showFeedback = true;
                checkForButton = true;
            }
        }
    }

    IEnumerator WaitForIt(float seconds)
    {
        Debug.Log("Waiting START..." + Time.time);
        yield return new WaitForSeconds(seconds);
        //UsefulFunctions.RndPositionObj(player);
        //player.transform.position = UsefulFunctions.MappingRandomize(player); NOT WORKING
        UsefulFunctions.RandomizingGameObject(player);
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




