using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CompletionManager : MonoBehaviour
{
    public List<GameObject> binList;

    public GameObject transitionNextLevel, transitionSameLevel;

    public float setTime, timeLeft;

    public bool hasStarted, alreadyCompleted;

    public TMP_Text statusText;

    public int numberOfBins, completedBins;

    // Start is called before the first frame update
    void Start()
    {
        numberOfBins = binList.Count;
        timeLeft = setTime;
        hasStarted = false;
        statusText.text = "Game starts in\n5 seconds!";
        Invoke("StartThing", 5);
    }

    // Update is called once per frame
    void Update()
    {
        string binCount = "";

        if (!alreadyCompleted)
        {
            // if the amoung of completed bins is equal to the total number of bins (in other words, if all bins are properly filled), then the next level begins in 5 seconds.
            if (completedBins == numberOfBins)
            {
                Invoke("LoadNext", 5);
                hasStarted = false;
                alreadyCompleted = true;
            }

            // determines the binCount string; it shows how many bins have been completed out of the total number.
            else
            {
                binCount = "Bins Filled: " + completedBins + " / " + numberOfBins;
            }
        }

        // calculates the amount of time left.
        if (hasStarted && (timeLeft > 0.00f))
        {
            timeLeft -= Time.deltaTime;
        }

        // if the time left reaches 0, the level resets.
        if (hasStarted && timeLeft <= 0)
        {
            transitionSameLevel.SetActive(true);
            timeLeft = 0.00f;
        }

        // TODO: make the clock show milliseconds (i wasn't able to get this finished before release, but i managed to get minutes and seconds working, which was good enough!
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60f);
        //string secondsStr = seconds.ToString("0.00");
        string time = string.Format("{0:00}:{1:00}", minutes, seconds);

        // this displays the time left and the amount of bins completed out of the total number of bins on the left hand when the status display button on the controller is held.
        // if all bins are filled, the status text says so.
        if (hasStarted)
        {
            statusText.text = "Time Left:\n" + time + "\n\n" + binCount;
        }

        if (!hasStarted && alreadyCompleted)
        {
            statusText.text = "All bins have\nbeen filled!";
        }
    }

    // for each bin that's in the current scene, the CheckBins method determines whether or not each bin is valid for completion.
    // if the bin is valid, it increased the completedBins integer by 1.
    public void CheckBins()
    {
        completedBins = 0;

        for (int i = 0; i < binList.Count; i++)
        {
            BinScript bs = binList[i].GetComponentInChildren<BinScript>();

            if (bs.shouldCount)
            {
                Debug.Log("bin " + binList[i].name + " should count");
                completedBins++;
            }

            else
            {
                Debug.Log("bin " + binList[i].name + " shouldnt count");
            }
        }
    }

    public void StartThing()
    {
        hasStarted = true;
    }

    public void LoadNext()
    {
        transitionNextLevel.SetActive(true);
    }
}
