using UnityEngine;
using System.Collections;

public class GemsCollection : MonoBehaviour {

    public GameObject player, treasure,feedbackCamera;
    public static GameObject collectObj;
    int menu_score,score = 0;
    static float pause_t, trial_t, collision_t = 0.0f;
    float old, duration = 0.0f;
    bool checkButtonPressed;
    bool isNewTrial = false;
    public static string _FileName = "";
    string _FileLocation, _data, subName;
    /*
    public Text countdown;
    int togo;
    public static List<SubjTrialInfo.InfoTrial> trialInfo = new List<SubjTrialInfo.InfoTrial>();
    public static List<PlayerInfo.PathMapping> trialPath = new List<PlayerInfo.PathMapping>();
    Color transparentColor = Color.clear;
    Color opaqueColor = Color.black;
    */


    // Use this for initialization
    void Start () {
        collectObj = UsefulFunctions.ChooseGem(); //Already randomized
        UsefulFunctions.RndPositionObj(player); //Randomize player and treasure chest position
        //Define file location
        _FileLocation = Application.dataPath + "/SubTrialInfo";
        //Store in variables prefabs info from menu
    }
	
	// Update is called once per frame
	void Update () {
        trial_t += Time.deltaTime;
        /*
        if (score == menu_score)
        {
            checkButtonPressed = UsefulFunctions.OnButtonPression();
            if (checkButtonPressed == true)
            {
                UsefulFunctions.MainObjInfo(player);
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
                    UsefulFunctions.trials++;
                }
                catch (System.Exception ex)
                {
                    Debug.LogError(ex.ToString());
                }
                finally
                {
                    isSaving = false;
                }
                //Loads new scene
                Application.LoadLevel(5);
            }
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            pause_menu.SetActive(true);
            pause_t = trial_t;
        }
        */
    }

    //Object collision
    //Collision
    public void OnTriggerEnter(Collider col)
    {
        //Debug.Log("Loaded level: " + Application.loadedLevel);
        AudioSource audio = player.GetComponent<AudioSource>();
        audio.Play();
        score++;
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
        UsefulFunctions.MainObjInfo(GameObject.FindGameObjectWithTag("Gem"));
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
        collectObj = UsefulFunctions.ChooseGem();
        /*
        collectObj = gems[current_gem];
        UsefulFunctions.RandomizeObjPosition(collectObj);
        //Increase the path integration trial counter
        UsefulFunctions.pathTrials++;*/
    }
}
