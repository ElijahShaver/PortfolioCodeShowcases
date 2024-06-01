using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public PlayerFollowTruck playerFollowTruck;
    public PlayerMovement playerMovement;
    public GameplayCountdown gameplayCountdown;

    public TMP_Text totalScoreText;
    public TMP_Text airTimeText;
    public TMP_Text timeText;
    public TMP_Text timeScoreText;

    public float airTime;
    public float timeBonus;
    public float winBonus;

    public float totalScore;
    public bool shouldCountTotalScore;
    public bool shouldCountTime;
    public bool shouldCountAirTime;

    bool findTextThing;

    float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerFollowTruck = GameObject.FindWithTag("Player").GetComponent<PlayerFollowTruck>();
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        gameplayCountdown = GameObject.FindWithTag("Player").GetComponent<GameplayCountdown>();
        airTimeText = GameObject.Find("airtimetext").GetComponent<TMP_Text>();
        timeText = GameObject.Find("timetext").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.WinScreen.activeInHierarchy && !findTextThing)
        {
            totalScoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
            timeScoreText = GameObject.Find("TimeScoreText").GetComponent<TMP_Text>();
            Debug.Log("cant find score text for some stupid reason so time to find the thing i guess...");
            findTextThing = true;
        }

        // each level has a set time bonus. if you complete the level faster, you get more points!
        if (shouldCountTime && gameplayCountdown.hasStarted)
        {
            if (!shouldCountTotalScore)
            {
                time += Time.deltaTime;

                airTimeText.text = "AIRTIME: " + Mathf.Round(airTime);
                timeText.text = "TIME: " + time.ToString("F2") + "s";
            }

            if (timeBonus > 0)
            {
                timeBonus -= Time.deltaTime;
            }
        }

        else if (timeBonus <= 0)
        {
            timeBonus = 0;
        }

        // if you're in the air, you get more points!
        if (playerFollowTruck.shouldGetAirTime)
        {
            switch (playerFollowTruck.isOnTruck)
            {
                case true:
                    break;
                case false:
                    if (shouldCountAirTime && !playerMovement.isDead)
                    {
                        airTime += (Time.deltaTime * 50);
                    }
                    break;
            }
        }

        // this calculates the total score and completion time upon completing the level, and displays it on the completion screen.
        if (shouldCountTotalScore && totalScoreText != null)
        {
            airTimeText.gameObject.SetActive(false);
            timeText.gameObject.SetActive(false);
            totalScore = Mathf.Round(airTime) + Mathf.Round(timeBonus) + Mathf.Round(winBonus);
            totalScoreText.text = "AIRTIME: " + Mathf.Round(airTime) + "\nTIME BONUS: " + Mathf.Round(timeBonus) + "\nCOMPLETION BONUS: " + Mathf.Round(winBonus) + "\nTOTAL SCORE: " + totalScore;
            timeScoreText.text = "TIME: " + time.ToString("F2") + "s";
        }
    }
}
