using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using UnityEngine;
using SFB;

public class FileHandler : MonoBehaviour
{
    public EditorControls editorControls;
    public PropertiesHandler propertiesHandler;

    public GameObject lineSpawnTrigger;
    public GameObject camZoomTrigger;
    public GameObject camRotateTrigger;
    public GameObject camPositionTrigger;
    public GameObject backgroundColorTrigger;
    public GameObject arroSpeedTrigger;

    public LineSpawnTrigger lineSpawns;
    public CamPositionTrigger camPosition;
    public CamRotateTrigger camRotation;
    public CamZoomTrigger camZoom;
    public BGColorTrigger backgroundColor;
    public ArroSpeedTrigger arroSpeed;

    public List<GameObject> triggersToSave;

    public string levelName;

    public int listPlace = 0;

    public int bruh = 0;

    public string[] audioPath;

    public string theAudioPath;

    public string[] filePath;

    public string theFilePath;

    private void Start()
    {
        editorControls = GameObject.Find("EditorManager").GetComponent<EditorControls>();

        triggersToSave = new List<GameObject>(GameObject.FindGameObjectsWithTag("Trigger"));

        theFilePath = PlayerPrefs.GetString("path", "");
    }

    private void Update()
    {
        // this was used to get the locations of the triggers to save in the list. its unused now, but it's still a cool thing to have around!
        /*if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("object " + triggersToSave[listPlace] + " at place #" + listPlace);
        }*/

        levelName = editorControls.levelName;
    }

    public void ChooseFilePath()
    {
        filePath = StandaloneFileBrowser.OpenFolderPanel("Set default directory", "", true);

        if (filePath.Length != 0)
        {
            theFilePath = filePath[0];

            PlayerPrefs.SetString("path", theFilePath);

            editorControls.notifText.text = "<size=30><i>Default file path set!</i></size>" + "\n" + "<size=15>Folder: \"" + Path.GetFileName(theFilePath) + "\"</size>";

            editorControls.notifPanel.sizeDelta = new Vector2(340, 80);

            editorControls.notification.time = 0;
            editorControls.notification.Play();

            Debug.Log("Default save/load location set!");
        }
    }

    public void ChooseAudio()
    {
        StartCoroutine(LoadSong());
    }

    IEnumerator LoadSong()
    {
        // tells you which audio files are accepted for songs.
        var extensions = new[]
            {
                new ExtensionFilter("Audio Files", "mp3", "ogg", "wav", "aiff")
            };

        audioPath = StandaloneFileBrowser.OpenFilePanel("Choose Song", "", extensions, false);

        // gets chosen audio from the filepath
        if (audioPath.Length != 0)
        {
            theAudioPath = audioPath[0];

            string url = string.Format("file://{0}", theAudioPath);
            WWW www = new WWW(url);
            yield return www;

            editorControls.audioSource.clip = www.GetAudioClip();

            editorControls.notifText.text = "<size=25><i>Song chosen!</i></size>" + "\n" + "<size=15>\"" + Path.GetFileName(theAudioPath) + "\"</size>";

            editorControls.notifPanel.sizeDelta = new Vector2(350, 70);

            editorControls.notification.time = 0;
            editorControls.notification.Play();
        }
    }

    public void SaveLocations()
    {
        // if the default filepath to save levels from is not set, it will give an error message.
        if (theFilePath.Length == 0)
        {
            editorControls.notifText.text = "<size=22><i>Default file path not set!</i></size>" + "\n" + "<size=15>Choose a file path to save levels in.</size>";

            editorControls.notifPanel.sizeDelta = new Vector2(350, 70);

            editorControls.notification.time = 0;
            editorControls.notification.Play();
        }

        else
        {
            // if the level name is blank, it will not save.
            if (levelName == "")
            {
                editorControls.notifText.text = "<size=25><i>Level name cannot be blank!</i></size>" + "\n" + "<size=18>It's just better with a name!!!</size>";

                editorControls.notifPanel.sizeDelta = new Vector2(400, 70);

                editorControls.notification.time = 0;
                editorControls.notification.Play();
            }

            /* unused code that prevented you from saving the level if there were no triggers in the timeline.
               i soon realized this was a stupid idea.
            if (triggersToSave.Count == 0)
            {
                Debug.LogWarning("Nothing to save! Place something in the timeline!!!");
            }*/

            else if (levelName != "")
            {
                Invoke("SaveAgain", 0.1f);
            }
        }
    }

    public void LoadLocations()
    {
        // if the default filepath to load levels from is not set, it will give an error message.
        if (theFilePath.Length == 0)
        {
            editorControls.notifText.text = "<size=22><i>Default file path not set!</i></size>" + "\n" + "<size=15>Choose a file path to load levels from.</size>";

            editorControls.notifPanel.sizeDelta = new Vector2(350, 70);

            editorControls.notification.time = 0;
            editorControls.notification.Play();
        }

        else
        {
            if (File.Exists(theFilePath + "/" + levelName + ".json"))
            {
                // deletes every object in the timeline to prepare for the next level to be loaded to prevent the level data from the previous level getting mixed in with the data being loaded
                foreach (GameObject trig in triggersToSave)
                {
                    GameObject.Destroy(trig);
                }

                triggersToSave.Clear();

                Invoke("LoadAgain", 0.1f);
            }

            // if the file isn't able to be found, nothing happens. it just gives you an error message.
            else if (!File.Exists(theFilePath + "/" + levelName + ".json"))
            {
                Debug.LogWarning("Cannot find file: " + levelName + ".json in selected file path.");

                editorControls.notifText.text = "<size=25><i>Cannot find JSON file!</i></size>" + "\n" + "<size=15>\"" + editorControls.levelName + ".json\" not found in " + "\n" + "\"" + Path.GetFileName(theFilePath) + "\"</size>";

                editorControls.notifPanel.sizeDelta = new Vector2(350, 90);

                editorControls.notification.time = 0;
                editorControls.notification.Play();
            }
        }
    }

    void SaveAgain()
    {
        // if a file with an existing name already exists, it overwrites the older data with newer data.
        if (File.Exists(theFilePath + "/" + levelName + ".json"))
        {
            editorControls.altNotifText.text = "<size=18>File \"" + levelName + ".json\" already exists" + "\n" + "in folder: \"" + Path.GetFileName(theFilePath) + "\"</size>" + "\n" + "<size=15>Overwriting file...</size>";

            editorControls.altNotifPanel.sizeDelta = new Vector2(400, 90);

            editorControls.altNotif.time = 0;
            editorControls.altNotif.Play();

            File.Delete(theFilePath + "/" + levelName + ".json");
        }

        for (int i = 0; i < triggersToSave.Count; i++)
        {
            if (triggersToSave[i] = null)
            {
                triggersToSave.RemoveAt(i);
            }
        }

        triggersToSave = new List<GameObject>(GameObject.FindGameObjectsWithTag("Trigger"));

        // NOTE: all of these things can be found within the ObjectLocations script. i never bothered to change it to something more fitting so like, deal with it i guess. ( <- this wasnt aimed at anyone but me btw)
        // for some context, that script handles all the trigger types and default settings.

        DefaultSettings defaultSettings = new DefaultSettings();
        ObjectLocations objectLocations = new ObjectLocations();
        LineSpawn lineSpawn = new LineSpawn();
        CameraPosition cameraPosition = new CameraPosition();
        CameraRotation cameraRotation = new CameraRotation();
        CameraZoom cameraZoom = new CameraZoom();
        LineFade lineFade = new LineFade();
        LineHit lineHit = new LineHit();
        BackgroundColor BGColor = new BackgroundColor();
        ArroSpeed ARSpeed = new ArroSpeed();

        // saving default settings

        defaultSettings.defaultCamPosX = editorControls.defaultCamPosX;
        defaultSettings.defaultCamPosY = editorControls.defaultCamPosY;
        defaultSettings.defaultCamRotation = editorControls.defaultCamRotation;
        defaultSettings.defaultCamZoom = editorControls.defaultCamZoom;

        defaultSettings.defaultBGCH = editorControls.defaultBGCH;
        defaultSettings.defaultBGCS = editorControls.defaultBGCS;
        defaultSettings.defaultBGCV = editorControls.defaultBGCV;

        defaultSettings.defaultArroSpeed = editorControls.defaultArroSpeed;

        defaultSettings.defaultLevelLength = editorControls.defaultLevelLength;

        string dflt = JsonConvert.SerializeObject(defaultSettings);

        // this begins the JSON file with a "[" to indicate that it's an array, and is then proceeded by the default settings.
        File.AppendAllText(theFilePath + "/" + levelName + ".json", "[" + dflt + ",");

        // this is the actual process of saving the level data to a JSON file. it loops through each gameobject in triggersToSave and writes down their X, Y, and Z values, appending them all to the file.

        for (int i = 0; i < triggersToSave.Count; i++)
        {
            objectLocations.posX = triggersToSave[i].transform.position.x;
            objectLocations.posY = triggersToSave[i].transform.position.y;
            objectLocations.posZ = 1;

            objectLocations.objName = triggersToSave[i].name;

            // this determines what attributes need to be saved depending on the trigger type.

            switch (triggersToSave[i].name)
            {
                case "CamLocationTrigger(Clone)":
                    CamPositionTrigger camPos = triggersToSave[i].GetComponent<CamPositionTrigger>();

                    cameraPosition.positionEndX = camPos.camPositionX2;
                    cameraPosition.positionEndY = camPos.camPositionY2;
                    cameraPosition.seconds = camPos.setSpeed;
                    break;

                case "CamRotateTrigger(Clone)":
                    CamRotateTrigger camRot = triggersToSave[i].GetComponent<CamRotateTrigger>();

                    cameraRotation.rotationEnd = camRot.camRotation2;
                    cameraRotation.seconds = camRot.setSpeed;
                    break;

                case "CamZoomTrigger(Clone)":
                    CamZoomTrigger camZm = triggersToSave[i].GetComponent<CamZoomTrigger>();

                    cameraZoom.zoomEnd = camZm.camZoom2;
                    cameraZoom.seconds = camZm.setSpeed;
                    break;

                case "LineSpawnTrigger(Clone)":
                    LineSpawnTrigger lnSpwn = triggersToSave[i].GetComponent<LineSpawnTrigger>();

                    Debug.Log("linespawn");

                    lineSpawn.linePosx = lnSpwn.xPos;
                    lineSpawn.linePosy = lnSpwn.yPos;
                    lineSpawn.lineRotation = lnSpwn.rotation;

                    //

                    LineFadeSpawnTrigger lnFdSp = triggersToSave[i].GetComponentInChildren<LineFadeSpawnTrigger>();

                    Debug.Log("fadespawn");

                    lineSpawn.fadePosX = lnFdSp.xPos;
                    lineSpawn.fadePosY = lnFdSp.yPos;
                    lineSpawn.fadePosZ = 1;

                    lineSpawn.fadeObjName = triggersToSave[i].name;

                    //

                    LineHitSpawnTrigger lnHtSp = triggersToSave[i].GetComponentInChildren<LineHitSpawnTrigger>();

                    Debug.Log("hitspawn");

                    lineSpawn.hitPosX = lnHtSp.xPos;
                    lineSpawn.hitPosY = lnHtSp.yPos;
                    lineSpawn.hitPosZ = 1;

                    lineSpawn.hitObjName = triggersToSave[i].name;
                    break;

                case "BGColorTrigger(Clone)":
                    BGColorTrigger bgClr = triggersToSave[i].GetComponent<BGColorTrigger>();

                    BGColor.hue2 = bgClr.hue2;
                    BGColor.saturation2 = bgClr.sat2;
                    BGColor.value2 = bgClr.val2;

                    BGColor.speed = bgClr.setSpeed;
                    break;

                case "ArroSpeedTrigger(Clone)":
                    ArroSpeedTrigger arSpd = triggersToSave[i].GetComponent<ArroSpeedTrigger>();

                    ARSpeed.speed = arSpd.arroSpeed;
                    break;
            }

            string json = JsonConvert.SerializeObject(objectLocations);
            string lnsp = JsonConvert.SerializeObject(lineSpawn);
            string lnfd = JsonConvert.SerializeObject(lineFade);
            string lnht = JsonConvert.SerializeObject(lineHit);
            string cmps = JsonConvert.SerializeObject(cameraPosition);
            string cmrt = JsonConvert.SerializeObject(cameraRotation);
            string cmzm = JsonConvert.SerializeObject(cameraZoom);
            string bgcc = JsonConvert.SerializeObject(BGColor);
            string arsp = JsonConvert.SerializeObject(ARSpeed);

            if (i < triggersToSave.Count)
            {
                switch (triggersToSave[i].name)
                {
                    case "CamLocationTrigger(Clone)":
                        File.AppendAllText(theFilePath + "/" + levelName + ".json", json + "," + cmps + ", ");
                        break;
                    case "CamRotateTrigger(Clone)":
                        File.AppendAllText(theFilePath + "/" + levelName + ".json", json + "," + cmrt + ", ");
                        break;
                    case "CamZoomTrigger(Clone)":
                        File.AppendAllText(theFilePath + "/" + levelName + ".json", json + "," + cmzm + ", ");
                        break;
                    case "LineSpawnTrigger(Clone)":
                        File.AppendAllText(theFilePath + "/" + levelName + ".json", json + "," + lnsp + ", ");
                        break;
                    case "BGColorTrigger(Clone)":
                        File.AppendAllText(theFilePath + "/" + levelName + ".json", json + "," + bgcc + ", ");
                        break;
                    case "ArroSpeedTrigger(Clone)":
                        File.AppendAllText(theFilePath + "/" + levelName + ".json", json + "," + arsp + ", ");
                        break;
                }
            }

            else
            {
                switch (triggersToSave[i].name)
                {
                    case "CamLocationTrigger(Clone)":
                        File.AppendAllText(theFilePath + "/" + levelName + ".json", json + "," + cmps);
                        break;
                    case "CamRotateTrigger(Clone)":
                        File.AppendAllText(theFilePath + "/" + levelName + ".json", json + "," + cmrt);
                        break;
                    case "CamZoomTrigger(Clone)":
                        File.AppendAllText(theFilePath + "/" + levelName + ".json", json + "," + cmzm);
                        break;
                    case "LineSpawnTrigger(Clone)":
                        File.AppendAllText(theFilePath + "/" + levelName + ".json", json + "," + lnsp);
                        break;
                    case "BGColorTrigger(Clone)":
                        File.AppendAllText(theFilePath + "/" + levelName + ".json", json + "," + bgcc);
                        break;
                    case "ArroSpeedTrigger(Clone)":
                        File.AppendAllText(theFilePath + "/" + levelName + ".json", json + "," + arsp);
                        break;
                }
            }
        }

        // ends the file with the level name and a ]
        File.AppendAllText(theFilePath + "/" + levelName + ".json", " ]");

        editorControls.SaveScene();

        Debug.Log("Saved " + levelName + ".json");
    }

    void LoadAgain()
    {
        string json = File.ReadAllText(theFilePath + "/" + levelName + ".json");
        List<ObjectLocations> objectLocations = JsonConvert.DeserializeObject<List<ObjectLocations>>(json);
        string dflt = File.ReadAllText(theFilePath + "/" + levelName + ".json");
        List<DefaultSettings> defaultSettings = JsonConvert.DeserializeObject<List<DefaultSettings>>(dflt);

        propertiesHandler.defaultCLX.text = defaultSettings[0].defaultCamPosX.ToString();
        propertiesHandler.defaultCLY.text = defaultSettings[0].defaultCamPosY.ToString();
        propertiesHandler.defaultCR.text = defaultSettings[0].defaultCamRotation.ToString();
        propertiesHandler.defaultCZ.text = defaultSettings[0].defaultCamZoom.ToString();
        propertiesHandler.defaultLL.text = defaultSettings[0].defaultLevelLength.ToString();

        propertiesHandler.defaultBGH.value = defaultSettings[0].defaultBGCH;
        propertiesHandler.defaultBGS.value = defaultSettings[0].defaultBGCS;
        propertiesHandler.defaultBGV.value = defaultSettings[0].defaultBGCV;

        propertiesHandler.defaultAS.text = defaultSettings[0].defaultArroSpeed.ToString();

        propertiesHandler.ChangeDefaults();

        // this is the actual process of loading the level's data.

        for (int i = 0; i < objectLocations.Count; i++)
        {
            switch (objectLocations[i].objName)
            {
                case "CamLocationTrigger(Clone)":
                    CamPositionTrigger camPos = Instantiate(camPosition, new Vector3(objectLocations[i].posX, objectLocations[i].posY, 1), Quaternion.identity);
                    FindTriggers();

                    Debug.Log("spawned " + camPos.name);

                    string cmps = File.ReadAllText(theFilePath + "/" + levelName + ".json");
                    List<CameraPosition> cameraPosition = JsonConvert.DeserializeObject<List<CameraPosition>>(cmps);

                    camPos.camPositionX1 = cameraPosition[i + 1].positionStartX; ;
                    camPos.camPositionY1 = cameraPosition[i + 1].positionStartY;
                    camPos.camPositionX2 = cameraPosition[i + 1].positionEndX; ;
                    camPos.camPositionY2 = cameraPosition[i + 1].positionEndY; ;
                    camPos.setSpeed = cameraPosition[i + 1].seconds;
                    break;

                case "CamRotateTrigger(Clone)":
                    CamRotateTrigger camRot = Instantiate(camRotation, new Vector3(objectLocations[i].posX, objectLocations[i].posY, 1), Quaternion.identity);
                    FindTriggers();

                    Debug.Log("spawned " + camRot.name);

                    string cmrt = File.ReadAllText(theFilePath + "/" + levelName + ".json");
                    List<CameraRotation> cameraRotation = JsonConvert.DeserializeObject<List<CameraRotation>>(cmrt);

                    camRot.camRotation1 = cameraRotation[i + 1].rotationStart;
                    camRot.camRotation2 = cameraRotation[i + 1].rotationEnd;
                    camRot.setSpeed = cameraRotation[i + 1].seconds;
                    break;

                case "CamZoomTrigger(Clone)":
                    CamZoomTrigger camZm = Instantiate(camZoom, new Vector3(objectLocations[i].posX, objectLocations[i].posY, 1), Quaternion.identity);
                    FindTriggers();

                    Debug.Log("spawned " + camZm.name);

                    string cmzm = File.ReadAllText(theFilePath + "/" + levelName + ".json");
                    List<CameraZoom> cameraZoom = JsonConvert.DeserializeObject<List<CameraZoom>>(cmzm);

                    camZm.camZoom1 = cameraZoom[i + 1].zoomStart;
                    camZm.camZoom2 = cameraZoom[i + 1].zoomEnd;
                    camZm.setSpeed = cameraZoom[i + 1].seconds;
                    break;

                case "LineSpawnTrigger(Clone)":
                    LineSpawnTrigger lnSpwn = Instantiate(lineSpawns, new Vector3(objectLocations[i].posX, objectLocations[i].posY, 1), Quaternion.identity);
                    FindTriggers();

                    Debug.Log("spawned " + lnSpwn.name + " at #" + i);

                    string lnsp = File.ReadAllText(theFilePath + "/" + levelName + ".json");
                    List<LineSpawn> lineSpawn = JsonConvert.DeserializeObject<List<LineSpawn>>(lnsp);


                    Debug.Log("spawned " + lineSpawn[i + 1].fadeObjName);
                    Debug.Log("spawned " + lineSpawn[i + 1].hitObjName);

                    lnSpwn.xPos = lineSpawn[i + 1].linePosx;
                    lnSpwn.yPos = lineSpawn[i + 1].linePosy;
                    lnSpwn.rotation = lineSpawn[i + 1].lineRotation;

                    for (int s = bruh; s < triggersToSave.Count; s++)
                    {
                        switch (triggersToSave[s].name)
                        {
                            case "LineFadeSpawnTrigger":
                                triggersToSave[s].transform.position = new Vector3(lineSpawn[i + 1].fadePosX, lineSpawn[i + 1].fadePosY, 1);
                                Debug.Log("at place #" + s);
                                break;

                            case "LineHitSpawnTrigger":
                                triggersToSave[s].transform.position = new Vector3(lineSpawn[i + 1].hitPosX, lineSpawn[i + 1].hitPosY, 1);
                                Debug.Log("at place #" + s);
                                break;
                        }

                        if (triggersToSave[s].name == "LineHitSpawnTrigger")
                        {
                            bruh = s + 1;

                            Debug.Log("should start at place #" + (s + 1));
                            break;
                        }
                    }
                    break;

                case "BGColorTrigger(Clone)":
                    BGColorTrigger bgColr = Instantiate(backgroundColor, new Vector3(objectLocations[i].posX, objectLocations[i].posY, 1), Quaternion.identity);
                    FindTriggers();

                    Debug.Log("spawned " + bgColr.name);

                    string bgcc = File.ReadAllText(theFilePath + "/" + levelName + ".json");
                    List<BackgroundColor> backgroundColors = JsonConvert.DeserializeObject<List<BackgroundColor>>(bgcc);

                    bgColr.hue1 = backgroundColors[i + 1].hue1;
                    bgColr.sat1 = backgroundColors[i + 1].saturation1;
                    bgColr.val1 = backgroundColors[i + 1].value1;

                    bgColr.hue2 = backgroundColors[i + 1].hue2;
                    bgColr.sat2 = backgroundColors[i + 1].saturation2;
                    bgColr.val2 = backgroundColors[i + 1].value2;

                    bgColr.setSpeed = backgroundColors[i + 1].speed;
                    break;
                case "ArroSpeedTrigger(Clone)":
                    ArroSpeedTrigger aroSpd = Instantiate(arroSpeed, new Vector3(objectLocations[i].posX, objectLocations[i].posY, 1), Quaternion.identity);
                    FindTriggers();

                    Debug.Log("spawned " + aroSpd.name);

                    string arsp = File.ReadAllText(theFilePath + "/" + levelName + ".json");
                    List<ArroSpeed> arroSpeeds = JsonConvert.DeserializeObject<List<ArroSpeed>>(arsp);

                    aroSpd.arroSpeed = arroSpeeds[i + 1].speed;
                    break;
            }
        }

        editorControls.LoadScene();

        Debug.Log("Loaded " + levelName + ".json");

        bruh = 0;
    }

    // finds every gameobject with tag "Trigger" and adds them to the triggersToSave list. this makes finding each trigger and saving their properties easier.
    void FindTriggers()
    {
        triggersToSave = new List<GameObject>(GameObject.FindGameObjectsWithTag("Trigger"));
    }

    public void ResetBruh()
    {
        bruh = 0;

        // this was never used LMAO i forgot what i was gonna use it for. probably to test resetting the level or something.
    }
}