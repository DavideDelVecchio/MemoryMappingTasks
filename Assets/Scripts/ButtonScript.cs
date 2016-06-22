using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;

public class ButtonScript : MonoBehaviour {
    public static int pageIndex = 0;
    public int dummy = 0;
    public GameObject menu; //page0,page1, page2,page3,page4,page5,page6,page7,next,start,
    public GameObject[] pages;
    public string _FileLocation, _data;
    public bool hasStartedInstructions = false;
    bool isSaving;

    void Start()
    {
        if (!hasStartedInstructions)
        {
            hasStartedInstructions = true; //we want to make sure we only start the instruction coroutine *once*
            StartCoroutine(DisplayInstructions()); //starts the instruction coroutine!
        }
    }

    //DisplayInstructions coroutine!
    public IEnumerator DisplayInstructions()
    {
        yield return StartCoroutine(WaitForActionButton());
        if (pageIndex == 0)
        {
            pages[pageIndex].SetActive(false);

            while (pageIndex < 7)
            {
                pages[pageIndex].SetActive(false);
                pages[pageIndex + 1].SetActive(true);
                pageIndex++;
                yield return StartCoroutine(WaitForActionButton());
            }
            if(pageIndex == 7)
            {
                menu.SetActive(false);
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<CharacterController>().enabled = true;
                player.GetComponent<MouseLook>().enabled = true;
                GameObject treasure = GameObject.FindGameObjectWithTag("Treasure");
                treasure.GetComponent<Animator>().enabled = true;
            }
        }
    }

    //WaitForActionButton coroutine!
    public IEnumerator WaitForActionButton()
    {
        bool hasPressedButton = false;
        while (Input.GetAxis("A") != 0f)
        { //wait for the button to be released if it was pushed down
            yield return 0; //waits a frame each time until button is released so that we don't get stuck
        }
        while (!hasPressedButton)
        { //now wait for the button to be pushed again
            if (Input.GetAxis("A") == 1.0f )
            {
                hasPressedButton = true;
            }
            yield return 0; //waits a frame so that we don't get stuck
        }
        //once we reach the end of the function, the button has been successfully pressed!
    }

    public void ContinueExp()
    {
        UsefulFunctions.RndEnvironment();
    }

    public void QuitExp()
    {
        Application.LoadLevel(6);
    }


}
