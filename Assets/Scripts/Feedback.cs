using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Feedback : MonoBehaviour {

    public GameObject loadingBarR, loadingBarY, loadingBarG;
    public Text textIndicator, percentage;
    public float currentAmount, speed, error;

	// Update is called once per frame
	void Update () {
        if (error <= 0.25)
        {
            loadingBarY.SetActive(false);
            loadingBarG.SetActive(false);
            FillLoadingBar();
            //Red loading
            loadingBarR.GetComponent<Image>().fillAmount = currentAmount / 100;
        }
        else if (error <= 0.75)
        {
            loadingBarR.SetActive(false);
            loadingBarG.SetActive(false);
            FillLoadingBar();
            //Yellow loading
            loadingBarY.GetComponent<Image>().fillAmount = currentAmount / 100;
        }
        else if (error > 0.75 && error <= 100)
        {
            loadingBarY.SetActive(false);
            loadingBarR.SetActive(false);
            FillLoadingBar();
            //Green loading
            loadingBarG.GetComponent<Image>().fillAmount = currentAmount / 100;
        }

	}

    void FillLoadingBar() {
        if (currentAmount<error)
        {
            //Red loading
            currentAmount += speed * Time.deltaTime;
            percentage.text = ((int)currentAmount).ToString() + "%";
        }
        else
        {
            textIndicator.text = "DONE!";
        }
    }
}
