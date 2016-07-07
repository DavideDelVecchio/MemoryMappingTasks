using UnityEngine;
using System.Collections;

public class ShorterFunctions : MonoBehaviour {
    public GameObject[] randomPositions;
    public static GameObject[] rp;

    public static Quaternion rotValuePlayer;
    public static Vector3 rotVectorPlayer;

    public static int tot_trials = 10;
    public static int oldPosition = -1;
    public static int levelCount = 1;
    public static int levelID = -1;

    public static bool isSaving, reloadDemo;

    public static string geometry, distal_geometry;

    public static MainTrialInfo.InfoTrial info;// = new SubjTrialInfo.InfoTrial();
    public static CompletePath.PathMapping objPath;// = new PlayerInfo.PathMapping();


    int[] choosenRotation = new int[] { 0, 90, 180, 270 };

    void Awake()
    {
        rp = randomPositions;
    }


    public static void RandomizeGemPosition(GameObject gem, GameObject[] pos)
    {
        Vector3 position;
        int newPosition = -2;

        if (pos.Length == 0)
            Debug.Log("Random positions are empty!!");

        newPosition = Random.Range(0, pos.Length);
        while(oldPosition == newPosition)
        {
            newPosition = Random.Range(0, rp.Length);
        }
        oldPosition = newPosition;
        position = pos[newPosition].transform.position;

        gem.transform.position = new Vector3(position.x,gem.transform.position.y,position.z);
        SetPlayerRotation(oldPosition);
         
    }

    public static void SetPlayerRotation(int index)
    {
        //Facing A
        if(index == 4 || index == 8 || index == 11)
        {
            rotValuePlayer = new Quaternion(0f,0f,0f,1f);
            rotVectorPlayer = new Vector3(0f, 0f, 0f);
        }
        //Facing B
        else if(index == 5 || index == 9 || index == 10)
        {
            rotValuePlayer = new Quaternion(0f, 90f, 0f, 1f);
            rotVectorPlayer = new Vector3(0f, 90f, 0f);
        }
        //Facing C
        else if (index == 0 || index == 1 || index == 7)
        {
            rotValuePlayer = new Quaternion(0f, 180f, 0f, 1f);
            rotVectorPlayer = new Vector3(0f, 180f, 0f);
        }
        //Facing D
        else if(index == 2 || index == 3 || index == 6)
        {
            rotValuePlayer = new Quaternion(0f, 270f, 0f, 1f);
            rotVectorPlayer = new Vector3(0f, 270f, 0f);
        }
    }

    public static bool ExpStatus()
    {
        bool quitApp = false;

        if (levelCount > 10)
        {
            isSaving = true;
            quitApp = true;
            string _data, _FileLocation;
            _FileLocation = Application.dataPath + "/SubTrialInfo";
            //End experiment
            try
            {
                _data = UsefulFunctions.SerializeObject(DemoShorter.trialFeedback);
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
            Debug.Log(quitApp);
        }
        Debug.Log(quitApp);
        return quitApp;
    }

    public static void SaveMainInfo(GameObject obj)
    {
        if ((obj.tag == "Player" && DemoShorter.collision_t == 0))
        {
            info = new MainTrialInfo.InfoTrial();
            info.ID = "Start Position Player";
            info.s_x = obj.transform.position.x;
            info.s_y = obj.transform.position.y;
            info.s_z = obj.transform.position.z;
            info.r_x = obj.transform.rotation.x;
            info.r_y = obj.transform.rotation.y;
            info.r_z = obj.transform.rotation.z;
            info.time = DemoShorter.trial_t;
            info.score = PlayerPrefs.GetString("Score");
            DemoShorter.trialInfo.Add(info);
        }
        else if (obj.tag == "Gems")
        {
            info = new MainTrialInfo.InfoTrial();
            info.ID = "Gem";
            info.s_x = obj.transform.position.x;
            info.s_y = obj.transform.position.y;
            info.s_z = obj.transform.position.z;
            info.time = DemoShorter.trial_t;
            info.score = PlayerPrefs.GetString("Score");
            DemoShorter.trialInfo.Add(info);
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
            info.time = DemoShorter.trial_t;
            info.score = PlayerPrefs.GetString("Score");
            info.d = Vector3.Distance(DemoShorter.goal_position, DemoShorter.last_position);
            DemoShorter.trialInfo.Add(info);
        }
    }
}
