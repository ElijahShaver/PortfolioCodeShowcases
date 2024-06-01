using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArroController : MonoBehaviour
{
    private InputManager inputManager;
    
    Rigidbody2D rb;

    public GameObject arro;

    public GameObject upDot;
    public GameObject rightDot;
    public GameObject downDot;
    public GameObject leftDot;
    public GameObject upRightDot;
    public GameObject upLeftDot;
    public GameObject downRightDot;
    public GameObject downLeftDot;
    public GameObject centerDot;

    public bool none, up, right, down, left, upRight, upLeft, downRight, downLeft;

    public bool diagonal;

    public bool pressed;

    // this speed is only for letting the arro stop when paused.
    public float speed;

    // this speed is for the speed at which the arro moves; can be changed using the arro speed trigger.
    public float speedToChange;

    public GameObject EditorManager;
    public EditorControls editorControls;

    // Start is called before the first frame update
    void Start()
    {
        arro = GameObject.Find("arro");

        rb = GetComponent<Rigidbody2D>();

        inputManager = InputManager.instance;

        EditorManager = GameObject.Find("EditorManager");
        editorControls = EditorManager.GetComponent<EditorControls>();
    }

    // Update is called once per frame
    void Update()
    {
        // with the amount of "else if" statements in this block of code, you'd think i was just starting out with Unity...
        // anyways, this controls whether or not the arro can move or not. if it's paused, you can't move it.
        if (!editorControls.isPlaying)
        {
            speed = 0;
        }

        else if (editorControls.isPlaying)
        {
            speed = speedToChange;
        }
        
        // determines if a movement key input is pressed.
        if (inputManager.GetKey(KeybindActions.Up) == true || inputManager.GetKey(KeybindActions.Down) == true || inputManager.GetKey(KeybindActions.Right) == true || inputManager.GetKey(KeybindActions.Left) == true)
        {
            pressed = true;
        }


        // this was one of the very first things i ever implemented into arro-dodge! because of this, the code is kind of a mess... it works perfectly the way i want it to, though!
        // anyways, this is for horizontal and vertical movement.
        if (!diagonal && inputManager.GetKey(KeybindActions.Up) == true)
        {
            StopAllCoroutines();
            StartCoroutine(MoveArro(arro, upDot, speed));
            diagonal = false;

            up = true;
            none = right = down = left = upRight = upLeft = downRight = downLeft = false;
        }

        if (!diagonal && inputManager.GetKey(KeybindActions.Down) == true)
        {
            StopAllCoroutines();
            StartCoroutine(MoveArro(arro, downDot, speed));
            diagonal = false;

            down = true;
            none = up = right = left = upRight = upLeft = downRight = downLeft = false;
        }

        if (!diagonal && inputManager.GetKey(KeybindActions.Right) == true)
        {
            StopAllCoroutines();
            StartCoroutine(MoveArro(arro, rightDot, speed));
            diagonal = false;

            right = true;
            none = up = down = left = upRight = upLeft = downRight = downLeft = false;
        }

        if (!diagonal && inputManager.GetKey(KeybindActions.Left) == true)
        {
            StopAllCoroutines();
            StartCoroutine(MoveArro(arro, leftDot, speed));

            left = true;
            none = up = right = down = upRight = upLeft = downRight = downLeft = false;
        }


        // determines if the player is going diagonally.
        if ((inputManager.GetKey(KeybindActions.Up) == true && inputManager.GetKey(KeybindActions.Right) == true) || (inputManager.GetKey(KeybindActions.Up) == true && inputManager.GetKey(KeybindActions.Left) == true) || (inputManager.GetKey(KeybindActions.Down) == true && inputManager.GetKey(KeybindActions.Right) == true) || (inputManager.GetKey(KeybindActions.Down) == true && inputManager.GetKey(KeybindActions.Left) == true))
        {
            diagonal = true;
        }

        else
        {
            diagonal = false;
        }


        // this is for diagonal movement.
        if (inputManager.GetKey(KeybindActions.Up) == true && inputManager.GetKey(KeybindActions.Right) == true)
        {
            StopAllCoroutines();
            StartCoroutine(MoveArro(arro, upRightDot, speed));

            upRight = true;
            none = up = right = down = left = upLeft = downRight = downLeft = false;
        }

        if (inputManager.GetKey(KeybindActions.Up) == true && inputManager.GetKey(KeybindActions.Left) == true)
        {
            StopAllCoroutines();
            StartCoroutine(MoveArro(arro, upLeftDot, speed));

            upLeft = true;
            none = up = right = down = left = upRight = downRight = downLeft = false;
        }

        if (inputManager.GetKey(KeybindActions.Down) == true && inputManager.GetKey(KeybindActions.Right) == true)
        {
            StopAllCoroutines();
            StartCoroutine(MoveArro(arro, downRightDot, speed));

            downRight = true;
            none = up = right = down = left = upRight = upLeft = downLeft = false;
        }

        if (inputManager.GetKey(KeybindActions.Down) == true && inputManager.GetKey(KeybindActions.Left) == true)
        {
            StopAllCoroutines();
            StartCoroutine(MoveArro(arro, downLeftDot, speed));

            downLeft = true;
            none = up = right = down = left = upRight = upLeft = downRight = false;
        }


        // since you can't go up and down at the same time, or left and right, the game considers this an illegal move, and will automatically reset the arro back to the center dot.
        if ((inputManager.GetKey(KeybindActions.Up) == true && inputManager.GetKey(KeybindActions.Down) == true) || (inputManager.GetKey(KeybindActions.Left) == true && inputManager.GetKey(KeybindActions.Right) == true))
        {
            Debug.Log("can't do that move, resetting to center");

            StopAllCoroutines();
            StartCoroutine(MoveCenter(arro, centerDot, speed));
        }


        // if no buttons are pressed, the arro defaults to the center dot.
        if (inputManager.GetKey(KeybindActions.Up) == false && inputManager.GetKey(KeybindActions.Down) == false && inputManager.GetKey(KeybindActions.Right) == false && inputManager.GetKey(KeybindActions.Left) == false)
        {
            StopAllCoroutines();
            StartCoroutine(MoveCenter(arro, centerDot, speed));

            pressed = false;

            none = true;
            up = right = down = left = upRight = upLeft = downRight = downLeft = false;
        }
    }

    // this IEnumerator determines where the arro goes. it has 3 things: the arro, the dot, and the speed.
    // i utilized this to customize where the arro will go depending on which buttons were pressed.
    private IEnumerator MoveArro(GameObject arro, GameObject dot, float speed)
    {
        while (arro.transform.position != dot.transform.position)
        {
            arro.transform.position = Vector3.MoveTowards(arro.transform.position, dot.transform.position, (speed * 10.0f) * Time.deltaTime);
            yield return null;
        }
    }

    // this one handles the arro's movement to the center.
    private IEnumerator MoveCenter(GameObject arro, GameObject center, float speed)
    {
        while (arro.transform.position != center.transform.position)
        {
            arro.transform.position = Vector3.MoveTowards(arro.transform.position, center.transform.position, (speed * 10.0f) * Time.deltaTime);
            yield return null;
        }
    }
}
