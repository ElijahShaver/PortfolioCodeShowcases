using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public PlayerMovement playerMovement;

    public bool canTalk;
    public bool hasStarted;

    public bool activateDeactivate;

    // hey! if you're looking inside the code, you'll see that i streamlined the dialogue trigger in its entirety! this was to make developing the game's dialogue system much, much easier.
    
    // this controls whether an object is activated/deactivated before or after dialogue is finished playing.
    // if triggerOnStart is true, the object is triggered upon interacting with the dialogue trigger, and vice-versa
    [Header("Activate/Deactivate object")]
    public List<GameObject> objectsToActivate;
    public List<GameObject> objectsToDestroy;
    public bool triggerOnStart;

    // this controls whether the dialogue is played upon the dialogue trigger loading in
    // the timeToStart float dictates, well, the amount of time before the dialogue starts
    [Header("Start on load?")]
    public bool startOnLoad;
    public float timeToStart;

    // this controls other misc options
    // if activateOnTouch is true, if the player enters the dialogue trigger's hitbox, the dialogue starts playing automatically
    // while dialogue is playing, if canStopPlayer is set to true, the dialogue box stops the player in their tracks
    // if doesTriggerObject is set to true, then the dialogue trigger gets the ability to activate/deactivate an object on start or end
    // freezeOnStart is unused, but i think it was originally used to freeze the player upon the level starting. however, the canStopPlayer boolean pretty much does the same thing.
    // the doActivDeactiv boolean, if set to true, allows the dialogue trigger to activate and/or deactivate objects. it does the same thing as the triggerOnStart boolean, but it's more straightforward.
    // the destroySelf boolean, if set to true, destroys the dialogue trigger upon it ending.
    [Header("Other controls")]
    public bool activateOnTouch;
    public bool canStopPlayer;
    public bool doesTriggerObject;
    public bool freezeOnStart;
    public bool doActivDeactiv;
    public bool destroySelf;

    // this boolean determines whether or not the dialogue already was played. this is used to prevent the player from going into a loop with dialogue, softlocking them.
    bool alreadyPlayed;

    public GameObject notif;

    public TextAsset inkJSON;

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // if startOnLoad is set to true, the WaitForDialogue coroutine is done, and if the dialogueManager's current Ink file isn't null, it does the dialogue.
        if (startOnLoad)
        {
            StartCoroutine(WaitForDialogue());

            if (dialogueManager.currentStory != null)
            {
                // pressing E at the last line of dialogue, while the doesTriggerObject boolean is true and the triggerOnStart boolean is false, it activates/deactivates GameObjects.
                if (Input.GetKeyDown(KeyCode.E) && !dialogueManager.currentStory.canContinue && hasStarted)
                {
                    if (doesTriggerObject && !triggerOnStart)
                    {
                        activateDeactivate = true;
                    }

                    Debug.Log("do this thing");

                    //Destroy(this.gameObject);
                }
            }
        }

        else
        {
            // if the player is in range of the dialogue trigger and it isn't playing, and if activateOnTouch is false, it gives the player a notif telling them to press E to start the dialogue.
            if (canTalk && !dialogueManager.isPlaying)
            {
                if (!activateOnTouch)
                {
                    notif.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.E) && !hasStarted)
                    {
                        notif.SetActive(false);
                        dialogueManager.EnterDialogueMode(inkJSON);
                        hasStarted = true;

                        // if both of these booleans are true, it activates/deactivates GameObjects.
                        if (doesTriggerObject && triggerOnStart)
                        {
                            activateDeactivate = true;
                        }
                    }

                    // pressing E at the last line of dialogue, while the doesTriggerObject boolean is true and the triggerOnStart boolean is false, it activates/deactivates GameObjects.
                    if (Input.GetKeyDown(KeyCode.E) && !dialogueManager.currentStory.canContinue)
                    {
                        if (doesTriggerObject && !triggerOnStart)
                        {
                            activateDeactivate = true;
                        }

                        hasStarted = false;
                    }
                }

                // if activateOnTouch is true, walking into the dialogue trigger automatically plays dialogue.
                else
                {
                    if (!alreadyPlayed)
                    {
                        if (doesTriggerObject && !triggerOnStart)
                        {
                            activateDeactivate = true;
                        }

                        if (doActivDeactiv)
                        {
                            activateDeactivate = true;
                        }

                        notif.SetActive(false);
                        dialogueManager.EnterDialogueMode(inkJSON);

                        alreadyPlayed = true;

                    }
                }
            }

            else
            {
                notif.SetActive(false);
            }
        }

        if (activateDeactivate && !dialogueManager.dialoguePanel.activeInHierarchy)
        {
            ActivateDeactivate();
        }

        // controls how the player is able to move (sprinting, walking, etc.) when they encounter a dialogue trigger.
        switch (dialogueManager.isPlaying)
        {
            case true:
                // if canStopPlayer is true and dialogue is playing, the player stops and cannot move.
                if (canStopPlayer)
                {
                    if (Input.GetButton("Sprint"))
                    {
                        playerMovement.characterSpeed = 0;
                    }
                    else
                    {
                        playerMovement.characterSpeed = 0;
                    }

                    playerMovement.rb.constraints = RigidbodyConstraints.FreezeAll;
                }

                else
                {
                    if (Input.GetButton("Sprint"))
                    {
                        playerMovement.characterSpeed = playerMovement.runSpeed;
                    }
                    else
                    {
                        playerMovement.characterSpeed = playerMovement.walkSpeed;
                    }

                    playerMovement.rb.constraints = RigidbodyConstraints.None;
                    playerMovement.rb.constraints = RigidbodyConstraints.FreezeRotation;
                }
                break;
            // if dialogue isn't playing, the player can move around like normal.
            case false:
                if (canStopPlayer)
                {
                    if (Input.GetButton("Sprint"))
                    {
                        playerMovement.characterSpeed = playerMovement.runSpeed;
                    }
                    else
                    {
                        playerMovement.characterSpeed = playerMovement.walkSpeed;
                    }

                    playerMovement.rb.constraints = RigidbodyConstraints.None;
                    playerMovement.rb.constraints = RigidbodyConstraints.FreezeRotation;
                }

                else
                {
                    if (Input.GetButton("Sprint"))
                    {
                        playerMovement.characterSpeed = playerMovement.runSpeed;
                    }
                    else
                    {
                        playerMovement.characterSpeed = playerMovement.walkSpeed;
                    }

                    playerMovement.rb.constraints = RigidbodyConstraints.None;
                    playerMovement.rb.constraints = RigidbodyConstraints.FreezeRotation;
                }
                break;
        }
    }

    // enters dialogue mode via the dialogueManager after the given amount of time.
    public IEnumerator WaitForDialogue()
    {
        yield return new WaitForSeconds(timeToStart);

        if (!hasStarted)
        {
            //notif.SetActive(false);
            dialogueManager.EnterDialogueMode(inkJSON);
            hasStarted = true;
        }
    }

    // activates and/or deactivates the GameObjects inside the objectsToActivate and objectsToDestroy lists.
    public void ActivateDeactivate()
    {
        for (int i = 0; i < objectsToActivate.Count; i++)
        {
            Debug.Log("do this activate");
            objectsToActivate[i].SetActive(true);
        }

        for (int i = 0; i < objectsToDestroy.Count; i++)
        {
            Debug.Log("do this destroy");
            objectsToDestroy[i].SetActive(false);
        }

        if (destroySelf)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            canTalk = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canTalk = false;
            alreadyPlayed = false;
        }
    }
}
