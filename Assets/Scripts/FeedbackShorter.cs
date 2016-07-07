using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class FeedbackShorter : MonoBehaviour
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
            
            DemoShorter.feedbackInfo = new DistanceAndFeedBack.FeedbackInfo();
            DemoShorter.feedbackInfo.ID = PlayerPrefs.GetString("SubjID") + UsefulFunctions.current_trial;
            DemoShorter.feedbackInfo.g_x = DemoShorter.goal_position.x;
            DemoShorter.feedbackInfo.g_y = DemoShorter.goal_position.y;
            DemoShorter.feedbackInfo.g_z = DemoShorter.goal_position.z;
            DemoShorter.feedbackInfo.rg_x = DemoShorter.goal_rotation.x;
            DemoShorter.feedbackInfo.rg_y = DemoShorter.goal_rotation.y;
            DemoShorter.feedbackInfo.rg_z = DemoShorter.goal_rotation.z;
            DemoShorter.feedbackInfo.e_x = DemoShorter.last_position.x;
            DemoShorter.feedbackInfo.e_y = DemoShorter.last_position.y;
            DemoShorter.feedbackInfo.e_z = DemoShorter.last_position.z;
            DemoShorter.feedbackInfo.re_x = DemoShorter.last_rotation.x;
            DemoShorter.feedbackInfo.re_y = DemoShorter.last_rotation.y;
            DemoShorter.feedbackInfo.re_z = DemoShorter.last_rotation.z;
            DemoShorter.feedbackInfo.distance = DemoShorter.distance;

            trialPanel.SetActive(true);
            totTrials.text = "Trials: " + UsefulFunctions.current_trial.ToString() + "/" + 11;


            d = (int)DemoShorter.distance;
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
                DemoShorter.feedbackInfo.color = "Green";
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
                DemoShorter.feedbackInfo.color = "Yellow";
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
                DemoShorter.feedbackInfo.color = "Red";
            }
        }
        DemoShorter.trialFeedback.Add(DemoShorter.feedbackInfo);

        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(GameObject.FindGameObjectWithTag("FeedbackContinue"));

    }
    

    public void onQuitClick()
    {
        string _data, _FileLocation;
        _FileLocation = Application.dataPath + "/SubTrialInfo";
        //End experiment
        try
        {
            _data = UsefulFunctions.SerializeObject(DemoShorter.trialFeedback);
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
        bool checkStatus = false;
        checkStatus = ShorterFunctions.ExpStatus();

        if(!checkStatus)
        { 
            if(ShorterFunctions.levelID == 1)
            {
                Application.LoadLevel(ShorterFunctions.geometry);
            }
            else if(ShorterFunctions.levelID == 2)
            {
                Application.LoadLevel(ShorterFunctions.distal_geometry);
            }
        }
        else
        {
            Application.LoadLevel("End");
        }

    }


    public void OnRetryClick()
    {
        UsefulFunctions.current_trial = 0;
        ShorterFunctions.reloadDemo = true;
        Application.LoadLevel("TestTrialShorter");
    }


}

