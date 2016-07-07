using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

    string _FileLocation, _data;
    bool isSaving = false;

    public void QuitExp()
    {
        if (UsefulFunctions.current_trial > 0)
        {
            _FileLocation = Application.dataPath + "/SubTrialInfo";
            try
            {
                isSaving = true;
                _data = UsefulFunctions.SerializeObject(Demo.trialInfo);
                UsefulFunctions.CreateXML(_FileLocation, _data);
                Debug.Log("Main trial data Saved");
                _data = UsefulFunctions.SerializeObject(Demo.trialPath);
                UsefulFunctions.CreateXML(_FileLocation, _data);
                Debug.Log("Overall path data Saved");
                _data = UsefulFunctions.SerializeObject(Demo.trialFeedback);
                UsefulFunctions.CreateXML(_FileLocation, _data);
                Debug.Log("Feedback data Saved");
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.ToString());
            }
            finally
            {
                isSaving = false;
            }
        }

        Application.LoadLevel(6);
    }

    public void ResumeButton()
    {
        if(!UsefulFunctions.isShorterVersion)
        {
            Demo.trial_t = Demo.pause_t;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<CharacterController>().enabled = true;
            player.GetComponent<MouseLook>().enabled = true;
            Demo.pmenu.SetActive(false);
        }
        else
        {
            DemoShorter.trial_t = DemoShorter.pause_t;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<CharacterController>().enabled = true;
            player.GetComponent<MouseLook>().enabled = true;
            DemoShorter.pmenu.SetActive(false);
        }

    }
}
