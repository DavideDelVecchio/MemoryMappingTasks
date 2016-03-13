using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Linq;
using System.Collections;

public class UsefulFunctions : MonoBehaviour {
    public static GameObject[] gems,totems,invisibleBorders,islandItems,palms,woods,waves;
    public static List<Vector3> envItems = new List<Vector3>();
    public static List<int> levels = new List<int> {1,2,3}; //up to 5 if we add two more levels
    public static int tot_trials = 90;
    public static int current_level,current_trial,tot_env1, tot_env2, tot_env3, nobj1, nobj2, nobj3, old_env = 0;
    public static MainTrialInfo.InfoTrial info;// = new SubjTrialInfo.InfoTrial();
    public static CompletePath.PathMapping objPath;// = new PlayerInfo.PathMapping();
    


    //If Oculus is toggled activates the right player prefab
    public static void ActivatePlayerPrefab()
    {
        if (PlayerPrefs.GetInt("Oculus") == 0)
        {
            //Activate First Person player, the trial is without Oculus
            if (GameObject.FindGameObjectWithTag("Player") != null) {
                if (!GameObject.FindGameObjectWithTag("Player").activeSelf)
                {
                    GameObject.FindGameObjectWithTag("Player").SetActive(true);
                }
            }

            if (GameObject.FindGameObjectWithTag("OculusPlayer") != null)
            {
                if (GameObject.FindGameObjectWithTag("OculusPlayer").activeSelf)
                {
                    GameObject.FindGameObjectWithTag("OculusPlayer").SetActive(false);
                }
            }
        }
        else
        {
            //Activate OVR player, the trial is with Oculus
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                if (GameObject.FindGameObjectWithTag("Player").activeSelf)
                {
                    GameObject.FindGameObjectWithTag("Player").SetActive(false);
                }
            }

            if (GameObject.FindGameObjectWithTag("OculusPlayer") != null)
            {
                if (!GameObject.FindGameObjectWithTag("OculusPlayer").activeSelf)
                {
                    GameObject.FindGameObjectWithTag("OculusPlayer").SetActive(true);
                }
            }
        }
    }

    //Main Trial information saving
    public static void MainInfoSaving(GameObject obj)
    {
        if ((obj.tag == "Player" && GemsCollection.collision_t == 0) || (obj.tag == "OculusPlayer" && GemsCollection.collision_t == 0))
        {
            info = new MainTrialInfo.InfoTrial();
            info.ID = "Start Position Player";
            info.s_x = obj.transform.position.x;
            info.s_y = obj.transform.position.y;
            info.s_z = obj.transform.position.z;
            info.time = GemsCollection.trial_t;
            GemsCollection.trialInfo.Add(info);
            info = new MainTrialInfo.InfoTrial();
            info.ID = "Treasure Position";
            info.s_x = GameObject.FindGameObjectWithTag("Treasure").transform.position.x;
            info.s_y = GameObject.FindGameObjectWithTag("Treasure").transform.position.y;
            info.s_z = GameObject.FindGameObjectWithTag("Treasure").transform.position.z;
            info.time = GemsCollection.trial_t;
            GemsCollection.trialInfo.Add(info);

        }
        else if (obj.tag == "Gems")
        {
            info = new MainTrialInfo.InfoTrial();
            info.ID = "Gem";
            info.s_x = obj.transform.position.x;
            info.s_y = obj.transform.position.y;
            info.s_z = obj.transform.position.z;
            info.time = GemsCollection.trial_t;
            GemsCollection.trialInfo.Add(info);
        }
        else
        {
            info = new MainTrialInfo.InfoTrial();
            info.ID = "End Trial Position";
            info.s_x = obj.transform.position.x;
            info.s_y = obj.transform.position.y;
            info.s_z = obj.transform.position.z;
            info.time = GemsCollection.trial_t;
            info.d = Vector3.Distance(GemsCollection.goal_position, GemsCollection.last_position);
            GemsCollection.trialInfo.Add(info);
        }
    }

    //Save overall path player
    public static void PathTracing(GameObject obj)
    {
        Vector3 current_position = obj.transform.position;
        Vector3 current_rotation = obj.transform.rotation.eulerAngles;
        objPath = new CompletePath.PathMapping();
        int k = GemsCollection.trialPath.Count;
        if (k == 0 || GemsCollection.trialPath[k - 1].s_x != current_position.x && GemsCollection.trialPath[k - 1].s_z != current_position.z)
        {
            objPath.s_x = current_position.x;
            objPath.s_y = current_position.y;
            objPath.s_z = current_position.z;
            objPath.r_x = current_rotation.x;
            objPath.r_y = current_rotation.y;
            objPath.r_z = current_rotation.z;
            objPath.t = GemsCollection.trial_t;
            GemsCollection.trialPath.Add(objPath);
        }
    }

    //Finds environmental items in the scene
    public static void EnvironmentalClues()
    {
        //Totems
        totems = GameObject.FindGameObjectsWithTag("Totem");
        if(totems != null)
        {
            foreach (GameObject g in totems)
            {
                envItems.Add(g.transform.position);
            }
        }
        //Invisible borders
        invisibleBorders = GameObject.FindGameObjectsWithTag("InvisibleBorder");
        if (invisibleBorders != null)
        {
            foreach (GameObject g in invisibleBorders)
            {
                envItems.Add(g.transform.position);
            }
        }
        //Island items
        islandItems = GameObject.FindGameObjectsWithTag("IslandItems");
        if(islandItems != null)
        {
            foreach (GameObject g in islandItems)
            {
                envItems.Add(g.transform.position);
            }
        }
        //Palms
        palms = GameObject.FindGameObjectsWithTag("Palms");
        if (palms != null)
        {
            foreach (GameObject g in palms)
            {
                envItems.Add(g.transform.position);
            }
        }
        //Woods
        woods = GameObject.FindGameObjectsWithTag("Wood");
        if(woods != null)
        {
            foreach (GameObject g in woods)
            {
                envItems.Add(g.transform.position);
            }
        } 
        //Waves
        waves = GameObject.FindGameObjectsWithTag("Waves");
        if (waves != null)
        {
            foreach (GameObject g in waves)
            {
                envItems.Add(g.transform.position);
            }
        }
    }

    //Randomize the obj position in the scene
    public static void RndPositionObj(GameObject obj)
    {
        //Variables
        float x, z;
        bool checkPosition = true;
        Vector3 distancePT = new Vector3(0.28f, 0f, -1.52f);
        GameObject treasure = GameObject.FindGameObjectWithTag("Treasure");
        //Clears list env obj
        if (envItems.Count != 0) {
            envItems.Clear();
        }
        //Find obj in the environments
        EnvironmentalClues();

        //Calculates new position
        while (checkPosition)
        {
            x = Random.Range(258f, 914f);
            z = Random.Range(521f, 763f);

            if (258f <= x && x <= 304f)
            {
                z = Random.Range(752f, 770f);
            }
            else if (305f <= x && x <= 384f)
            {
                z = Random.Range(586f, 802f);
            }
            else if (914f <= x && x <= 971f) {
                z = Random.Range(763f, 823f);
            }

            Vector3 oldPosition = obj.transform.position;
            
            //If the obj is the player we need to add the gem position
            //in constrast if obj is the gem we need to add player position
            if (obj.tag == "Player" || obj.tag == "OculusPlayer")
            {
                Vector3 oldTreasurePosition = treasure.transform.position;
                envItems.Add(GemsCollection.collectObj.transform.position);
            }
            else
            {
                if (obj.tag == "Player")
                {
                    envItems.Add(GameObject.FindGameObjectWithTag("Player").transform.position);
                }
                else
                {
                    //envItems.Add(GameObject.FindGameObjectWithTag("OculusPlayer").transform.position);
                }
                
            }

            //Checks if the position is free
            Vector3 newPositionP = new Vector3(x, oldPosition.y, z);
            if ((totems == null && islandItems == null && palms == null && woods == null) || (!envItems.Contains(newPositionP)))
            {
                obj.transform.position = newPositionP;
                if(obj.tag == "Player" || obj.tag == "OculusPlayer")
                {
                    Vector3 newPositionT = new Vector3(newPositionP.x + distancePT.x, 0, newPositionP.z + distancePT.z);
                    treasure.transform.position = newPositionT;
                }
                checkPosition = false;
            }
        }
    }

    //Choose which gem to show
    public static GameObject ChooseGem()
    {
        if (gems == null)
        {
            gems = GameObject.FindGameObjectsWithTag("Gems");
        }
        int i = Random.Range(0, 3);
        while(GemsCollection.collectObj == gems[i])
        {
            i = Random.Range(0, 3);
        }
        switch (i)
        {
            case 0:
                gems[i].SetActive(true);
                gems[i + 1].SetActive(false);
                gems[i + 2].SetActive(false);
                gems[i + 3].SetActive(false);
                break;
            case 1:
                gems[i].SetActive(true);
                gems[i - 1].SetActive(false);
                gems[i + 1].SetActive(false);
                gems[i + 2].SetActive(false);
                break;
            case 2:
                gems[i].SetActive(true);
                gems[i - 1].SetActive(false);
                gems[i - 2].SetActive(false);
                gems[i + 1].SetActive(false);
                break;
            case 3:
                gems[i].SetActive(true);
                gems[i - 3].SetActive(false);
                gems[i - 2].SetActive(false);
                gems[i - 1].SetActive(false);
                break;
        }
        RndPositionObj(gems[i]);
        return gems[i];
    }

    //Randomize the environment
    public static int RndEnvironment()
    {
        Application.LoadLevel(2);
        return 2;
        /*
        if (levels.Count != 0)
        {
            int l = Random.Range(0, levels.Count); //to 4 if we consider 2 more environments
            switch (l)
            {
                case 0:
                    //Level 1: Distal cues only
                    if (tot_env1 != 30)
                    {
                        Application.LoadLevel(1);
                        break;
                    }
                    else if (tot_trials == 90)
                    {
                        Application.LoadLevel(4); //End scene
                    }
                    break;
                case 1:
                    //Level 2: Distal cues & Boundaries
                    if (tot_env2 != 30)
                    {
                        Application.LoadLevel(2);
                        break;
                    }
                    else if (tot_trials == 90)
                    {
                        Application.LoadLevel(4); //End scene
                    }
                    break;
                case 2:
                    //Level 3: Distal cues & Local Landmarks
                    if (tot_env3 != 30)
                    {
                        Application.LoadLevel(3);
                        break;
                    }
                    else if (tot_trials == 90)
                    {
                        Application.LoadLevel(4); //End scene
                    }
                    break;
            }
            return l+1;
        }
        else {
            if (tot_trials == 90)
            {
                bool isSaving;
                string _data;
                string _FileLocation = Application.dataPath + "/SubTrialInfo";
                try
                {
                    isSaving = true;
                    _data = SerializeObject(GemsCollection.trialFeedback);
                    UsefulFunctions.CreateXML(_FileLocation, _data);
                    Debug.Log("Main trial data Saved");
                    }
                catch (System.Exception ex)
                {
                    Debug.LogError(ex.ToString());
                }
                finally
                {
                    isSaving = false;
                }
                Application.LoadLevel(4); //End scene
            }
            return 4;
        } */
    }

    //Checks if the block is concluded
    public static int CheckEnvState(int e)
    {
        int isEnvReachedLimit = 2; //0 when is not, 1 when it has reached 30 trials
        switch (e)
        {
            case 0:
                if (tot_env1 == 30)
                {
                    levels.Remove(1);
                    //Reset to zero obj counters
                    nobj1 = 0;
                    nobj2 = 0;
                    nobj3 = 0;
                    isEnvReachedLimit = 1;
                    break;
                }
                else {
                    tot_env1++;
                    isEnvReachedLimit = 0;
                    GemsCollection.currentEnv = 0;
                }
                break;
            case 1:
                if (tot_env2 == 30)
                {
                    levels.Remove(2);
                    //Reset to zero obj counters
                    nobj1 = 0;
                    nobj2 = 0;
                    nobj3 = 0;
                    isEnvReachedLimit = 1;
                    break;
                }
                else
                {
                    tot_env2++;
                    isEnvReachedLimit = 0;
                }
                break;
            case 2:
                if (tot_env3 == 30)
                {
                    levels.Remove(3);
                    //Reset to zero obj counters
                    nobj1 = 0;
                    nobj2 = 0;
                    nobj3 = 0;
                    isEnvReachedLimit = 1;
                    break;
                }
                else
                {
                    tot_env3++;
                    isEnvReachedLimit = 0;
                }
                break;
        }
        return isEnvReachedLimit;
    }

    //Randomize the number of obj to be collected
    public static int RndObjNumber()
    {
        bool isLimitReached = true;
        int o = 0;
        while (isLimitReached)
        {
            o = Random.Range(1, 3); //min and max of collectable obj in the scene during trial
            switch (o)
            {
                case 1:
                    if (nobj1 < 10)
                    {
                        nobj1++;
                        isLimitReached = false;
                        break;
                    }
                    break;
                case 2:
                    if (nobj2 < 10)
                    {
                        nobj2++;
                        isLimitReached = false;
                        break;
                    }
                    break;
                case 3:
                    if (nobj3 < 10)
                    {
                        nobj3++;
                        isLimitReached = false;
                        break;
                    }
                    break;
            }
        }
        return o; 
    }

    //Checks if a button is pressed
    public static bool OnButtonPression()
    {
        bool check = false;
        if ((GameObject.FindGameObjectWithTag("Player") != null) || (GameObject.FindGameObjectsWithTag("OculusPlayer") != null))
        {
            //Allows input from Xbox360 controller, generic controller and Mac users
            if (Input.GetButton("A") || Input.GetButton("OtherX") || Input.GetButton("MacX"))
            {
                check = true;
            }
        }
        else {
            check = false;
        }

        return check;
    }

    //Checks if player pressed pause
    public static bool isGamePaused()
    {
        bool check = false;

        if ((GameObject.FindGameObjectWithTag("Player") != null) || (GameObject.FindGameObjectsWithTag("OculusPlayer") != null))
        {
            //Allows input from Xbox360 controller, generic controller and Mac users
            if (Input.GetButton("BackButton") || Input.GetButton("BackButtonMac") || Input.GetButton("GenericBackButton"))
            {
                check = true;
            }
        }
        else {
            check = false;
        }
        return check;
    }

    //Save info into XML files
    /* The following metods came from the referenced URL */
    public static string UTF8ByteArrayToString(byte[] characters)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        string constructedString = encoding.GetString(characters);
        return (constructedString);
    }

    public static byte[] StringToUTF8ByteArray(string pXmlString)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        byte[] byteArray = encoding.GetBytes(pXmlString);
        return byteArray;
    }


    // Here we serialize our UserData object of myData 
    public static string SerializeObject(object pObject)
    {
        string XmlizedString = null;
        XmlSerializer xs;
        MemoryStream memoryStream = new MemoryStream();
        if (pObject.GetType() == typeof(List<MainTrialInfo.InfoTrial>))
        {
            xs = new XmlSerializer(typeof(List<MainTrialInfo.InfoTrial>));
            GemsCollection._FileName = PlayerPrefs.GetString("SubjID") + "_MainTrialInfo" + current_trial + ".xml";
        }
        else if (pObject.GetType() == typeof(List<CompletePath.PathMapping>))
        {
            xs = new XmlSerializer(typeof(List<CompletePath.PathMapping>));
            GemsCollection._FileName = PlayerPrefs.GetString("SubjID") + "_CompleteTrialPath" + current_trial + ".xml";
        }
        else
        {
            xs = new XmlSerializer(typeof(List<DistanceAndFeedBack.FeedbackInfo>));
            GemsCollection._FileName = PlayerPrefs.GetString("SubjID") + "_FeedbackTrials" + ".xml";
        }
        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        xmlTextWriter.Formatting = Formatting.Indented;
        xs.Serialize(xmlTextWriter, pObject);
        memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
        XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
        return XmlizedString;
    }

    // Finally our save and load methods for the file itself 
    public static void CreateXML(string _FileLocation, string _data)
    {
        StreamWriter writer;
        FileInfo t = new FileInfo(_FileLocation + "/" + GemsCollection._FileName);
        if (!t.Exists)
        {
            writer = t.CreateText();
        }
        else
        {
            t.Delete();
            writer = t.CreateText();
        }
        writer.Write(_data);
        writer.Close();
        Debug.Log("File written.");
    }
}


/*
if (waitForButton) {
			yield return StartCoroutine (WaitForActionButton ());
		}

	public IEnumerator WaitForActionButton(){
		bool hasPressedButton = false;
		while(Input.GetAxis("Action Button") != 0f){
			yield return 0;
		}
		while(!hasPressedButton){
			if(Input.GetAxis("Action Button") == 1.0f){
				hasPressedButton = true;
			}
			yield return 0;
		}
	}

    public static bool nextButton()
    {
        bool check = false;

        if ((GameObject.FindGameObjectWithTag("Player") != null) || (GameObject.FindGameObjectsWithTag("OculusPlayer") != null))
        {
            //Allows input from Xbox360 controller, generic controller and Mac users
            if (Input.GetButton("Ok") || Input.GetButton("OkMac") || Input.GetButton("OkGeneric"))
            {
                check = true;
            }
        }
        else {
            check = false;
        }
        return check;

    }*/

