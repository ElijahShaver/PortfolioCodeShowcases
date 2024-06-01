using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPositionTrigger : MonoBehaviour
{
    public string objectType = "CamPositionTrigger";

    public GameObject childTrigger;
    public Transform parentTriggerList;

    public GameObject EditorManager;
    public EditorControls editorControls;

    public GameObject wholeTrigger;

    public bool hasTriggered;
    public bool shouldTrack;

    [Header("Position Start")]
    public float camPositionX1;
    public float camPositionY1;
    private Vector3 camPos1;

    [Header("Position End")]
    public float camPositionX2;
    public float camPositionY2;
    private Vector3 camPos2;

    [Header("Timing/Easing")]
    public AnimationCurve animCurve;
    public float setSpeed;
    private float moveSpeed;

    private float current, target;

    [Header("Trigger Position")]
    [SerializeField] private float WTxPos;
    [SerializeField] private float WTyPos;

    // Start is called before the first frame update
    void Start()
    {
        parentTriggerList = GameObject.Find("TriggerList").GetComponent<Transform>();

        childTrigger.transform.SetParent(parentTriggerList);

        EditorManager = GameObject.Find("EditorManager");
        editorControls = EditorManager.GetComponent<EditorControls>();

        shouldTrack = true;
    }

    // Update is called once per frame
    void Update()
    {
        float zPosOther = 1f;
        
        // these next 3 if statements prevent the trigger from going above, below, and behind the timeline.
        if (wholeTrigger.transform.position.y > -100.5)
        {
            wholeTrigger.transform.position = new Vector3(wholeTrigger.transform.position.x, -100.50f, zPosOther);
            editorControls.AboveWarning();
            Debug.LogWarning("Trigger should not exceed above this point. Don't want it going out of bounds...");
        }

        if (wholeTrigger.transform.position.y < -106.5)
        {
            wholeTrigger.transform.position = new Vector3(wholeTrigger.transform.position.x, -106.5f, zPosOther);
            editorControls.BelowWarning();
            Debug.LogWarning("Trigger should not exceed below this point. Don't want it going out of bounds...");
        }

        if (wholeTrigger.transform.position.x < -13.5)
        {
            wholeTrigger.transform.position = new Vector3(-13.5f, wholeTrigger.transform.position.y, zPosOther);
            editorControls.BeforeLineWarning();
        }

        // determines if the trigger should track the camera's current position to make for better movement
        if (shouldTrack)
        {
            camPos1 = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
            camPositionX1 = Camera.main.transform.position.x;
            camPositionY1 = Camera.main.transform.position.y;
        }

        // determines the X and Y coordinates the camera should move to.
        camPos2 = new Vector3(camPositionX2, camPositionY2, 0);

        // determines how fast or slow the camera moves
        current = Mathf.MoveTowards(current, target, Time.deltaTime / moveSpeed);

        // if the trigger has been triggered by the timeline's line, the camera moves to the given X and Y coordinates in a given amount of seconds.
        if (hasTriggered)
        {
            target = 1;

            Camera.main.transform.position = Vector3.Lerp(camPos1, camPos2, animCurve.Evaluate(current));
        }

        // if the camera has reached its destination, the trigger isnt triggered anymore
        if (current == 1)
        {
            hasTriggered = false;
        }

        // if the timeline is paused, the camera doesnt move. self-explanatory.
        if (!editorControls.isPlaying)
        {
            moveSpeed = int.MaxValue;
        }

        else if (editorControls.isPlaying)
        {
            moveSpeed = setSpeed;
        }

        // if the timeline has reset, it resets the triggers properties back to what they were before it got triggered.
        if (editorControls.hasReset)
        {
            hasTriggered = false;
            shouldTrack = true;

            current = 0;
            target = 0;

            Camera.main.transform.position = new Vector3(editorControls.defaultCamPosX, editorControls.defaultCamPosY, 0);
        }

        WTxPos = wholeTrigger.transform.position.x;
        WTyPos = wholeTrigger.transform.position.y;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TimelineLine"))
        {
            hasTriggered = true;

            shouldTrack = false;
        }
    }
}