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
    public static string level1, level2, level3;
    public static List<Vector3> envItems = new List<Vector3>();
    public static int current_gem_number;
    public static int tot_trials = 60;
    public static int old_trial, current_trial = 0;
    public static int env1, env2, env3 = 1;
    public static int changeMusic= 0;
    public static int skyIndex = -1;
    public static bool sameTrial, endBlock, changeSky, isDisorientation, isShorterVersion = false;
    public static List<int> alreadyUsedSky = new List<int>();
    public static MainTrialInfo.InfoTrial info;// = new SubjTrialInfo.InfoTrial();
    public static CompletePath.PathMapping objPath;// = new PlayerInfo.PathMapping();

    const float X = 0.9f;
    const float Z = -10.0f;
    public static Vector3 old_PT_pos, old_gem_pos;
    public static int old_gem_index = -1;

    

    //Main Trial information saving
    public static void MainInfoSaving(GameObject obj)
    {
        if ((obj.tag == "Player" && Demo.collision_t == 0))
        {
            info = new MainTrialInfo.InfoTrial();
            info.ID = "Start Position Player";
            info.s_x = obj.transform.position.x;
            info.s_y = obj.transform.position.y;
            info.s_z = obj.transform.position.z;
            info.r_x = obj.transform.rotation.x;
            info.r_y = obj.transform.rotation.y;
            info.r_z = obj.transform.rotation.z;
            info.time = Demo.trial_t;
            info.score = PlayerPrefs.GetString("Score");
            Demo.trialInfo.Add(info);
            info = new MainTrialInfo.InfoTrial();
            info.ID = "Treasure Position";
            info.s_x = treasure.transform.position.x;
            info.s_y = treasure.transform.position.y;
            info.s_z = treasure.transform.position.z;
            info.r_x = treasure.transform.rotation.x;
            info.r_y = treasure.transform.rotation.y;
            info.r_z = treasure.transform.rotation.z;
            info.time = Demo.trial_t;
            info.score = PlayerPrefs.GetString("Score");
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
            info.score = PlayerPrefs.GetString("Score");
            Demo.trialInfo.Add(info);
        }
        else
        {
            info = new MainTrialInfo.InfoTrial();
            info.ID = "End Trial Position";
            info.s_x = obj.transform.position.x;
            info.s_y = obj.transform.position.y;
            info.s_z = obj.transform.position.z;
            info.r_x = obj.transform.rotation.x;
            info.r_y = obj.transform.rotation.y;
            info.r_z = obj.transform.rotation.z;
            info.time = Demo.trial_t;
            info.score = PlayerPrefs.GetString("Score");
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
        float check_prev_pos = 0.0f;
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
        if (tag == "Gems")
        {
            envItems.Add(GameObject.FindGameObjectWithTag("Player").transform.position);
            envItems.Add(treasure.transform.position);
        }
        //Calculates new position
        while (checkPosition)
        {
            x = Random.Range(400f, 701f);
            float upOrDown = Random.Range(0, 2);
            if (upOrDown == 0)
            {
                z = Random.Range(600f, 656f);
            }
            else
            {
                z = Random.Range(745f, 801f);
            }
            //z = Random.Range(600f, 800f);
            //Checks if the position is free
            Vector3 newPositionP = new Vector3(x, oldPosition.y, z);
            foreach (Vector3 v in envItems)
            {
                if ((v.x != newPositionP.x && v.z != newPositionP.z) || (v.x != newPositionP.x && v.z == newPositionP.z) || (v.x == newPositionP.x && v.z != newPositionP.z))
                {
                    obj.transform.position = newPositionP;
                    if (tag == "Player")
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
            check_prev_pos = Vector3.Distance(newPositionP, Demo.goal_position);
            if (tag == "Player" && check_prev_pos < 100)
            {
                checkPosition = true;
            }
            else if(tag == "Gems")
            {
                float dist_treas = Vector3.Distance(treasure.transform.position, newPositionP);
                float dist_play = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, newPositionP);
                if(dist_treas < 150 || dist_play < 150 || (dist_play <150 && dist_treas < 150))
                    checkPosition = true;
            }
        }
    }

    //Choose which gem to show
    public static GameObject ChooseGem(GameObject[] gems)
    {
        int i = Random.Range(0, gems.Length);
        foreach(GameObject g in gems)
        {
            if (g == gems[i])
                g.SetActive(true);
            else
                g.SetActive(false);
        }
        RndPositionObj(gems[i]);
        return gems[i];
    }

    //Randomize the environment
    public static void RndEnvironment()
    {
        //Checks the experimental status and ends it if necessary 
        CheckExpStatus();
        int env;
        if (current_trial <= 20)
        {
            env = 1;
        }
        else if (current_trial > 20 && current_trial <= 40)
        {
            env = 2;
        }
        else
        {
            env = 3;
            changeMusic = 3;
        }

        switch (env)
        {
            case 1:
                if(env1<20)
                {
                    if(env1<8)
                    {
                        current_gem_number = 1;
                    }
                    else
                    {
                        current_gem_number = 2;
                    }
                    env1++;
                    Debug.Log(env1);
                    changeMusic = 1;
                    Application.LoadLevel(level1);
                }
                else
                {
                    endBlock = true;
                    //changeSky = true;
                    current_gem_number = 1;
                    changeMusic = 2;
                    Application.LoadLevel(level2);
                }
                break;
            case 2:
                if (env2 < 20)
                {
                    if (env2 < 8)
                    {
                        current_gem_number = 1;
                    }
                    else
                    {
                        current_gem_number = 2;
                    }
                    env2++;
                    Debug.Log(env2);   
                    Application.LoadLevel(level2);
                }
                else
                {
                    endBlock = true;
                    current_gem_number = 1;
                    Application.LoadLevel(level3);
                }
                break;
            case 3:
                if (env3 < 20)
                {
                    if (env3 < 8)
                    {
                        current_gem_number = 1;
                    }
                    else
                    {
                        current_gem_number = 2;
                    }
                    env3++;
                    Debug.Log(env3);
                    Application.LoadLevel(level3);
                }
                else
                {
                    Application.LoadLevel(6); //End Scene
                }
                break;
        }
    }


    public static void CheckExpStatus()
    {
        if(current_trial == tot_trials)
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

        if (SystemInfo.operatingSystem.Contains("Windows"))
        {
            //Allows input from Xbox360 controller, generic controller and Mac users
            if (Input.GetButton("A"))
            {
                check = true;
            }
        }
        else
        {
            if(Input.GetButton("MacX"))
            {
                check = true;
            }
        }
        return check;
    }

    //Checks if player pressed pause
    public static bool isGamePaused()
    {
        bool check = false;
        if (SystemInfo.operatingSystem.Contains("Windows"))
        {
            //Allows input from Xbox360 controller, generic controller and Mac users
            if (Input.GetButton("BackButton"))
            {
                check = true;
            }
        }
        else
        {
            if (Input.GetButton("BackButtonMac"))
            {
                check = true;
            }
        }
        return check;
    }

    public static int SetSkyIndex(int oldIndex)
    {
        Debug.Log(oldIndex);
        int result;
        if(oldIndex == -1)
        {
            result = Random.Range(0, 3);
            skyIndex = result;
            alreadyUsedSky.Add(result);
        }
        else
        {
            result = oldIndex;
            do
            {
                result = Random.Range(0, 3);
            } while (oldIndex == result || alreadyUsedSky.Contains(result));
            skyIndex = result;
        }
        Debug.Log("Sky index: " + result);
        return result;
    }

    public static Vector3 RandomizingGameObject(GameObject obj)
    {
        Vector3 tempPos = new Vector3();
        bool continueRandomization = true;
        int ud = Random.Range(0, 2);
        float x, z;

        if(obj.tag == "Player")
        {
            treasure = GameObject.FindGameObjectWithTag("Treasure");
            //If it's the first trial
            if (old_PT_pos == null)
            {
                if(ud == 0)
                {
                    z = Random.Range(600f, 656f);
                }
                else
                {
                    z = Random.Range(745f, 801f);
                }
                //Player new position
                tempPos = new Vector3(Random.Range(400f, 701f), obj.transform.position.y, z);
                obj.transform.position = tempPos;
                //Update treasure position
                treasure.transform.position = new Vector3(tempPos.x + X, treasure.transform.position.y, tempPos.z + Z);
                //Saves this last position for further checks
                old_PT_pos = tempPos;
            }
            //If we already randomize player and treasure pos
            else
            {
                while(continueRandomization)
                {
                    if (ud == 0)
                    {
                        z = Random.Range(600f, 656f);
                    }
                    else
                    {
                        z = Random.Range(745f, 801f);
                    }
                    //Player new position
                    tempPos = new Vector3(Random.Range(400f, 701f), obj.transform.position.y, z);
                    if (Vector3.Distance(tempPos, old_PT_pos) > 100f)
                    {
                        old_PT_pos = tempPos;
                        obj.transform.position = tempPos;
                        //Update treasure position
                        if (treasure != null)
                        {
                            treasure.transform.position = new Vector3(tempPos.x + X, treasure.transform.position.y, tempPos.z + Z);
                        }
                        continueRandomization = false;
                    }
                }
            }
        }
        else if (obj.tag == "Gems")
        {
            if(old_gem_pos == null)
            {
                if (ud == 0)
                {
                    z = Random.Range(600f, 656f);
                }
                else
                {
                    z = Random.Range(745f, 801f);
                }
                //Gem new position
                tempPos = new Vector3(Random.Range(400f, 701f), obj.transform.position.y, z);
                obj.transform.position = tempPos;
                //Saves this last position for further checks
                old_PT_pos = tempPos;
            }
            else
            {
                while(continueRandomization)
                {
                    if (ud == 0)
                    {
                        z = Random.Range(600f, 656f);
                    }
                    else
                    {
                        z = Random.Range(745f, 801f);
                    }
                    //Gem new position
                    tempPos = new Vector3(Random.Range(400f, 701f), obj.transform.position.y, z);
                    if(Vector3.Distance(tempPos,old_gem_pos) > 100f && Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position,tempPos) > 100f)
                    {
                        old_gem_pos = tempPos;
                        obj.transform.position = tempPos;
                        continueRandomization = false;
                    }
                }
            }
        }
        return tempPos;
    }

    public static GameObject RandomizeGem(GameObject[] gems)
    {
        GameObject choosenGem;
        int i;
        do
        {
            i = Random.Range(0, gems.Length);
        } while (i == old_gem_index);
        choosenGem = gems[i];
        old_gem_index = i;
        foreach (GameObject g in gems)
        {
            if (g == gems[i])
                g.SetActive(true);
            else
                g.SetActive(false);
        }
        if(!isShorterVersion)
            RndPositionObj(choosenGem);

        return choosenGem;
    }

    public static Vector3 MappingRandomize(GameObject player)
    {
        Vector3 tempPos = new Vector3();
        Vector3 prev_player_pos;
        bool continueRandomization = true;
        int ud = Random.Range(0, 2);
        float x, z;

        //Saves actual position
        prev_player_pos = player.transform.position;
        //Randomize position
        while (continueRandomization)
        {
            if (ud == 0)
            {
                z = Random.Range(600f, 656f);
            }
            else
            {
                z = Random.Range(745f, 801f);
            }
            //Player new position
            tempPos = new Vector3(Random.Range(400f, 701f), player.transform.position.y, z);
            if (Vector3.Distance(tempPos, old_PT_pos) > 100f && Vector3.Distance(tempPos, prev_player_pos) > 100f)
            {
                old_PT_pos = tempPos;
                player.transform.position = tempPos;
                continueRandomization = false;
            }
        }
        return tempPos;
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
            if (PlayerPrefs.GetInt("isMapping") == 0)
            {
                Demo._FileName = PlayerPrefs.GetString("SubjID") + "_MainTrialInfo" + current_trial + ".xml";
            }
            else
            {
                Demo._FileName = PlayerPrefs.GetString("SubjID") + "_MainTrialInfo_Mapping" + current_trial + ".xml";
            }

        }
        else if (pObject.GetType() == typeof(List<CompletePath.PathMapping>))
        {
            xs = new XmlSerializer(typeof(List<CompletePath.PathMapping>));
            if (PlayerPrefs.GetInt("isMapping") == 0)
            {
                Demo._FileName = PlayerPrefs.GetString("SubjID") + "_CompleteTrialPath" + current_trial + ".xml";
            }
            else
            {
                Demo._FileName = PlayerPrefs.GetString("SubjID") + "_CompleteTrialPath_Mapping" + current_trial + ".xml";
            }
        }
        else
        {
            xs = new XmlSerializer(typeof(List<DistanceAndFeedBack.FeedbackInfo>));
            if (PlayerPrefs.GetInt("isMapping") == 0)
            {
                Demo._FileName = PlayerPrefs.GetString("SubjID") + "_FeedbackTrials" + ".xml";
            }
            else
            {
                Demo._FileName = PlayerPrefs.GetString("SubjID") + "_FeedbackTrials_Mapping" + ".xml";
            }
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


