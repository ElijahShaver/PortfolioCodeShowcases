using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject playerCam;
    public GameObject DeathScreen;
    public GameObject WinScreen;
    public GameplayCountdown gameplayCountdown;
    public PlayableDirector closeTransition;

    Vector3 moveInput;

    public float characterSpeed;

    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float stompForce;
    [SerializeField] float dashForce;

    public bool isJumping;
    public bool isGrounded;

    public bool hasDashed;

    public int dashCount;
    public int jumpCount;
    int storedJumpCount;
    int storedDashCount;

    int stompCount;

    public float dashStopCooldown;

    public bool isDead;
    public bool hasWon;
    public bool canDie;

    public bool cantMove;

    float deathBounceTimes = 0;

    public string thisSceneName;
    public string sceneToGoTo;

    bool hasStarted;

    // Start is called before the first frame update
    void Start()
    {
        canDie = true;
        gameplayCountdown = GetComponent<GameplayCountdown>();
        DeathScreen = GameObject.Find("DeathPanel");
        WinScreen = GameObject.Find("WinPanel");
        closeTransition = GameObject.Find("ClosingTimeline").GetComponent<PlayableDirector>();

        DeathScreen.SetActive(false);
        WinScreen.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>();

        storedJumpCount = jumpCount;
        storedDashCount = dashCount;

        stompCount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // commented these out before releasing them, but they did work during development! i think some of them might still be here...
        // also, a dash feature was planned but it was a pain to implement so i ended up scrapping it.
        /*list of debug keys:
        I / O: transitions
        T: start moving trucks
        L: show/hide triggers

        /* ive decided not to add a dash function
         * 
         * if (hasDashed)
        {
            dashStopCooldown += Time.deltaTime;

            if (dashStopCooldown >= 0.5)
            {
                rb.velocity = new Vector3(rb.velocity.x / 3, rb.velocity.y / 3, rb.velocity.z / 3);
                rb.velocity = new Vector3(rb.velocity.x / 3, rb.velocity.y / 3, rb.velocity.z / 3);

                hasDashed = false;
            }
        }

       *if (Input.GetButtonDown("Dash") && dashCount > 0)
        {
            dashStopCooldown = 0;

            hasDashed = true;

            rb.AddForce(playerCam.transform.forward * dashForce);

            dashCount--;
        }*/

        // if the player isn't dead, they can move. pretty simple!
        if (!isDead)
        {
            if (gameplayCountdown.hasStarted)
            {
                if (!hasWon)
                {
                    // determines sprint speed.
                    if (Input.GetButton("Sprint"))
                    {
                        characterSpeed = runSpeed;
                    }
                    else
                    {
                        characterSpeed = walkSpeed;
                    }
                }
            }

            // calculates how the player moves if they're able to move.
            if (!cantMove)
            {
                moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

                transform.Translate(moveInput * characterSpeed * Time.deltaTime);
            }

            // if the player presses the jump button, two things can happen: if the level hasn't started yet and the player jumps, the level starts. the jump button acts as a regular jump button otherwise.
            if (Input.GetButtonDown("Jump") && jumpCount != 0)
            {
                if (!hasStarted)
                {
                    rb.AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);

                    isJumping = false;

                    jumpCount--;

                    hasStarted = true;
                }

                else
                {
                    rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);

                    isJumping = false;

                    jumpCount--;
                }
            }

            // if the player has a stomp (gained after landing on a truck and when you're starting the level), the player is forced downwards.
            if (Input.GetButtonDown("Stomp") && !isGrounded && stompCount != 0)
            {
                rb.AddForce(new Vector3(0, -stompForce, 0), ForceMode.Impulse);

                stompCount--;
            }
        }

        // if the player is dead, then the death screen shows up. pressing space restarts the level.
        if (isDead && canDie)
        {
            DeathScreen.SetActive(true);

            if (Input.GetButtonDown("Jump") && closeTransition.time <= 0)
            {
                closeTransition.Play();

                Invoke("ReloadScene", 1);
            }
        }

        // if the player has won and hasnt died, then the game moves onto the next level!
        if (hasWon && !isDead)
        {
            WinScreen.SetActive(true);

            if (Input.GetButtonDown("Jump") && closeTransition.time <= 0)
            {
                closeTransition.Play();

                Invoke("LoadNextScene", 1);
            }
        }

        // dev tools to make the game faster or to make it so i'm invincible and really fast!

        if (Input.GetKeyDown(KeyCode.K))
        {
            Time.timeScale = 5;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            canDie = !canDie;
            runSpeed = 200;
        }

        //keeping this because its funny lol
        // just added it to the OnTriggerEnter method instead
        /*if (isDead)
        {
            float randomDestX = Random.Range(-0.7f, 0.7f);
            float randomDestY = Random.Range(-0.7f, 0.7f);
            float randomDestZ = Random.Range(-0.7f, 0.7f);

            rb.constraints = RigidbodyConstraints.None;
            rb.AddForce(new Vector3(randomDestX, randomDestY, randomDestZ), ForceMode.Impulse);
            walkSpeed = 0;
            runSpeed = 0;
        }*/
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(thisSceneName);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToGoTo);
    }

    private void OnTriggerEnter(Collider other)
    {
        // if the player falls onto a death zone (aka pretty much anything that is the ground or the buildings) then they will lose.
        // in order to spice up the death animation, if the amount of times the player bounces on the death zone is less than 5, the rigidbody will jostle around a bit since some force will be applied to it at random directions.
        // this makes it so the player falls over and doesnt stand still after losing.
        if (other.tag == "DeathZone" && canDie && !hasWon)
        {
            deathBounceTimes++;

            isDead = true;

            float randomDestX = Random.Range(-0.7f, 0.7f);
            float randomDestY = Random.Range(1f, 2f);
            float randomDestZ = Random.Range(-0.7f, 0.7f);

            rb.constraints = RigidbodyConstraints.None;

            if (deathBounceTimes < 5)
            {
                rb.AddForce(new Vector3(randomDestX, randomDestY, randomDestZ), ForceMode.Impulse);
            }
        }

        // if the player hits the goalpost, they win!
        if (other.tag == "Goal")
        {
            hasWon = true;

            characterSpeed = 0;
        }
    }

    // if the player lands on a truck, their jumps and stomps are restored.
    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
        stompCount = 1;
        jumpCount = storedJumpCount;
        dashCount = storedDashCount;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
