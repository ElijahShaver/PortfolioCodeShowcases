using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NPCController : MonoBehaviour
{
    public float moveSpeed;
    public ContactFilter2D movementFilter;
    public float collisionOffset = 0.05f;
    public PlayerController playerControl;
    public SpriteRenderer spriteRenderer;

    public GameObject NPC;
    public GameObject player;

    public bool isSprinting = false;
    public bool isInteracting = false;
    public bool canMove;
    public bool isMoving;
    public bool isPaused;
    public bool shouldNotMove;
    public bool shouldFollow;
    public float offset;

    public Vector2 movementInput;
    Rigidbody2D rb;

    private Animator animator;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
    }

    public void Update()
    {
        // if the shouldFollow boolean is set to true, the NPC that's given will follow behind the player depending on which direction they're facing.
        // i tried to get this to behave similar to how other RPGs like Deltarune handle NPCs in a party, but due to time constraints, i was only able to make it so the NPC is offset at a fixed distance away from the player at all times.
        // it looks pretty awkward during gameplay, but it works!
        if (shouldFollow)
        {
            if (playerControl.movementInput.x > 0)
            {
                transform.position = new Vector2(player.transform.position.x + offset, player.transform.position.y);
                
                Debug.Log("moving +X");

                if (playerControl.movementInput.x == 0 && playerControl.movementInput.y == 0)
                {
                    transform.position = new Vector2(player.transform.position.x + offset, player.transform.position.y);
                }
            }

            if (playerControl.movementInput.x < 0)
            {
                transform.position = new Vector2(player.transform.position.x - offset, player.transform.position.y);

                Debug.Log("moving -X");

                if (playerControl.movementInput.x == 0 && playerControl.movementInput.y == 0)
                {
                    transform.position = new Vector2(player.transform.position.x - offset, player.transform.position.y);
                }
            }

            // for the Y coordinate, if the player is moving down (thus facing the camera), the NPC's Z order in the hierarchy is set to be behind the player to make it look like they're behind relative to the camera's perspective.
            // the opposite can be said for when the player is moving up, thus facing away from the camera.
            if (playerControl.movementInput.y > 0)
            {
                transform.position = new Vector2(player.transform.position.x, player.transform.position.y + offset);

                Debug.Log("moving +Y");

                spriteRenderer.sortingOrder = 1;

                if (playerControl.movementInput.x == 0 && playerControl.movementInput.y == 0)
                {
                    transform.position = new Vector2(player.transform.position.x, player.transform.position.y + offset);
                }
            }

            if (playerControl.movementInput.y < 0)
            {
                transform.position = new Vector2(player.transform.position.x, player.transform.position.y - offset);

                Debug.Log("moving -Y");

                spriteRenderer.sortingOrder = -1;

                if (playerControl.movementInput.x == 0 && playerControl.movementInput.y == 0)
                {
                    transform.position = new Vector2(player.transform.position.x, player.transform.position.y - offset);
                }
            }
        }
    }

    // this freezes the NPC's rigidbody constraints so they can't move.
    public void FreezeEverybodyClapYourHands()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    // this unfreezes them.
    public void Unfreeze()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        Invoke("NoRotate", 0.05f);
    }

    // this freezes everything except the Y coordinate.
    public void FreezeNotY()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
    }

    // this makes it so the rotation is frozen.
    void NoRotate()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void Paused()
    {
        canMove = false;
    }

    public void NotPaused()
    {
        canMove = true;
    }

    // the lines of code below determine the sprite used. for example, if the NPC is supposed to be moving left, then a sprite of them moving left will be shown.
    private void FixedUpdate()
    {
        if (movementInput != Vector2.zero)
        {
            int count = rb.Cast(movementInput, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime * collisionOffset);

            if (count == 0)
            {
                rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
            }
        }
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();

        if (shouldFollow)
        {
            if (movementInput.x != 0 || movementInput.y != 0)
            {
                animator.SetFloat("x", movementInput.x);
                animator.SetFloat("y", movementInput.y);

                animator.SetBool("isWalking", true);

                Debug.Log("you are walking");
            }

            else
            {
                animator.SetBool("isWalking", false);

                Debug.Log("you are not walking");
            }
        }
    }
}
