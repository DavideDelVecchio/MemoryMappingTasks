using UnityEngine;
using System.Collections;

public class TestRandomPos : MonoBehaviour {

    public Vector3 initialPos,lastPosition;
    public bool continueRandomizing = true;
    public float distance;


    void Awake()
    {
        //initialPos = this.transform.position;
        //Debug.Log("Posizione iniziale: " + initialPos);
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        //while (continueRandomizing)
        //{
        //    lastPosition = new Vector3(Random.Range(405f, 696f), initialPos.y, Random.Range(595f, 907f));
        //    distance = Vector3.Distance(lastPosition, initialPos);
        //    if (distance > 100)
        //    {
        //        continueRandomizing = false;
        //        this.transform.position = lastPosition;
        //        Debug.Log(distance);
        //    }
        //}
        //continueRandomizing = true;
        this.transform.position = ShorterFunctions.RandomizeGem();
	}


}
