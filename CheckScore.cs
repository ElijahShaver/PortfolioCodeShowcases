using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

// while this isn't a big script, and a rather simple one, i still see it as important due to this being one of my first times experimenting with SteamVR's input system.
public class CheckScore : MonoBehaviour
{
    public SteamVR_Action_Boolean showAction, resetAction;
    public CompletionManager completionManager;
    public GameObject scoreThings, resetThings, sceneLoader;

    public float timer;

    public bool isResetting;

    bool thing = false;
    bool tick = false;
    // note: the "thing" boolean was so i could test whether or not the DisappearLol method worked.

    // Start is called before the first frame update
    void Start()
    {
        completionManager = GameObject.Find("CompletionManager").GetComponent<CompletionManager>();
        resetThings.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (completionManager.hasStarted)
        {
            // this tick boolean is set to false at the starting frame of every scene. since it's false, the status text will be set to inactive, and the tick will go to true so this if statement doesn't get called again.
            // the reason why i did this is because Unity doesn't allow you to reference a GameObject that isn't active in the hierarchy.
            if (!tick)
            {
                scoreThings.SetActive(false);
                tick = true;
            }

            // if the button to show the status text is pressed down, it shows the status text, and vice-versa. pretty simple stuff!
            if (showAction.stateDown)
            {
                Debug.Log("buttondown");
                scoreThings.SetActive(true);
            }
            else if (showAction.stateUp)
            {
                Debug.Log("buttonup");
                scoreThings.SetActive(false);
            }

            // if the button to reset the level is held down, it shows the text that the level is resetting in 3 seconds, but the countdown is cancelled when its unpressed.
            if (resetAction.stateDown)
            {
                resetThings.SetActive(true);
                isResetting = true;
            }
            else if (resetAction.stateUp)
            {
                resetThings.SetActive(false);
                isResetting = false;
            }

            // the isResetting boolean (that's set to true upon the player holding down level reset button) is responsible for letting the game know whether or not to reset the level. if the timer is at or over 3 seconds, it resets the level.
            // otherwise, the countdown resets, and the level is not reset.
            switch (isResetting)
            {
                case true:
                    timer += Time.deltaTime;

                    if (timer >= 3)
                    {
                        sceneLoader.SetActive(true);
                    }
                    break;
                case false:
                    timer = 0;
                    break;
            }
        }

        // if the completionManager says the level has been completed (by filling all needed bins correctly) it keeps the status text onscreen to tell the player that the level is done.
        if (!completionManager.hasStarted && completionManager.alreadyCompleted)
        {
            scoreThings.SetActive(true);
        }
    }

    // makes the status text disappear.
    public void DisappearLol()
    {
        scoreThings.SetActive(false);
        thing = true;
    }
}
