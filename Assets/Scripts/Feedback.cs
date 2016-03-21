using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class Feedback : MonoBehaviour
{

    public GameObject startTrialMenu,feedbackMenu,firstTime,quitButton,nextButton,treasureFull,treasureMedium,treasureEmpty;
    public Text score_gained, feedback_text;
    public int d,error, score_ui;
    
    void Awake()
    {
        if (UsefulFunctions.current_trial == 0)
        {
            startTrialMenu.SetActive(true);
            feedbackMenu.SetActive(false);
        }
        else
        {
            startTrialMenu.SetActive(false);
            feedbackMenu.SetActive(true);
        }

    }
    
    void Start()
    {
        FeedbackVisualization();
    }

    public void FeedbackVisualization()
    {
        //Saves info about feedback
        if (!startTrialMenu.activeSelf)
        {
            
            GemsCollection.feedbackInfo = new DistanceAndFeedBack.FeedbackInfo();
            GemsCollection.feedbackInfo.ID = PlayerPrefs.GetString("SubjID") + UsefulFunctions.current_trial;
            GemsCollection.feedbackInfo.g_x = GemsCollection.goal_position.x;
            GemsCollection.feedbackInfo.g_y = GemsCollection.goal_position.y;
            GemsCollection.feedbackInfo.g_z = GemsCollection.goal_position.z;
            GemsCollection.feedbackInfo.e_x = GemsCollection.last_position.x;
            GemsCollection.feedbackInfo.e_y = GemsCollection.last_position.y;
            GemsCollection.feedbackInfo.e_z = GemsCollection.last_position.z;
            GemsCollection.feedbackInfo.distance = GemsCollection.distance;

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

            if (d < 25 && d >= 0)
            {
                treasureFull.SetActive(true);
                score_ui = int.Parse(score_t);
                score_ui = score_ui + 100;
                score_gained.text = score_ui.ToString();
                feedback_text.text = "Well done mate! You gained 100 coins!! Keep it up!";
                UsefulFunctions.tot_score = score_gained;
                GemsCollection.feedbackInfo.color = "Green";
            }
            else if (d < 75 && d >= 25)
            {
                treasureMedium.SetActive(true);
                score_ui = int.Parse(score_t);
                score_ui = score_ui + 50;
                score_gained.text = score_ui.ToString();
                feedback_text.text = "Almost there mate! 50 coins for you!";
                UsefulFunctions.tot_score = score_gained;
                GemsCollection.feedbackInfo.color = "Yellow";
            }
            else if (d >= 100)
            {
                treasureEmpty.SetActive(true);
                score_ui = int.Parse(score_t);
                score_ui = score_ui + 0;
                score_gained.text = score_ui.ToString();
                feedback_text.text = "Better luck next time, mate!";
                UsefulFunctions.tot_score = score_gained;
                GemsCollection.feedbackInfo.color = "Red";
            }
        }
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(GameObject.FindGameObjectWithTag("FeedbackContinue"));
    }
    

    public void onQuitClick()
    {
        Application.Quit();
    }

    public void onContinueClick()
    {
        UsefulFunctions.RndEnvironment();
    }

}


