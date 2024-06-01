using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;

public class EditorControls : MonoBehaviour
{
    public GameObject TimelineLine;
    public GameObject TimelineLineStart;
    public AudioSource audioSource;

    public PropertiesHandler propertiesHandler;
    public ChangeCameras changeCameras;

    [Header("Notification")]
    public PlayableDirector notification;
    public RectTransform notifPanel;
    public TMP_Text notifText;

    public PlayableDirector altNotif;
    public RectTransform altNotifPanel;
    public TMP_Text altNotifText;

    public float notifWidth; // = 300
    public float notifHeight; // = 50

    [Header("Level Settings")]
    public TMP_InputField levelNameTextField;
    public TMP_InputField levelCreatorTextField;
    public TMP_InputField levelDescTextField;

    [Header("Other")]
    public GameObject arro;
    private ArroHealth arroHealth;
    public ArroController arroController;

    public float speed = 0f;

    public float xMove;

    public bool isPlaying;
    public bool playingMusic;

    public bool dragMode = true;
    public bool propertiesMode;
    public bool deleteMode;

    public TMP_Text modeText;
    public TMP_Text modeTextDesc;

    public bool hasReset;

    public bool hasSaved;
    public bool hasLoaded;

    public bool notPlayed;

    public bool hasDeselected;
    public bool isTyping;

    int playOnce = 0;
    int pauseOnce = 0;

    [Header("Default Settings")]
    public float defaultCamPosX = 0;
    public float defaultCamPosY = 0;
    public float defaultCamRotation = 0;
    public float defaultCamZoom = 6;
    public float defaultLevelLength = 250;

    public float defaultBGCH;
    public float defaultBGCS;
    public float defaultBGCV;
    public Color defaultBGColor;

    public float defaultArroSpeed = 5;

    public string levelName;
    public string levelAudio;
    public string levelCreator;
    public string levelDesc;

    public Scene scene;

    [Header("Default input stuff")]
    public GameObject defaultProperties;

    public TMP_InputField DFCLX;
    public TMP_InputField DFCLY;
    public TMP_InputField DFCR;
    public TMP_InputField DFCZ;

    public TMP_InputField DFAS;

    public Slider DFBGH;
    public Slider DFBGS;
    public Slider DFBGV;

    public TMP_Text DFHT;
    public TMP_Text DFST;
    public TMP_Text DFVT;

    public Image DFBGC;

    public bool showDefaultSettings;

    [Header("hsv placeholders, hope they work (they work)")]
    public float hyoo;
    public float sachyurashin;
    public float valyew;

    // Start is called before the first frame update
    void Start()
    {
        arro = GameObject.FindGameObjectWithTag("Player");
        arroHealth = arro.GetComponent<ArroHealth>();

        scene = SceneManager.GetActiveScene();

        // finds the ArroController so the editor can access the Arro (player)'s attributes.
        if (scene.name == "ArroScene")
        {
            arroController = GameObject.Find("ArroController").GetComponent<ArroController>();
        }

        isPlaying = false;

        // by default the mode is in drag mode

        if (modeText != null && modeTextDesc != null)
        {
            modeText.text = "Currently in: <b>Drag Mode</b>";
            modeTextDesc.text = "Click and drag on a trigger to move it around the timeline.";
        }

        // controlling the defaults

        //Camera.main.transform.position = new Vector3(defaultCamPosX, defaultCamPosY, 0);
        //Camera.main.transform.rotation = Quaternion.Euler(new Vector3(0, 0, defaultCamRotation));
        //Camera.main.orthographicSize = defaultCamZoom;


        //arroController.speedToChange = defaultArroSpeed;
    }

    private void Awake()
    {
        scene = SceneManager.GetActiveScene();

        // resets things to defaults upon the scene awaking.
        if (scene.name == "ArroScene")
        {
            Camera.main.transform.position = new Vector3(defaultCamPosX, defaultCamPosY, 0);

            Camera.main.transform.rotation = Quaternion.Euler(0, 0, defaultCamRotation);

            Camera.main.orthographicSize = defaultCamZoom;

            Camera.main.backgroundColor = Color.HSVToRGB(defaultBGCH / 360, defaultBGCS / 100, defaultBGCV / 100);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if the player has deselected a text box, then the music can play. this is to prevent the player from playing the level on accident while inputting something into a text field.
        if (Input.GetKeyDown(KeyCode.Space) && hasDeselected && !isTyping)
        {
            isPlaying = !isPlaying;
            playingMusic = !playingMusic;
        }

        // resets everything to its defaults if the timeline line is at its start position. this is the "notPlayed" state, because the level hasn't played yet.
        if (scene.name == "ArroScene" && !isPlaying && TimelineLine.transform.position == TimelineLineStart.transform.position)
        {
            Camera.main.backgroundColor = Color.HSVToRGB(defaultBGCH / 360, defaultBGCS / 100, defaultBGCV / 100);

            notPlayed = true;

            Camera.main.orthographicSize = defaultCamZoom;

            hyoo = defaultBGCH;
            sachyurashin = defaultBGCS;
            valyew = defaultBGCV;
        }

        else
        {
            notPlayed = false;

            defaultBGCH = hyoo;
            defaultBGCS = sachyurashin;
            defaultBGCV = valyew;
        }

        // this controls the speed at which the timeline's line goes.
        // if the level is playing, then the timeline's line goes steadily to the right!
        TimelineLine.transform.Translate(new Vector2(1f, 0f) * speed * Time.deltaTime);

        if (isPlaying)
        {
            speed = 2.0f;
        }

        else if (!isPlaying)
        {
            speed = 0.0f;
        }

        // if your arro isn't dead, the playingMusic boolean determines whether or not music is playing. if it is, then music should be playing as you go!
        if (!arroHealth.isDead)
        {
            if (playingMusic)
            {
                while (playOnce < 1)
                {
                    audioSource.Play();
                    playOnce++;
                }

                pauseOnce = 0;
            }

            else if (!playingMusic)
            {
                while (pauseOnce < 1)
                {
                    audioSource.Pause();
                    pauseOnce++;
                }

                playOnce = 0;
            }
        }

        // if you're dead then the audio stops. self explanatory...
        if (arroHealth.isDead)
        {
            audioSource.Stop();
        }

        if (hasReset)
        {
            arroController.speedToChange = defaultArroSpeed;
        }
    }

    public void StopTyping()
    {
        isTyping = false;
    }

    public void StartTyping()
    {
        isTyping = true;
    }

    // controls if the music is playing or paused
    public void PlayPause()
    {
        isPlaying = !isPlaying;
        playingMusic = !playingMusic;
    }

    // these 3 methods control which mode you are in in the editor.
    // the modeTextDesc text states which mode does what.
    public void DragMode()
    {
        dragMode = true;
        propertiesMode = deleteMode = false;

        modeText.text = "Currently in: <b>Drag Mode</b>";
        modeTextDesc.text = "Click and drag on a trigger to move it around the timeline.";
    }

    public void PropertiesMode()
    {
        propertiesMode = true;
        dragMode = deleteMode = false;

        modeText.text = "Currently in: <b>Properties Mode</b>";
        modeTextDesc.text = "Click on a trigger to view its properties and change them.";
    }

    public void DeleteMode()
    {
        deleteMode = true;
        dragMode = propertiesMode = false;

        modeText.text = "Currently in: <b>Delete Mode</b>";
        modeTextDesc.text = "Click on a trigger to delete it from the timeline. Be careful.";
    }

    // resets everything in the scene. the triggers, the line, your lives, the music, all that stuff.
    public void ResetLine()
    {
        TimelineLine.transform.position = TimelineLineStart.transform.position;
        isPlaying = false;
        speed = 0.0f;
        arroHealth.health = 5;
        arroHealth.isDead = false;

        arroController.StopAllCoroutines();
        arroController.arro.transform.position = arroController.centerDot.transform.position;

        hasReset = true;
        Invoke("DisableReset", 0.05f);

        playingMusic = false;

        audioSource.Stop();

        notifText.text = "<size=35><i>Scene Reset!</i></size>";

        notifPanel.sizeDelta = new Vector2(300, 50);

        notification.time = 0;
        notification.Play();
    }

    // literally just the above method but without the notif.
    public void ResetNoText()
    {
        TimelineLine.transform.position = TimelineLineStart.transform.position;
        isPlaying = false;
        speed = 0.0f;
        arroHealth.health = 5;
        arroHealth.isDead = false;

        arroController.StopAllCoroutines();
        arroController.arro.transform.position = arroController.centerDot.transform.position;

        hasReset = true;
        Invoke("DisableReset", 0.5f);

        playingMusic = false;

        audioSource.Stop();
    }

    // notifies you if you saved the scene
    public void SaveScene()
    {
        //hasSaved = true;
        //Invoke("DisableSave", 0.25f);

        notifText.text = "<size=35><i>Level Saved!</i></size>" + "\n" + "<size=15>\"" + levelName + ".json\"</size>";

        notifPanel.sizeDelta = new Vector2(300, 80);

        notification.time = 0;
        notification.Play();
    }

    // notifies you if you loaded the scene
    public void LoadScene()
    {
        //hasLoaded = true;
        //Invoke("DisableLoad", 0.25f);

        notifText.text = "<size=35><i>Level Loaded!</i></size>" + "\n" + "<size=15>\"" + levelName + ".json\"</size>";

        notifPanel.sizeDelta = new Vector2(300, 80);

        notification.time = 0;
        notification.Play();
    }

    // unused methods for speeding up and slowing down the timeline line.
    public void SpeedUp()
    {
        speed = 4f;
    }

    public void SlowDown()
    {
        speed = 1f;
    }

    public void NormalSpeed()
    {
        speed = 2f;
    }

    // these are meant to be used by the Invoke method after like 0.1 seconds to make the respective bool false.
    void DisableReset()
    {
        hasReset = false;
    }

    void DisableSave()
    {
        hasSaved = false;
    }

    void DisableLoad()
    {
        hasLoaded = false;
    }

    public void DeselectTextField()
    {
        hasDeselected = true;
    }

    public void SelectTextField()
    {
        hasDeselected = false;
    }

    // various warnings for different things. all warnings are explained within the notifText's text.
    public void AboveWarning()
    {
        notifText.text = "<size=20>Trigger cannot be above this height.</size>" + "\n" + "<size=18>Don't want it going out of bounds...</size>";

        notifPanel.sizeDelta = new Vector2(400, 70);

        notification.time = 0;
        notification.Play();
    }
    public void BelowWarning()
    {
        notifText.text = "<size=20>Trigger cannot be below this height.</size>" + "\n" + "<size=18>Don't want it going out of bounds...</size>";

        notifPanel.sizeDelta = new Vector2(400, 70);

        notification.time = 0;
        notification.Play();
    }

    public void BeforeLineWarning()
    {
        notifText.text = "<size=16>Trigger cannot be before the timeline's line.</size>" + "\n" + "<size=18>Don't want it going out of bounds...</size>";

        notifPanel.sizeDelta = new Vector2(420, 60); // hehe funny number

        notification.time = 0;
        notification.Play();
    }

    // resets everything to default values.
    public void ResetDefaults()
    {
        Camera.main.transform.position = new Vector3(defaultCamPosX, defaultCamPosY, 0);

        Camera.main.transform.rotation = Quaternion.Euler(0, 0, defaultCamRotation);

        Camera.main.orthographicSize = defaultCamZoom;

        Camera.main.backgroundColor = Color.HSVToRGB(defaultBGCH / 360, defaultBGCS / 100, defaultBGCV / 100);

        arroController.speedToChange = defaultArroSpeed;

        changeCameras.timelineSlider.maxValue = defaultLevelLength;
    }
}
