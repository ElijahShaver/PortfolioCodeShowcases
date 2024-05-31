using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BinScript : MonoBehaviour
{
    public CompletionManager completionManager;

    public bool isFull;

    public TMP_Text statusText;

    public bool shouldCount, hasBad;

    public int goodCount, badCount, objectsNeeded;

    public List<Collider> colliderList, goodColliderList;

    // Start is called before the first frame update
    void Start()
    {
        completionManager = GameObject.Find("CompletionManager").GetComponent<CompletionManager>();

        statusText.text = "Nothing is in the bin.";
    }

    // Update is called once per frame
    void Update()
    {
        // if all valid objects are in the bin (thus equaling the amount of objects needed for the bin), and if there are no invalid objects inside, then it counts.
        if (goodCount == objectsNeeded && badCount == 0)
        {
            shouldCount = true;
        }

        else
        {
            shouldCount = false;
        }

        // if the shouldCount boolean is true, the text above the bin tells the player that the bin has been successfully filled.
        // otherwise, it tells the player how many objects the player still needs to put in/take out from it.
        switch (shouldCount)
        {
            case true:
                statusText.text = "All objects needed are in the bin.\nNo incorrect objects are in the bin.";
                break;
            case false:
                if (badCount > 0)
                {
                    statusText.text = "Correct Objects: " + goodCount + "/" + objectsNeeded + ", Incorrect Objects: " + badCount + "\nThere are still one or more incorrect objects in the bin.";
                }
                else if (badCount == 0)
                {
                    statusText.text = "Correct Objects: " + goodCount + "/" + objectsNeeded + ", Incorrect Objects: " + badCount + "\nThere are not enough objects needed in the bin.";
                }
                break;
        }

        // dev tool to instantly check how many bins are valid/invalid.
        if (Input.GetKeyDown(KeyCode.V))
        {
            completionManager.CheckBins();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // if the object going inside the bin is tagged with "Pickup," this method determines whether or not the object's collider is meant to go inside it.
        // if the object's collider matches any of the colliders listed in the goodColliderList, then it counts as a valid object, increasing the goodCount integer by 1.
        // if the object's collider doesn't match any of the colliders listed in the goodColliderList, then it counts as an invalid object, increasing the badCount integer by 1.
        if (other.tag == "Pickup")
        {
            colliderList.Add(other);

            if (colliderList.Count <= 0)
            {
                statusText.text = "Nothing is in the bin.";
            }

            else
            {
                if (goodColliderList.Contains(other))
                {
                    Debug.Log("good in box: " + other.name);

                    goodCount++;
                }

                else
                {
                    Debug.Log("bad in box: " + other.name);

                    badCount++;
                }
            }

            Invoke("DoCheck", 0.1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // does the same as the above method, except for when colliders are moving out of the bin.
        // if the object's collider matches any of the colliders listed in the goodColliderList, then it counts as a valid object, decreasing the goodCount integer by 1.
        // if the object's collider doesn't match any of the colliders listed in the goodColliderList, then it counts as an invalid object, decreasing the badCount integer by 1.
        if (other.tag == "Pickup")
        {
            colliderList.Remove(other);

            if(goodColliderList.Contains(other))
            {
                Debug.Log("good out box: " + other.name);

                goodCount--;
            }

            else
            {
                Debug.Log("bad out box: " + other.name);

                badCount--;
            }

            Debug.Log("other out box");

            Invoke("DoCheck", 0.1f);
        }

        Debug.Log("out box");
    }

    // invokes the CheckBins method for the completion manager.
    public void DoCheck()
    {
        completionManager.CheckBins();
    }
}
