using UnityEngine;
using System.Collections;

public class MenuStart : MonoBehaviour
{
    public GameObject welcomeText, instructionText, goodLuckText, next, start;
    int count = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        NextButtonPressed();
    }

    void NextButtonPressed()
    {
        if (Input.GetButton("A"))
        {
            switch (count)
            {
                case 0:
                    welcomeText.SetActive(false);
                    instructionText.SetActive(true);
                    count++;
                    break;
                case 1:
                    instructionText.SetActive(false);
                    goodLuckText.SetActive(true);
                    next.SetActive(false);
                    start.SetActive(true);
                    break;
            }
        }
    }

    /*
    public IEnumerator WaitForButtonPressed()
    {
        if (Input.GetButton("A"))
        {
            yield return 0;
        }
    }*/
}
