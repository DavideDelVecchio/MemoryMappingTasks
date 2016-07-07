using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EndScene : MonoBehaviour {
    public int task, score, subjID;
    public string subjName;
    public bool isConnected = true;
    public bool showLeaderboard = false;
    public Text pirateFeedback;
    public GameObject quit;


    void Start()
    {
        StartCoroutine(CheckingConnection());

        if(isConnected && !UsefulFunctions.isShorterVersion)
        {
            quit.SetActive(false);
            //Post subject and score on the leaderboard based on the task
            subjName = PlayerPrefs.GetString("SubjID");
            score = int.Parse(PlayerPrefs.GetString("Score"));

            if (PlayerPrefs.GetInt("isMapping") == 0)
            {
                StartCoroutine(AzureManagerScript.singleton.PostPathScores(subjName, score));
            }
            else
            {
                StartCoroutine(AzureManagerScript.singleton.PostMappingScores(subjName, score));
            }
            pirateFeedback.text = "Thank you for your participation mate! Let's see how well you did...";
            if (!showLeaderboard)
            {
                showLeaderboard = true; //we want to make sure we only start the instruction coroutine *once*
                StartCoroutine(WaitForActionButton()); //starts the instruction coroutine!
            }
        }
        else
        {
            pirateFeedback.text = "Thank you for your participation mate!/n Arrrrrivederci!";
            quit.SetActive(true);
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(GameObject.FindGameObjectWithTag("FeedbackContinue"));
        }
    }

    public void onCancelClick()
    {
        string _data, _FileLocation;
        _FileLocation = Application.dataPath + "/SubTrialInfo";
        //End experiment
        try
        {
            _data = UsefulFunctions.SerializeObject(Demo.trialFeedback);
            UsefulFunctions.CreateXML(_FileLocation, _data);
            Debug.Log("Feedback data Saved");
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.ToString());
        }

        Application.Quit();
    }

    public IEnumerator CheckingConnection()
    {
        WWW web = new WWW("http://google.com");
        do { yield return null; } while (!web.isDone);

        string error = web.error;

        if (!string.IsNullOrEmpty(error))
        {
            print("No internet connection...Ending game...");
            isConnected = false;
        }
        else
        {
            print("Internet connection available, sending data to leaderboard..");
        }
    }

    //WaitForActionButton coroutine!
    public IEnumerator WaitForActionButton()
    {
        bool hasPressedButton = false;
        while (Input.GetAxis("A") != 0f)
        { //wait for the button to be released if it was pushed down
            yield return 0; //waits a frame each time until button is released so that we don't get stuck
        }
        while (!hasPressedButton)
        { //now wait for the button to be pushed again
            if (Input.GetAxis("A") == 1.0f)
            {
                hasPressedButton = true;
            }
            yield return 0; //waits a frame so that we don't get stuck
        }
        StartCoroutine(WaitForIt(1f));
        //once we reach the end of the function, the button has been successfully pressed!
    }

    IEnumerator WaitForIt(float seconds)
    {
        Debug.Log("Waiting START..." + Time.time);
        yield return new WaitForSeconds(seconds);
        Debug.Log("Waiting END..." + Time.time);
        Application.LoadLevel(7);
    }
}
