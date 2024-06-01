using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

// note: this game's dialogue system utilizes the Ink Unity Integration unitypackage, which allows me to use Ink files to control dialogue easily.
public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public GameObject continueButton;
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public Animator portraitAnimator;

    public Story currentStory;

    private Coroutine displayLineCoroutine;

    public AudioClip dialogueAudioClip;
    public AudioSource audioSource;

    public bool isPlaying;
    public bool canContinue;
    public bool stopAudioSource;

    bool isAddingRT = false;

    public float typingSpeed;

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";

    private void Awake()
    {
        audioSource = this.gameObject.AddComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        dialoguePanel = GameObject.Find("WholeDialoguePanel");
        continueButton = GameObject.Find("ContinueButton");
        dialogueText = GameObject.Find("DialogueText").GetComponent<TMP_Text>();
        nameText = GameObject.Find("NameText").GetComponent<TMP_Text>();
        portraitAnimator = GameObject.Find("PortraitImage").GetComponent<Animator>();
        dialoguePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying)
        {
            return;
        }

        // if the player presses E and the Ink file has more dialogue lines, it continues the story.
        if (Input.GetKeyDown(KeyCode.E) && canContinue)
        {
            ContinueStory();
        }
    }

    // this method enters dialogue mode utilizing the .JSON file that came with the Ink file.
    public void EnterDialogueMode(TextAsset inkJSON)
    {
        canContinue = false;

        currentStory = new Story(inkJSON.text);
        isPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    // exits dialogue mode.
    public void ExitDialogueMode()
    {
        Debug.Log("stop");
        isPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    // this continues to the next line of dialogue. if there is no line of dialogue that proceeds, then it exits dialogue mode.
    public void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }

            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));

            HandleTags(currentStory.currentTags);
        }
        else
        {
            ExitDialogueMode();
        }
    }

    // this is the IEnumerator that is responsible for displaying each line of dialogue.
    private IEnumerator DisplayLine(string line)
    {
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;

        isAddingRT = false;

        canContinue = false;

        continueButton.SetActive(false);

        // this foreach loop adds one character at a time from the line of dialogue to the dialogue box on screen.
        // pressing left shift makes it finish instantly.
        foreach (char letter in line.ToCharArray())
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                dialogueText.maxVisibleCharacters = line.Length;
                break;
            }

            // this but of code here is responsible for not making dialogue display things inside <these brackets>, like when a line of dialogue is italicized, bolded, or colored.
            // for example, if a line of dialogue is put into the Ink file as "<b>this is a line</b>" it will show up as "this is a line" except bolded.
            if (letter == '<' || isAddingRT)
            {
                isAddingRT = true;
                
                if (letter == '>')
                {
                    isAddingRT = false;
                }
            }

            // each character that's added makes a noise too. this wasn't necessary, but it was a fun addition!
            else
            {
                dialogueText.maxVisibleCharacters++;

                if (stopAudioSource)
                {
                    audioSource.Stop();
                }

                audioSource.PlayOneShot(dialogueAudioClip);
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        // after the all the characters are done being put on screen, you can continue the dialogue.

        continueButton.SetActive(true);

        canContinue = true;
    }

    // this handles the different tags that are put into the Ink file.
    // the SPEAKER_TAG tag tells the player the name of which character is speaking, and the PORTRAIT_TAG tag shows the player an image of the character who is speaking.
    public void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');

            if (splitTag.Length != 2)
            {
                Debug.LogError("tag " + tag + " couldnt be parsed sorry :(");
            }

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    nameText.text = tagValue;
                    break;
                case PORTRAIT_TAG:
                    portraitAnimator.Play(tagValue);
                    break;
                case LAYOUT_TAG:
                    break;
                default:
                    Debug.LogWarning("tag" + tag +  "came in but not being handled rn");
                    break;
            }
        }
    }
}
