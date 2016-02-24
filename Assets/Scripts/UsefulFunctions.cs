using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Linq;

public class UsefulFunctions : MonoBehaviour {
    public static GameObject[] gems,totems,invisibleBorders,islandItems,palms,woods,waves;
    public static List<Vector3> envItems = new List<Vector3>();
    public static List<int> levels = new List<int> {1,2,3}; //up to 5 if we add two more levels
    public static int tot_trials = 90;
    public static int current_level,current_trial,tot_env1, tot_env2, tot_env3, nobj1, nobj2, nobj3, old_env = 0;


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
            if (obj.tag == "Player")
            {
                Vector3 oldTreasurePosition = treasure.transform.position;
                envItems.Add(GemsCollection.collectObj.transform.position);
            }
            else
            {
                envItems.Add(GameObject.FindGameObjectWithTag("Player").transform.position);
            }

            //Checks if the position is free
            Vector3 newPositionP = new Vector3(x, oldPosition.y, z);
            if ((totems == null && islandItems == null && palms == null && woods == null) || (!envItems.Contains(newPositionP)))
            {
                obj.transform.position = newPositionP;
                if(obj.tag == "Player")
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

    public static int RndEnvironment()
    {
        if(levels.Count != 0)
        {
            int l = Random.Range(0, levels.Count);//to 4 if we consider 2 more environments
            switch (l)
            {
                //Level 1: Distal cues only
                case 0:
                    //Application.LoadLevel(2);
                    tot_env1 = 1;
                    current_level = 1;
                    break;
                //Level 2: Distal cues & Boundaries
                case 1:
                    tot_env2 = 1;
                    current_level = 2;
                    break;
                //Level 3: Distal cues & Local Landmarks
                case 2:
                    tot_env3 = 1;
                    current_level = 3;
                    break;
                /*
                //Level 4: Distal cues + Local Landmarks + Boundaries
                case 3:
                    current_level = 4;
                    break;
                //Level 5: Empty
                case 4:
                    current_level = 5;
                    break;
                */
            }
            levels.RemoveAt(l);
            return l;
        }
        else
        {
            return 0;
        }
    }


    public static void RndObjNum()
    {
        switch (current_level)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            /*
            case 4:
                break;
            case 5:
                break;
            */
        }
        
    }

    public static void Feedback()
    {
        GameObject.FindGameObjectWithTag("Player").SetActive(false);
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
        else {
            xs = new XmlSerializer(typeof(List<CompletePath.PathMapping>));
            GemsCollection._FileName = PlayerPrefs.GetString("SubjID") + "_CompleteTrialPath" + current_trial + ".xml";
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

*/
