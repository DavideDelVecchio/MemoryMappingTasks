using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class Feedback : MonoBehaviour
{

    public GameObject startTrialMenu,feedbackMenu, scoreMenu,firstTime,quitButton,nextButton,treasureFull,treasureMedium,treasureEmpty, gems_collect,gems_panel;
    public Text score_gained, feedback_text;
    public int d,error, score_ui,tot_score, cc;
    public bool halfWayThroughIt = false;
    
    void Awake()
    {
        cc = 0;
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
            Demo.feedbackInfo.e_x = Demo.last_position.x;
            Demo.feedbackInfo.e_y = Demo.last_position.y;
            Demo.feedbackInfo.e_z = Demo.last_position.z;
            Demo.feedbackInfo.distance = Demo.distance;

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
        Debug.Log("Distance: " + d);
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
        if (UsefulFunctions.current_trial == 30)
            halfWayThroughIt = true;

        if(halfWayThroughIt)
        {
            treasureEmpty.SetActive(false);
            treasureFull.SetActive(false);
            treasureMedium.SetActive(false);
            feedback_text.text = "Good job pirate! You're half way through it." + Environment.NewLine + "Would you like to take a break?" + Environment.NewLine + "Press continue when you're ready.";
            halfWayThroughIt = false;
        }
        else
        {
            UsefulFunctions.RndEnvironment();
        }
    }

}


