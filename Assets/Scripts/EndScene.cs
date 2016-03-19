using UnityEngine;
using System.Collections;

public class EndScene : MonoBehaviour {

    public void onCancelClick()
    {
        Application.Quit();
    }

    public void onRestartClick()
    {
        Debug.Log("Restarting...");
        Debug.Log(PlayerPrefs.GetInt("EnvOrder"));
        Debug.Log(UsefulFunctions.env1);
        Application.LoadLevel(0);
    }
}
