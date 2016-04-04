using UnityEngine;
using System.Collections;
using System;

public class TornadoScript : MonoBehaviour {

    public GameObject player,tornado,tornadoFeedback;
    public GameObject [] twisters;

    public static TornadoScript singleton;

    void Awake()
    {
        singleton = this;
    }

    public void TornadoSand(bool b)
    {
        if (b)
        {
            tornado.transform.position = player.transform.position;
            player.GetComponent<CharacterController>().enabled = false;
            twisters[0].SetActive(true);
            twisters[1].SetActive(true);
            tornadoFeedback.SetActive(true);
            UsefulFunctions.RndPositionObj(player);
        }
        else
        {
            player.GetComponent<CharacterController>().enabled = true;
            for (int i = 0; i < tornado.transform.childCount; i++)
            {
                GameObject child = tornado.transform.GetChild(i).gameObject;
                child.SetActive(false);
                tornadoFeedback.SetActive(false);
            }

        }
    }
}
