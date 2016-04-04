using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Linq;
using System.Collections;

public class UsefulFunctions : MonoBehaviour {
    public static GameObject[] gems,totems,invisibleBorders,islandItems,palms,woods,waves,fence;
    public static GameObject treasure;
    public static List<Vector3> envItems = new List<Vector3>();
    public static int tot_trials = 30;
    public static int old_trial, current_trial = 0;
    public static int max_repetition_env = 5;
    public static int env1, env2, env3 = 0;
    public static bool sameTrial = false;
    public static MainTrialInfo.InfoTrial info;// = new SubjTrialInfo.InfoTrial();
    public static CompletePath.PathMapping objPath;// = new PlayerInfo.PathMapping();
    

    //Main Trial information saving
    public static void MainInfoSaving(GameObject obj)
    {
        if ((obj.tag == "Player" && Demo.collision_t == 0) || (obj.tag == "OculusPlayer" && Demo.collision_t == 0))
        {
            info = new MainTrialInfo.InfoTrial();
            info.ID = "Start Position Player";
            info.s_x = obj.transform.position.x;
            info.s_y = obj.transform.position.y;
            info.s_z = obj.transform.position.z;
            info.time = Demo.trial_t;
            info.score = Demo.score;
            Demo.trialInfo.Add(info);
            info = new MainTrialInfo.InfoTrial();
            info.ID = "Treasure Position";
            info.s_x = treasure.transform.position.x;
            info.s_y = treasure.transform.position.y;
            info.s_z = treasure.transform.position.z;
            info.time = Demo.trial_t;
            info.score = Demo.score;
            Demo.trialInfo.Add(info);

        }
        else if (obj.tag == "Gems")
        {
            info = new MainTrialInfo.InfoTrial();
            info.ID = "Gem";
            info.s_x = obj.transform.position.x;
            info.s_y = obj.transform.position.y;
            info.s_z = obj.transform.position.z;
            info.time = Demo.trial_t;
            info.score = Demo.score;
            Demo.trialInfo.Add(info);
        }
        else
        {
            info = new MainTrialInfo.InfoTrial();
            info.ID = "End Trial Position";
            info.s_x = obj.transform.position.x;
            info.s_y = obj.transform.position.y;
            info.s_z = obj.transform.position.z;
            info.time = Demo.trial_t;
            info.score = Demo.score;
            info.d = Vector3.Distance(Demo.goal_position, Demo.last_position);
            Demo.trialInfo.Add(info);
        }
    }

    //Save overall path player
    public static void PathTracing(GameObject obj)
    {
        Vector3 current_position = obj.transform.position;
        Vector3 current_rotation = obj.transform.rotation.eulerAngles;
        objPath = new CompletePath.PathMapping();
        int k = Demo.trialPath.Count;
        if (k == 0 || Demo.trialPath[k - 1].s_x != current_position.x && Demo.trialPath[k - 1].s_z != current_position.z)
        {
            objPath.s_x = current_position.x;
            objPath.s_y = current_position.y;
            objPath.s_z = current_position.z;
            objPath.r_x = current_rotation.x;
            objPath.r_y = current_rotation.y;
            objPath.r_z = current_rotation.z;
            objPath.t = Demo.trial_t;
            Demo.trialPath.Add(objPath);
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
        //Fence
        fence = GameObject.FindGameObjectsWithTag("Fence");
        if (fence != null)
        {
            foreach(GameObject g in fence)
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
        string tag = obj.tag;
        if(treasure == null)
            treasure = GameObject.FindGameObjectWithTag("Treasure");

        Vector3 distancePT = new Vector3(treasure.transform.position.x - obj.transform.position.x, 0, treasure.transform.position.z - obj.transform.position.z); //Distance between player and the treasure
        Vector3 oldTreasurePosition = treasure.transform.position;
        Vector3 oldPosition = obj.transform.position;

        //Clears list env obj
        if (envItems.Count != 0)
        {
            envItems.Clear();
        }
        //Find obj in the environments
        EnvironmentalClues();

        //If the obj is the player we need to add the gem position
        //in constrast if obj is the gem we need to add player position
        if (tag == "Player" || tag == "OculusPlayer")
        {
            //envItems.Add(Demo.collectObj.transform.position);
        }
        else
        {
            switch (PlayerPrefs.GetInt("Oculus"))
            {
                //Default case
                case 0:
                    envItems.Add(GameObject.FindGameObjectWithTag("Player").transform.position);
                    break;
                //Oculus case
                case 1:
                    envItems.Add(GameObject.FindGameObjectWithTag("OculusPlayer").transform.position);
                    break;
            }
        }

        //Calculates new position
        while (checkPosition)
        {
            x = Random.Range(413.071f, 863.15f);
            z = Random.Range(528.554f, 828.4f);

            //Checks if the position is free
            Vector3 newPositionP = new Vector3(x, oldPosition.y, z);

            foreach (Vector3 v in envItems)
            {
                if ((v.x != newPositionP.x && v.z != newPositionP.z) || (v.x != newPositionP.x && v.z == newPositionP.z) || (v.x == newPositionP.x && v.z != newPositionP.z))
                {
                    obj.transform.position = newPositionP;
                    if(tag == "Player"|| tag == "OculusPlayer")
                    {
                        Vector3 newPositionT = new Vector3(newPositionP.x + distancePT.x, 0, newPositionP.z + distancePT.z);
                        treasure.transform.position = newPositionT;
                    }
                    checkPosition = false;
                }
                else
                {
                    obj.transform.position = oldPosition;
                    Debug.Log("Cannot change test position, there's already an object");
                    break;
                }
            }
        }
    }

    //Choose which gem to show
    public static GameObject ChooseGem(GameObject current_gem)
    {
        int index_current_gem = 4;
        if (gems == null || (current_trial != old_trial && !sameTrial))
        {
            gems = GameObject.FindGameObjectsWithTag("Gems");
            sameTrial = true;
        }
        else
        {
            sameTrial = false;
        }
        int i = Random.Range(0, 3);
        if(current_gem != null)
        {
            for (int k = 0; k < gems.Length; k++)
            {
                if (gems[k] == current_gem)
                {
                    index_current_gem = k;
                    break;
                }
            }
            while (index_current_gem == i)
            {
                i = Random.Range(0, 3);
            }
        }
        else
        {
            current_gem = gems[i];
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
    public static void RndEnvironment()
    {
        //Possible environmental order
        //0: Distal Cues, Distal Cues and Boundaries, Distal Cues and Landmarks 1 2 3
        //1: Distal Cues, Distal Cues and Landmarks, Distal Cues and Boundaries 1 3 2
        //2: Distal Cues and Boundaries, Distal Cues, Distal Cues and Landmarks 2 1 3
        //3: Distal Cues and Boundaries, Distal Cues and Landmarks, Distal Cues 2 3 1
        //4: Distal Cues and Landmarks, Distal Cues and Boundaries, Distal Cues 3 2 1
        //5: Distal Cues and Landmarks, Distal Cues, Distal cues and Boundaries 3 1 2
        
        //Checks the experimental status and ends it if necessary 
        CheckExpStatus();
        int env = PlayerPrefs.GetInt("EnvOrder");
        switch (env)
        {
            case 0:
                //Level 1, 2, 3
                if(env1 < 5 && env2 == 0 && env3 == 0)
                {
                    env1++;
                    Application.LoadLevel(1);
                }
                else if(env2 < 5 && env1 == 5 && env3 == 0)
                {
                    env2++;
                    Application.LoadLevel(2);
                }
                else if (env3 < 5 && env1 == 5 && env2 == 5)
                {
                    env3++;
                    Application.LoadLevel(3);
                }
                else if (env1 == 5 && env2 ==5 && env3 == 5)
                {
                    Application.LoadLevel(7);
                }
                break;
            case 1:
                //Level 1 3 2
                if (env1 < 5 && env2 == 0 && env3 == 0)
                {
                    env1++;
                    Application.LoadLevel(1);
                }
                else if (env3 < 5 && env1 == 5 && env2 == 0)
                {
                    env3++;
                    Application.LoadLevel(3);
                }
                else if (env2 < 5 && env1 == 5 && env3 == 5)
                {
                    env2++;
                    Application.LoadLevel(2);
                }
                else if (env1 == 5 && env2 == 5 && env3 == 5)
                {
                    Application.LoadLevel(7);
                }
                break;
            case 2:
                //Level 2 1 3
                if (env1 < 5 && env2 == 0 && env3 == 0)
                {
                    env1++;
                    Application.LoadLevel(1);
                }
                else if (env2 < 5 && env1 == 5 && env3 == 0)
                {
                    env2++;
                    Application.LoadLevel(2);
                }
                else if (env3 < 5 && env1 == 5 && env2 == 5)
                {
                    env3++;
                    Application.LoadLevel(3);
                }
                else if (env1 == 5 && env2 == 5 && env3 == 5)
                {
                    Application.LoadLevel(7);
                }
                break;
            case 3:
                //Level 2 3 1
                if (env2 < 5 && env1 == 0 && env3 == 0)
                {
                    env2++;
                    Application.LoadLevel(2);
                }
                else if (env3 < 5 && env2 == 5 && env1 == 0)
                {
                    env3++;
                    Application.LoadLevel(3);
                }
                else if (env1 < 5 && env2 == 5 && env3 == 5)
                {
                    env1++;
                    Application.LoadLevel(1);
                }
                else if (env1 == 5 && env2 == 5 && env3 == 5)
                {
                    Application.LoadLevel(7);
                }
                break;
            case 4:
                //Level 3 2 1
                if (env3 < 5 && env2 == 0 && env1 == 0)
                {
                    env3++;
                    Application.LoadLevel(3);
                }
                else if (env2 < 5 && env3 == 5 && env1 == 0)
                {
                    env2++;
                    Application.LoadLevel(2);
                }
                else if (env1 < 5 && env2 == 5 && env3 == 5)
                {
                    env1++;
                    Application.LoadLevel(1);
                }
                else if (env1 == 5 && env2 == 5 && env3 == 5)
                {
                    Application.LoadLevel(7);
                }
                break;
            case 5:
                //Level 3 1 2
                if (env3 < 5 && env1 == 0 && env1 == 0)
                {
                    env3++;
                    Application.LoadLevel(3);
                }
                else if (env1 < 5 && env3 == 5 && env2 == 0)
                {
                    env1++;
                    Application.LoadLevel(1);
                }
                else if (env2 < 5 && env1 == 5 && env3 == 5)
                {
                    env2++;
                    Application.LoadLevel(2);
                }
                else if (env1 == 5 && env2 == 5 && env3 == 5)
                {
                    Application.LoadLevel(7);
                }
                break;
        }
    }


    public static void CheckExpStatus()
    {
        if(current_trial == 15)
        {
            //Break point
            env1 = 0;
            env2 = 0;
            env3 = 0;
            Application.LoadLevel(5);
        }
        else if (current_trial == tot_trials)
        {
            string _data,_FileLocation;
            _FileLocation = Application.dataPath + "/SubTrialInfo";
            //End experiment
            try
            {
                _data = SerializeObject(Demo.trialFeedback);
                CreateXML(_FileLocation, _data);
                Debug.Log("Feedback data Saved");
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.ToString());
            }
            Application.LoadLevel(7);
        }
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
            Demo._FileName = PlayerPrefs.GetString("SubjID") + "_MainTrialInfo" + current_trial + ".xml";
        }
        else if (pObject.GetType() == typeof(List<CompletePath.PathMapping>))
        {
            xs = new XmlSerializer(typeof(List<CompletePath.PathMapping>));
            Demo._FileName = PlayerPrefs.GetString("SubjID") + "_CompleteTrialPath" + current_trial + ".xml";
        }
        else
        {
            xs = new XmlSerializer(typeof(List<DistanceAndFeedBack.FeedbackInfo>));
            Demo._FileName = PlayerPrefs.GetString("SubjID") + "_FeedbackTrials" + ".xml";
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
        FileInfo t = new FileInfo(_FileLocation + "/" + Demo._FileName);
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

