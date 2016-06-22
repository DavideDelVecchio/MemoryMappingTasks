using UnityEngine;
using System.Collections;

public class ShorterFunctions : MonoBehaviour {
    public GameObject[] randomPositions;
    public static GameObject[] rp;
    public static Quaternion rotValuePlayer;
    public static int oldPosition = -1; 

    int[] choosenRotation = new int[] { 0, 90, 180, 270 };

    void Awake()
    {
        rp = randomPositions;
        Debug.Log(rp.Length);
    }


    public static void RandomizeGemPosition(GameObject gem)
    {
        Vector3 position;
        int newPosition = -2;

        newPosition = Random.Range(0, rp.Length);
        while(oldPosition == newPosition)
        {
            newPosition = Random.Range(0, rp.Length);
        }
        oldPosition = newPosition;
        position = rp[newPosition].transform.position;

        gem.transform.position = position;

         
    }

    public static void SetPlayerRotation(int index)
    {
        //Facing A
        if(index == 4 || index == 8 || index == 11)
        {
            rotValuePlayer = new Quaternion(0f,0f,0f,0);
        }
        //Facing B
        else if(index == 5 || index == 9 || index == 10)
        {
            rotValuePlayer = new Quaternion(0f, 90f, 0f, 0);
        }
        //Facing C
        else if (index == 0 || index == 1 || index == 7)
        {
            rotValuePlayer = new Quaternion(0f, 180f, 0f, 0);
        }
        //Facing D
        else if(index == 2 || index == 3 || index == 6)
        {
            rotValuePlayer = new Quaternion(0f, 270f, 0f, 0);
        }

    }

}
