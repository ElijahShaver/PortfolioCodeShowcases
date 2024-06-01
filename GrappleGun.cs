using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrappleGun : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public PauseMenuManager pmm;
    public GameObject gun;

    public Transform muzzle, returnPoint, mainCam, player;

    public Image reticle;

    public LayerMask redReticle;
    public LayerMask greenReticle;

    public float maxDistance;

    public LineRenderer lineRenderer;
    private Vector3 grapplePoint;
    public LayerMask canGrapple;
    public LayerMask excludePlayer;

    private SpringJoint joint;

    [SerializeField] bool grappling;
    bool inDistance;

    public Quaternion storedRotation;

    // Start is called before the first frame update
    void Start()
    {
        gun = this.gameObject;
        reticle = GameObject.Find("reticle").GetComponent<Image>();
        pmm = GameObject.Find("PauseMenuManager").GetComponent<PauseMenuManager>();
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // if the game isn't paused, the player can grapple.
        if (!pmm.isPaused)
        {
            // if the player is in range to grapple to a grappleable object, and they hold down the left mouse button, they can grapple and swing across!
            if (Input.GetMouseButtonDown(0))
            {
                StartGrapple();

                if (inDistance)
                {
                    lineRenderer.enabled = true;
                }

                playerMovement.rb.mass = playerMovement.storedMass;

                grappling = true;
            }

            // if the player stops holding down, they stop grappling, and the player's rigidbody's mass is increased to make them fall to the ground faster.
            else if (Input.GetMouseButtonUp(0))
            {
                StopGrapple();
                grappling = false;

                if (!playerMovement.isGrounded)
                {
                    playerMovement.rb.mass = playerMovement.storedMass + 15;

                    playerMovement.fallFast = true;
                }
            }
        }

        // determines if the player is looking at a grappleable surface.
        // if the reticle is red, then they're in range to grapple, but they're looking at an ungrappleable surface.
        // if the reticle is green, then they're in range to grapple, and they're looking at a grappleable surface.
        // if the reticle is black, they're not in range to grapple.
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, maxDistance, redReticle))
        {
            reticle.color = Color.red;
            inDistance = true;
        }

        else if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, maxDistance, greenReticle))
        {
            reticle.color = Color.green;
            inDistance = true;
        }

        else
        {
            reticle.color = Color.black;
            inDistance = false;
        }
    }

    // the LateUpdate method is used to handle the drawing of the rope.
    private void LateUpdate()
    {
        if (!pmm.isPaused)
        {
            switch (grappling)
            {
                case true:

                    playerMovement.hasGrappled = false;

                    //gun.transform.LookAt(theHit.transform.position);
                    //gun.transform.rotation *= Quaternion.Euler(-90, 0, 90);

                    DrawRope();

                    break;
                case false:
                    StopDrawRope();

                    //gun.transform.LookAt(returnPoint.transform.position);
                    //gun.transform.rotation *= Quaternion.Euler(-90, 0, 90);

                    if (!playerMovement.isGrounded)
                    {
                        playerMovement.hasGrappled = true;
                    }
                    break;
            }
        }
    }

    // this method starts the grapple.
    void StartGrapple()
    {
        Debug.Log("yeag clicked");

        RaycastHit theHit;

        // if the player is in distance to grapple, and is looking at a grappleable object, then the grapple gun starts to, well, grapple!
        // the joint here is used to give the player a sense of swinging and elasticity to the grapple gun's rope, thus allowing the player to swing back and forth.
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out theHit, maxDistance, canGrapple))
        {
            playerMovement.isGrappling = true;

            Debug.Log("hit at " + theHit.point.x + "," + theHit.point.y + "," + theHit.point.z);

            grapplePoint = theHit.point;

            joint = player.gameObject.AddComponent<SpringJoint>();

            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float pointDistance = Vector3.Distance(player.position, grapplePoint);

            joint.maxDistance = pointDistance * 0.1f;
            joint.minDistance = pointDistance * 0.05f;

            joint.spring = 20f;
            joint.damper = 10f;
            joint.massScale = 5f;

            if (!playerMovement.isGrounded)
            {
                playerMovement.fallFast = false;
            }
        }

        // even if the player isn't looking at a grappleable surface, they can still have the rope appear. it just won't do anything physically.
        else if (Physics.Raycast(mainCam.position, mainCam.transform.forward, out theHit, maxDistance, excludePlayer))
        {
            Debug.Log("hit at " + theHit.point.x + "," + theHit.point.y + "," + theHit.point.z);

            grapplePoint = theHit.point;

            if (!playerMovement.isGrounded)
            {
                playerMovement.fallFast = true;
            }
        }
    }

    // if the player isn't grappling, it destroys the joint so the player doesn't swing anymore.
    void StopGrapple()
    {
        playerMovement.isGrappling = false;

        Destroy(joint);
    }

    // this allows the rope to actually be drawn from the grapple gun's muzzle to the surface that it's supposed to land on.
    void DrawRope()
    {
        lineRenderer.SetPosition(0, muzzle.position);
        lineRenderer.SetPosition(1, grapplePoint);
    }

    // this stops the rope from being drawn.
    void StopDrawRope()
    {
        lineRenderer.enabled = false;
    }
}
