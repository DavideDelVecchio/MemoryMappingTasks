using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class Feedback : MonoBehaviour
{

    public GameObject startTrialMenu,feedbackMenu, scoreMenu,firstTime,quitButton,nextButton,treasureFull,treasureMedium,treasureEmpty, gems_collect,gems_panel,trialPanel;
    public Text score_gained, feedback_text,totTrials;
    public int d,error, score_ui,tot_score;
    public bool halfWayThroughIt = false;
    
    void Awake()
    {
        if (UsefulFunctions.current_trial == 0)
        {
            startTrialMenu.SetActive(true);
            feedbackMenu.SetActive(false);
            scoreMenu.SetActive(false);
        }
        else
        {
            startTrialMenu.SetActive(false);
            feedbackMenu.SetActive(true);
        }

        Debug.Log(UsefulFunctions.current_trial);
        Debug.Log("Sono nell'awake");

        if (UsefulFunctions.current_trial == 20 || UsefulFunctions.current_trial == 41)
        {
            nextButton.SetActive(false);
        }
    }
    
    void Start()
    {
        if(UsefulFunctions.current_trial != 0)
            FeedbackVisualization();
    }

    public void FeedbackVisualization()
    {
        //Saves info about feedback
        if (!startTrialMenu.activeSelf)
        {
            
            Demo.feedbackInfo = new DistanceAndFeedBack.FeedbackInfo();
            Demo.feedbackInfo.ID = PlayerPrefs.GetString("SubjID") + UsefulFunctions.current_trial;
            Demo.feedbackInfo.g_x = Demo.goal_position.x;
            Demo.feedbackInfo.g_y = Demo.goal_position.y;
            Demo.feedbackInfo.g_z = Demo.goal_position.z;
            Demo.feedbackInfo.rg_x = Demo.goal_rotation.x;
            Demo.feedbackInfo.rg_y = Demo.goal_rotation.y;
            Demo.feedbackInfo.rg_z = Demo.goal_rotation.z;
            Demo.feedbackInfo.e_x = Demo.last_position.x;
            Demo.feedbackInfo.e_y = Demo.last_position.y;
            Demo.feedbackInfo.e_z = Demo.last_position.z;
            Demo.feedbackInfo.re_x = Demo.last_rotation.x;
            Demo.feedbackInfo.re_y = Demo.last_rotation.y;
            Demo.feedbackInfo.re_z = Demo.last_rotation.z;
            Demo.feedbackInfo.distance = Demo.distance;

            trialPanel.SetActive(true);
            totTrials.text = "Trials: " + UsefulFunctions.current_trial.ToString() + "/" + 61;


            d = (int)Demo.distance;
            string score_t;
            if (score_gained.text != string.Empty)
            {
                score_t = score_gained.text;
            }
            else
            {
                score_t = "0";
            }

            if (d < 25)
            {
                treasureFull.SetActive(true);
                score_ui = int.Parse(score_t);
                score_ui = score_ui + 100;
                score_gained.text = score_ui.ToString();
                feedback_text.text = "Well done mate! You gained 100 coins!! Keep it up!";
                tot_score = int.Parse(PlayerPrefs.GetString("Score"));
                tot_score = tot_score + score_ui;
                PlayerPrefs.SetString("Score", tot_score.ToString());
                Demo.feedbackInfo.color = "Green";
            }
            else if (d < 75 && d >= 25)
            {
                treasureMedium.SetActive(true);
                score_ui = int.Parse(score_t);
                score_ui = score_ui + 50;
                score_gained.text = score_ui.ToString();
                feedback_text.text = "Almost there mate! 50 coins for you!";
                tot_score = int.Parse(PlayerPrefs.GetString("Score"));
                tot_score = tot_score + score_ui;
                PlayerPrefs.SetString("Score", tot_score.ToString());
                Demo.feedbackInfo.color = "Yellow";
            }
            else if (d >= 75)
            {
                treasureEmpty.SetActive(true);
                score_ui = int.Parse(score_t);
                score_ui = score_ui + 0;
                score_gained.text = score_ui.ToString();
                feedback_text.text = "Better luck next time, mate!";
                tot_score = int.Parse(PlayerPrefs.GetString("Score"));
                tot_score = tot_score + score_ui;
                PlayerPrefs.SetString("Score", tot_score.ToString());
                Demo.feedbackInfo.color = "Red";
            }
        }
        Demo.trialFeedback.Add(Demo.feedbackInfo);
        if (UsefulFunctions.current_trial == 20 || UsefulFunctions.current_trial == 41)
        {
            StartCoroutine(ChangeOfEnvironmentFeedback());
            UsefulFunctions.endBlock = false;
        }
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(GameObject.FindGameObjectWithTag("FeedbackContinue"));

    }
    

    public void onQuitClick()
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
        Application.LoadLevel(6);
    }

    public void onContinueClick()
    {
        UsefulFunctions.RndEnvironment();

    }

    public IEnumerator ChangeOfEnvironmentFeedback()
    {
        Debug.Log("Waiting few seconds...");
        yield return new WaitForSeconds(1f);
        Debug.Log("Exiting 5s..Showing feedback");
        if (UsefulFunctions.current_trial == 20)
        {
            feedback_text.text = "Hey mate, they've found other lost gems in a new part of the island. We should check!";
        }
        else if(UsefulFunctions.current_trial == 41){
            feedback_text.text = "Good job pirate! Hope you're not tired. The last gems are waiting for us in this other side of the island!";
        }
        Debug.Log("Waiting again...");
        yield return new WaitForSeconds(2f);
        Debug.Log("Exiting the waiting..");
        nextButton.SetActive(true);
        UsefulFunctions.changeSky = true;
        yield return new WaitForSeconds(0f);
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(GameObject.FindGameObjectWithTag("FeedbackContinue"));
    }



}

