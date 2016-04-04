using UnityEngine;
using System.Collections;

public class EndScene : MonoBehaviour {

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

    public void onRestartClick()
    {
        Debug.Log("Restarting...");
        Application.LoadLevel(0);
    }
}
