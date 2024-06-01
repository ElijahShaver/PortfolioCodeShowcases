using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleTruckSpawnerScript : MonoBehaviour
{
    public List<GameObject> FrontSpawns;
    public List<GameObject> MiddleSpawns;
    public List<GameObject> BackSpawns;

    public GameObject spinnyTruck;

    public float frontTimer;
    public float middleTimer;
    public float backTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // every timer is based on real time seconds.
        frontTimer += Time.deltaTime;
        middleTimer += Time.deltaTime;
        backTimer += Time.deltaTime;

        // if the timers reach 1 second, then a special version of the truck instantiates and the timer resets.
        // the truck is instantiated at a random object's place from the list of GameObjects in each spawner column, complete with randomized movement speeds and rotations for more diversity.
        if (frontTimer >= 1)
        {
            frontTimer = 0;

            int F = Random.Range(0, FrontSpawns.Count);

            Debug.Log("chose " + F + " as desired spawnpoint for front");

            Instantiate(spinnyTruck, new Vector3(FrontSpawns[F].gameObject.transform.position.x, FrontSpawns[F].gameObject.transform.position.y, FrontSpawns[F].gameObject.transform.position.z), FrontSpawns[F].gameObject.transform.rotation);
        }

        if (middleTimer >= 1)
        {
            middleTimer = 0;

            int M = Random.Range(0, MiddleSpawns.Count);

            Debug.Log("chose " + M + " as desired spawnpoint for middle");

            Instantiate(spinnyTruck, new Vector3(MiddleSpawns[M].gameObject.transform.position.x, MiddleSpawns[M].gameObject.transform.position.y, MiddleSpawns[M].gameObject.transform.position.z), MiddleSpawns[M].gameObject.transform.rotation);
        }

        if (backTimer >= 1)
        {
            backTimer = 0;

            int B = Random.Range(0, BackSpawns.Count - 1);

            Debug.Log("chose " + B + " as desired spawnpoint for back");

            Instantiate(spinnyTruck, new Vector3(BackSpawns[B].gameObject.transform.position.x, BackSpawns[B].gameObject.transform.position.y, BackSpawns[B].gameObject.transform.position.z), BackSpawns[B].gameObject.transform.rotation);
        }
    }
}
