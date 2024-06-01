using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;
using UnityEngine.InputSystem;
using Ink.Runtime;

public class NumberCode : MonoBehaviour
{
    public SimpleDisableGO simpleDGO;
    public PlayerController playerControl;
    public AudioSource source;
    public AudioClip correct;

    public GameObject numberPad;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI numberText;
    public string answer;
    public int howManyInts = 0;

    public bool isCorrect = false;

    bool audioPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        // the status text and the number text are set to these defaults when the number pad is opened.
        statusText.text = "PLEASE INPUT CODE.";
        numberText.text = "";

        numberPad.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        // these if-else statements make the player stop moving once the number pad is on screen, and vice-versa.
        if (numberPad.activeInHierarchy)
        {
            playerControl.shouldNotMove = true;
        }

        else if (!numberPad.activeInHierarchy)
        {
            playerControl.shouldNotMove = false;
        }

        // if the number you input as a code is correct, then access is granted, and you can move along!
        if (isCorrect)
        {
            statusText.text = "ACCESS GRANTED.";

            if (!audioPlayed)
            {
                source.PlayOneShot(correct);
                audioPlayed = true;
            }

            simpleDGO.destroyThisThing = true;

            Invoke("Disappear", 2.0f);
            Invoke("KeepMoving", 2.0f);
        }
    }
    
    // this makes the number pad set as inactive.
    void Disappear()
    {
        numberPad.SetActive(false);
    }

    // this makes the player regain movement.
    void KeepMoving()
    {
        playerControl.shouldNotMove = false;
    }

    // plays a fun jingle that signifies that you completed the number pad puzzle successfully.
    void YouDidItWow()
    {
        source.PlayOneShot(correct);
    }
}
