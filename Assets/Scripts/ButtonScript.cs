using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;

public class ButtonScript : MonoBehaviour {
    public static int pageIndex = 0;
    public int dummy = 0;
    public GameObject page0,page1, page2,page3,page4,page5,page6,next,start,menu;

    public void NextPage()
    {
        switch (pageIndex)
        {
            case 0:
                page0.SetActive(false);
                page1.SetActive(true);
                pageIndex++;
                break;
            case 1:
                page1.SetActive(false);
                page2.SetActive(true);
                pageIndex++;
                break;
            case 2:
                page2.SetActive(false);
                page3.SetActive(true);
                pageIndex++;
                break;
            case 3:
                page3.SetActive(false);
                page4.SetActive(true);
                pageIndex++;
                break;
            case 4:
                page4.SetActive(false);
                page5.SetActive(true);
                pageIndex++;
                break;
            case 5:
                page5.SetActive(false);
                page6.SetActive(true);
                next.SetActive(false);
                start.SetActive(true);
                EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(start);
                pageIndex++;
                break;
        }
    }

    public void StartDemo()
    {
        if (pageIndex == 6)
        {
            menu.SetActive(false);
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("OculusPlayer");
                player.GetComponent<OVRGamepadController>().enabled = true;
                player.GetComponent<OVRPlayerController>().enabled = true;
            }
            else
            {
                player.GetComponent<CharacterController>().enabled = true;
                player.GetComponent<MouseLook>().enabled = true;
            }
        }
        GameObject treasure = GameObject.FindGameObjectWithTag("Treasure");
        treasure.GetComponent<Animator>().enabled = true;
    }

    public void ContinueExp()
    {
        UsefulFunctions.RndEnvironment();
    }

    public void QuitExp()
    {
        Application.LoadLevel(7);
    }



}
