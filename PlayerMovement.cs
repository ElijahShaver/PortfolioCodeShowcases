using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public LayerMask notPlayer;
    public DialogueManager dialogueManager;

    public GameObject cheaterModeStuff;

    Vector3 moveInput;

    public float characterSpeed;

    public float walkSpeed;
    public float runSpeed;
    public float jumpForce;

    public bool isJumping;
    public bool isGrounded;
    public bool isGrappling;
    public bool hasGrappled;
    public bool fallFast;

    public bool isCheating;

    public float storedMass;
    public Vector3 storedGravity;
    public Vector3 currentGravity;

    public float groundCheckNum;

    float storedWalkSpeed, storedRunSpeed;

    // Start is called before the first frame update
    void Start()
    {
        fallFast = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        storedMass = rb.mass;
        storedGravity = new Vector3(0, -9.81f, 0);

        storedWalkSpeed = walkSpeed;
        storedRunSpeed = runSpeed;

        rb = GetComponent<Rigidbody>();
        dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        cheaterModeStuff = GameObject.Find("CheaterText");
    }

    // Update is called once per frame
    void Update()
    {
        // activates cheater mode. this was actually just a dev tool so i could move super fast around the level and test out different things without worrying about losing.
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            isCheating = !isCheating;
            Debug.Log("cheater mode on/off");
        }

        // this is the regular player movement and mechanics. i'll explain it all as i go down.
        if (!isCheating)
        {
            cheaterModeStuff.SetActive(false);

            if (Input.GetKey(KeyCode.Q))
            {
                Debug.Log(rb.mass.ToString());
            }

            // sets the player's gravity to the current gravity that is given.
            currentGravity = Physics.gravity;

            // if the player should be falling fast after grappling, the gravity is increased by 7 units.
            switch (fallFast)
            {
                case true:
                    Physics.gravity = new Vector3(storedGravity.x, -16.81f, storedGravity.z);
                    break;
                case false:
                    Physics.gravity = new Vector3(storedGravity.x, -9.81f, storedGravity.z);
                    break;
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                Debug.Log(Physics.gravity);
            }

            // unused code
            if (isGrappling && !isGrounded)
            {
                //hasGrappled = true;
            }

            // if the player has already grappled and they have let go, then the game determines how much the velocity should decelerate depending on how fast or slow they're going to prevent any bugs with movement.
            // if the player releases at a high speed, the velocity decelerates quicker.
            if (hasGrappled)
            {
                if ((rb.velocity.x > 0 && rb.velocity.x < 30) || (rb.velocity.x < 0 && rb.velocity.x > -30) || rb.velocity.y > 0 || (rb.velocity.z > 0 && rb.velocity.z < 30) || (rb.velocity.z < 0 && rb.velocity.z > -30))
                {
                    rb.velocity = new Vector3(rb.velocity.x * 0.99f, rb.velocity.y, rb.velocity.z * 0.99f);
                }

                else if (rb.velocity.x >= 30 || rb.velocity.x <= -30 || rb.velocity.y > 0 || rb.velocity.z >= 30 || rb.velocity.z <= -30)
                {
                    rb.velocity = new Vector3(rb.velocity.x * 0.9f, rb.velocity.y, rb.velocity.z * 0.9f);
                }
            }

            // all of this determines whether or not the player is grounded. it checks a certain number of units from the bottom of the player, then uses that to see if the player should be considered grounded or not.
            // if the player is grounded, they can jump once.
            RaycastHit hit;

            if (Physics.Raycast(this.transform.position, -this.transform.up, out hit, groundCheckNum, notPlayer))
            {
                //Debug.Log("on ground, on "+ hit.transform.name);

                isGrounded = true;

                rb.mass = storedMass;

                hasGrappled = false;
            }
            else
            {
                //Debug.Log("nuh uh");

                isGrounded = false;
            }

            moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            transform.Translate(moveInput * characterSpeed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            }
        }

        // this is the same thing, but for cheater mode. not much changed here other than the removal of the raycast for determining whether or not the player is grounded so you can jump infinitely.
        else
        {
            cheaterModeStuff.SetActive(true);

            if (Input.GetKey(KeyCode.Q))
            {
                Debug.Log(rb.mass.ToString());
            }

            currentGravity = Physics.gravity;

            switch (fallFast)
            {
                case true:
                    Physics.gravity = new Vector3(storedGravity.x, -16.81f, storedGravity.z);
                    break;
                case false:
                    Physics.gravity = new Vector3(storedGravity.x, -9.81f, storedGravity.z);
                    break;
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                Debug.Log(Physics.gravity);
            }

            if (isGrappling && !isGrounded)
            {
                //hasGrappled = true;
            }

            if (hasGrappled)
            {
                if ((rb.velocity.x > 0 && rb.velocity.x < 30) || (rb.velocity.x < 0 && rb.velocity.x > -30) || rb.velocity.y > 0 || (rb.velocity.z > 0 && rb.velocity.z < 30) || (rb.velocity.z < 0 && rb.velocity.z > -30))
                {
                    rb.velocity = new Vector3(rb.velocity.x * 0.99f, rb.velocity.y, rb.velocity.z * 0.99f);
                }

                else if (rb.velocity.x >= 30 || rb.velocity.x <= -30 || rb.velocity.y > 0 || rb.velocity.z >= 30 || rb.velocity.z <= -30)
                {
                    rb.velocity = new Vector3(rb.velocity.x * 0.9f, rb.velocity.y, rb.velocity.z * 0.9f);
                }
            }

            RaycastHit hit;

            if (Physics.Raycast(this.transform.position, -this.transform.up, out hit, groundCheckNum, notPlayer))
            {
                //Debug.Log("on ground, on "+ hit.transform.name);

                isGrounded = true;

                rb.mass = storedMass;

                hasGrappled = false;
            }
            else
            {
                //Debug.Log("nuh uh");

                isGrounded = false;
            }

            moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            transform.Translate(moveInput * characterSpeed * Time.deltaTime);

            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            }
        }
    }
}
