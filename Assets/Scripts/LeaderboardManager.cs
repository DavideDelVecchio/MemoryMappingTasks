using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class LeaderboardManager : MonoBehaviour
{

    public GameObject subjInfoPrefab;
    public GameObject elementsPanel;
    public bool isQuitting = false;

    void Start()
    {
        AzureManagerScript.singleton.OnLeaderReceived += Singleton_OnLeaderReceived;
        StartCoroutine(AzureManagerScript.singleton.GetLeaderboard());
        if (!isQuitting)
        {
            isQuitting = true; //we want to make sure we only start the instruction coroutine *once*
            StartCoroutine(WaitForActionButton()); //starts the instruction coroutine!
        }

    }

    private void Singleton_OnLeaderReceived(object data)
    {
        foreach (Dictionary<string, object> item in data as List<object>)
        {
            GameObject subj = Instantiate(subjInfoPrefab) as GameObject;
            LeaderboardElement le = subj.GetComponent<LeaderboardElement>();

            le.SetData(item["SubjName"].ToString(),
                int.Parse(item["Score"].ToString()));

            subj.transform.SetParent(elementsPanel.transform);
            subj.transform.localScale = Vector3.one;
        }
    }

    public void OnDisable()
    {
        AzureManagerScript.singleton.OnLeaderReceived -= Singleton_OnLeaderReceived;
    }

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
        QuittingGame();
    }

    public void QuittingGame()
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
}
