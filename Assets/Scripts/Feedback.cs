using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Feedback : MonoBehaviour
{

    public GameObject loadingBarR, loadingBarY, loadingBarG, fd, feedback_intro;
    public Text textIndicator, percentage;
    public float currentAmount, speed;
    public int error, score_ui;
    public int barPercentage = 0;
    public Text score_gained, feedback_text;
    public int distance;
    bool points_given = false;


    void Awake()
    {
        points_given = false;
    }


    void Update()
    {
        CheckError();
        switch (error)
        {
            case 25:
                if (currentAmount <= barPercentage)
                {
                    currentAmount += speed * Time.deltaTime;
                    percentage.text = ((int)currentAmount).ToString() + "%";
                }
                else
                {
                    textIndicator.text = "DONE!";
                }
                loadingBarG.SetActive(false);
                loadingBarY.SetActive(false);
                loadingBarR.SetActive(true);
                //Red loading
                loadingBarR.GetComponent<Image>().fillAmount = currentAmount / 100;
                break;
            case 75:
                if (currentAmount <= barPercentage)
                {
                    currentAmount += speed * Time.deltaTime;
                    percentage.text = ((int)currentAmount).ToString() + "%";
                }
                else
                {
                    textIndicator.text = "DONE!";
                }
                loadingBarG.SetActive(false);
                loadingBarR.SetActive(false);
                loadingBarY.SetActive(true);
                //Yellow loading
                loadingBarY.GetComponent<Image>().fillAmount = currentAmount / 100;
                break;
            case 100:
                if (currentAmount <= barPercentage)
                {
                    currentAmount += speed * Time.deltaTime;
                    percentage.text = ((int)currentAmount).ToString() + "%";
                }
                else
                {
                    textIndicator.text = "DONE!";
                }
                loadingBarY.SetActive(false);
                loadingBarR.SetActive(false);
                loadingBarG.SetActive(true);
                //Green loading
                loadingBarG.GetComponent<Image>().fillAmount = currentAmount / 100;
                break;
        }
        GemsCollection.feedbackInfo = new DistanceAndFeedBack.FeedbackInfo();
        GemsCollection.feedbackInfo.ID = PlayerPrefs.GetString("SubjID") + UsefulFunctions.current_trial;
        GemsCollection.feedbackInfo.g_x = GemsCollection.goal_position.x;
        GemsCollection.feedbackInfo.g_y = GemsCollection.goal_position.y;
        GemsCollection.feedbackInfo.g_z = GemsCollection.goal_position.z;
        GemsCollection.feedbackInfo.e_x = GemsCollection.last_position.x;
        GemsCollection.feedbackInfo.e_y = GemsCollection.last_position.y;
        GemsCollection.feedbackInfo.e_z = GemsCollection.last_position.z;
        GemsCollection.feedbackInfo.distance = GemsCollection.distance;
        if(!points_given)
            FeedbackTextVisualization();
    }


    void CheckError()
    {
        if ( distance <= 25) //GemsCollection.distance is 100
        {
            error = 100;
            if (distance < 12f)
            {
                barPercentage = 100;
            }
            else if (distance < 20f && distance >= 12f)
            {
                barPercentage = 80;
            }
            else
            {
                barPercentage = 75;
            }
        }
        else if (distance < 75f)
        {
            error = 75;
            if (distance > 60f)
            {
                barPercentage = 70;
            }
            else if (distance <= 60f && distance >= 40f)
            {
                barPercentage = 50;
            }
            else
            {
                barPercentage = 30;
            }
        }
        else
        {
            error = 25;
            barPercentage = 25;
        }
        /*
        if(distance <= 25) {
            error = 25;
        }
        else if (distance < 75) {
            error = 75;
        }
        else
        {
            error = 100;
        }*/
    }

    void FeedbackTextVisualization()
    {
        points_given = true;
        string score_t;
        if(score_gained.text != string.Empty)
        {
            score_t = score_gained.text;
        }
        else
        {
            score_t = "0";
        }
        if (error == 100)
        {
            score_ui = int.Parse(score_t);
            score_ui = score_ui + 0;
            score_gained.text = score_ui.ToString();
            feedback_intro.SetActive(false);
            fd.SetActive(true);
            feedback_text.text = "Better luck next time, mate!";
            GemsCollection.feedbackInfo.color = "Red";
        }
        else if (error == 75)
        {
            score_ui = int.Parse(score_t);
            score_ui = score_ui + 50;
            score_gained.text = score_ui.ToString();
            feedback_intro.SetActive(false);
            fd.SetActive(true);
            feedback_text.text = "Almost there mate! 50 coins for you!";
            GemsCollection.feedbackInfo.color = "Yellow";
        }
        else
        {
            score_ui = int.Parse(score_t);
            score_ui = score_ui + 100;
            score_gained.text = score_ui.ToString();
            feedback_intro.SetActive(false);
            fd.SetActive(true);
            feedback_text.text = "Well done mate! You gained 100 coins!! Keep it up!";
            GemsCollection.feedbackInfo.color = "Green";
        }
    }

}


