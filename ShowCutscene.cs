using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// note: this was unfortunately cut from the final game due to time constraints, but it works perfectly when utilized!
// a version of this code was also supposed to be used in the prologue, and technically that version is in the final game, but i didn't have the time to create the art assets needed to make it work, so there's only a stagnant image throughout it.
public class ShowCutscene : MonoBehaviour
{
    public GameObject[] cutsceneToShow;

    public SpriteFade spriteFade;

    public int numberOfCutscenes = 0;
    public int sceneToLoad;

    public bool isTouched = false;
    public bool canContinue = false;
    public bool cutsceneStarted = true;
    public bool onLastCutscene = false;
    public bool isPressed = false;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteFade.shouldFadeIn = false;

        spriteFade.shouldFadeOut = true;

        Invoke("Continue", 0.6f);

        // all images (cutscenes) are set to false to begin with.
        for (int i = 0; i < cutsceneToShow.Length; i++)
        {
            cutsceneToShow[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // determines whether the Z or enter keys are pressed so it can continue to the next image.
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            isPressed = true;
        }

        else if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            isPressed = false;
        }

        // this entire this in a whole process. if the cutscenes you're on is a lesser number than the amount of cutscenes that the array has, then you can still go on.
        // if not, and you're on the last cutscene, you get send to a different scene.
        if (isPressed && canContinue == true && (numberOfCutscenes < cutsceneToShow.Length - 1))
        {
            Invoke("NextCutscene", 0.0f);

            Invoke("AddCutscene", 0.6f);

            Invoke("FadeOut", 0.7f);

            Invoke("Continue", 2.3f);
        }

        else if (isPressed && canContinue == true && (numberOfCutscenes == cutsceneToShow.Length - 1))
        {
            onLastCutscene = true;
            
            spriteFade.shouldFadeIn = true;

            spriteFade.shouldFadeOut = false;

            Invoke("ChangeScene", 0.5f);
        }

        // this activates the next cutscene.
        // in retrospect, putting this inside the update function wasn't the best idea and i could have made a much more efficient system, but i made this code back in late 2022, and this was one of my first major projects in Unity.
        for (int i = 0; i < cutsceneToShow.Length; i++)
        {
            cutsceneToShow[numberOfCutscenes].SetActive(true);
        }
    }
    
    // this makes a black screen fade in, giving this script the chance to change to the next cutscene while the screen is dark.
    void NextCutscene()
    {
        canContinue = false;
        
        spriteFade.shouldFadeIn = true;

        spriteFade.shouldFadeOut = false;
    }

    
    // this adds the next cutscene. it makes the numberOfCutscenes integer increase by 1, thus making the irresponsibly placed for loop in the update function to set that cutscene active.
    void AddCutscene()
    {
        numberOfCutscenes++;

        Debug.Log("you are on cutscene " + numberOfCutscenes);
    }

    // this changes the scene. pretty self-explanatory.
    void ChangeScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    // makes the player able to continue to the next cutscene.
    void Continue()
    {
        canContinue = true;
    }

    // these both fade into and out of a black screen.
    void FadeOut()
    {
        spriteFade.shouldFadeIn = false;

        spriteFade.shouldFadeOut = true;
    }

    void FadeIn()
    {
        canContinue = false;

        spriteFade.shouldFadeIn = true;

        spriteFade.shouldFadeOut = true;
    }
}
