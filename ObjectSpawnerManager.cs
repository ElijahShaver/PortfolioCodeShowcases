using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnerManager : MonoBehaviour
{
    // these 3 arrays determine these 3 things respectively: which button is being pressed, which trigger it should spawn upon clicking the timeline, and which trigger preview should follow the cursor after clicking a button.
    public ObjectManager[] objectButtons;
    public GameObject[] objectPrefabs;
    public GameObject[] objectPreview;

    public Camera editorCam;

    public GameObject EditorManager;
    public EditorControls editorControls;

    // please note that the first ID entry needs to start with 0, because that's how arrays work. then the second entry is 1, and so on. that's programming for ya!
    public int currentButtonClicked;

    public bool isAbleToSpawn;

    private void Start()
    {
        EditorManager = GameObject.Find("EditorManager");
        editorControls = EditorManager.GetComponent<EditorControls>();
    }

    private void Update()
    {
        // gets the position of the mouse on the screen, as well as the position in the game.
        Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPos = editorCam.ScreenToWorldPoint(screenPos);

        // these are the screen boundaries of the timeline. if it's above or below these coordinates, the trigger you're trying to place will not spawn.
        if ((worldPos.y > -100.5) || (worldPos.y < -111))
        {
            isAbleToSpawn = false;
        }

        else
        {
            isAbleToSpawn = true;
        }

        // if you press the left mouse button, and you're able to spawn the trigger in the timeline, you'll be able to place it!
        // the preview for the trigger will also go away too, since there's no need to have it anymore.
        if (Input.GetMouseButtonDown(0) && objectButtons[currentButtonClicked].hasClicked && isAbleToSpawn) 
        {
            objectButtons[currentButtonClicked].hasClicked = false;
            Instantiate(objectPrefabs[currentButtonClicked], new Vector3(worldPos.x, worldPos.y, 1), Quaternion.identity);
            Debug.Log("object " + currentButtonClicked + " spawned at X: " + worldPos.x + ", Y: " + worldPos.y);

            Destroy(GameObject.FindWithTag("ObjectPreview"));
        }

        // if you try to spawn it outside of the timeline, you'll get an on-screen error message that tells you that you can't place the trigger.
        else if (Input.GetMouseButtonDown(0) && objectButtons[currentButtonClicked].hasClicked && !isAbleToSpawn)
        {
            editorControls.notifText.text = "<size=20>Cannot place trigger here.</size>" + "\n" + "<size=16>Place the trigger in the timeline.</size>";

            editorControls.notifPanel.sizeDelta = new Vector2(300, 60);

            editorControls.notification.time = 0;
            editorControls.notification.Play();
            Debug.LogWarning("Trigger cannot be placed here.");
        }

        // dev tools!! :3
        // these give you the world position and the screen position of the mouse cursor in the console.
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("cursor is at X: " + worldPos.x + ", Y: " + worldPos.y);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("screen cursor is at X: " + screenPos.x + ", Y: " + screenPos.y);
        }
    }
}
