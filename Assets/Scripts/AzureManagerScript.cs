using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void AzureManagerHandler(object data);

public class AzureManagerScript : MonoBehaviour {

    public static AzureManagerScript singleton;

    public event AzureManagerHandler OnLeaderReceived;

    private const string ApiUrl = "http://leaderboardnavigationaltasks.azurewebsites.net/api/";

    void Awake()
    {
        singleton = this;
    }

    public IEnumerator PostMappingScores(string name, int scoreSubj)
    {
        WWWForm form = new WWWForm();
        form.AddField("SubjName", name);
        form.AddField("Score", scoreSubj);

        WWW web = new WWW(string.Format("{0}{1}", ApiUrl, "ScoreMappings"), form);
        do { yield return null; } while (!web.isDone);

        string error = web.error;

        if (string.IsNullOrEmpty(error))
        {
            print("Subject and score inserted in Mapping leaderboard...");
        }
        else
        {
            print(error);
        }
    }

    public IEnumerator PostPathScores(string name, int scoreSubj)
    {
        WWWForm form = new WWWForm();
        form.AddField("SubjName", name);
        form.AddField("Score", scoreSubj);

        WWW web = new WWW(string.Format("{0}{1}", ApiUrl, "ScorePIs"), form);
        do { yield return null; } while (!web.isDone);

        string error = web.error;

        if (string.IsNullOrEmpty(error))
        {
            print("Subject and score inserted in Path Integration leaderboard...");
        }
        else
        {
            print(error);
        }

    }

    public IEnumerator GetLeaderboard()
    {
        WWW web = null;
        if(PlayerPrefs.GetInt("isMapping")==1)
        {
           web = new WWW(string.Format("{0}{1}", ApiUrl, "ScoreMappings"));
        }
        else
        {
            web = new WWW(string.Format("{0}{1}", ApiUrl, "ScorePIs"));
        }

        do { yield return null; } while (!web.isDone);

        string error = web.error;

        if (string.IsNullOrEmpty(error))
        {
            List<object> scores = MiniJSON.Json.Deserialize(web.text) as List<object>;

            if (OnLeaderReceived != null) OnLeaderReceived(scores);
        }
        else
        {
            print(error);
        }
    }

}













/*public IEnumerator PostSubject(string name)
    {
        WWWForm form = new WWWForm();
        form.AddField("SubjNAme", name);

        WWW web = new WWW(string.Format("{0}{1}", ApiUrl, "Subjects"), form);

        do { yield return null; } while (!web.isDone);

        string error = web.error;

        if (string.IsNullOrEmpty(error))
        {
            print("Subject inserted");
        }
        else
        {
            print(error);
        }
    }

    public IEnumerator PostScore(int subjId, int taskId, int score)
    {
        WWWForm form = new WWWForm();
        form.AddField("TaskId", taskId);
        form.AddField("SubjId", subjId);
        form.AddField("SubjScore", score);

        WWW web = new WWW(string.Format("{0}{1}", ApiUrl, "Scores"), form);

        do { yield return null; } while (!web.isDone);

        string error = web.error;

        if (string.IsNullOrEmpty(error))
        {
            print("Score inserted");
        }
        else
        {
            print(error);
        }
    }*/

/*public void InsertSubject(string subject)
{
    StartCoroutine(AzureManagerScript.singleton.PostSubject(subject));
}
public void InsertSubject()
{
    StartCoroutine(AzureManagerScript.singleton.PostSubject("Guenda"));
    StartCoroutine(AzureManagerScript.singleton.PostScore(1, 1, 3000));
    StartCoroutine(AzureManagerScript.singleton.PostSubject("Filomena"));
    StartCoroutine(AzureManagerScript.singleton.PostScore(1, 2, 4000));
    StartCoroutine(AzureManagerScript.singleton.PostSubject("Giuseppina"));
    StartCoroutine(AzureManagerScript.singleton.PostScore(1, 3, 5000));
}

public void InsertScore(int taskId, int subId, int score)
{
    StartCoroutine(AzureManagerScript.singleton.PostScore(taskId,subId,score));
}*/
