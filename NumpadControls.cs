using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumpadControls : MonoBehaviour
{
    public NumberCode numberCode;
    public AudioSource source;
    public AudioClip press;
    public PlayerController playerControl;

    public void Number(int number)
    {
        // since there are 5 possible spots that a number can go in, if you type in a number, the howManyInts integer goes up until it reaches 5.
        // this if statement also adds the code that is set in the NumberCode script.
        if (numberCode.howManyInts < 5)
        {
            numberCode.howManyInts++;

            numberCode.numberText.text += number.ToString();
        }

        // if you input a number in 5 times, you get two outcomes:
        // if you get it correct, then you get to move on to the next part of the lab!
        // if you get it wrong, then you have to input the code again until you get it right.
        if (numberCode.howManyInts == 5 && numberCode.numberText.text != numberCode.answer)
        {
            numberCode.statusText.text = "ACCESS DENIED.";
        }

        else if (numberCode.howManyInts == 5 && numberCode.numberText.text == numberCode.answer)
        {
            numberCode.isCorrect = true;
        }
    }

    // this erases the numbers from the display, and it tells you to input the code.
    public void EraseNumbers()
    {
        numberCode.numberText.text = "";

        numberCode.howManyInts = 0;

        numberCode.statusText.text = "PLEASE INPUT CODE.";
    }

    // these two methods cause the number pad to be set inactive, and let the player move again.
    public void GoAway()
    {
        Invoke("NumPadBye", 0.2f);

        playerControl.shouldNotMove = false;
    }

    void NumPadBye()
    {
        numberCode.numberPad.SetActive(false);
    }

    // plays a beep whenever you press a button.
    public void Beep()
    {
        source.PlayOneShot(press);
    }
}
