using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LeaderboardElement : MonoBehaviour {

    public Text subName;
    public Text task;
    public Text score;

    public void SetData (string name, int subScore)
    {
        subName.text = name;
        if (PlayerPrefs.GetInt("isMapping") == 1)
        {
            task.text = "Mapping";
        }  
        else
        {
            task.text = "Default";
        }
        score.text = string.Format("{0}", subScore);
    }

}
