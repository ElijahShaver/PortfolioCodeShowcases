using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckRotate : MonoBehaviour
{
    public float yRot;
    public float turnSpeed;
    public TruckRotateTrigger currentTruckRotateTrigger;
    public GameplayCountdown gameplayCountdown;

    // Start is called before the first frame update
    void Start()
    {
        gameplayCountdown = GameObject.FindWithTag("Player").GetComponent<GameplayCountdown>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // if the truck encounters a truck rotation trigger, the currentTruckRotateTrigger will get the Y rotation attribute of the trigger it just entered.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TurnTrigger")
        {
            currentTruckRotateTrigger = other.GetComponent<TruckRotateTrigger>();

            yRot = currentTruckRotateTrigger.yRot;
        }
    }

    // if the truck stays inside a truck rotation trigger, the truck will rotate a set amount based off the TruckRotateTrigger script given to the trigger, which is only a single float that says the rotation amount.
    // the bigger the number, the stronger the rotation.
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "TurnTrigger" && gameplayCountdown.hasStarted)
        {
            Vector3 truckVector = new Vector3(this.gameObject.transform.rotation.x, yRot, this.gameObject.transform.rotation.z);

            turnSpeed += Time.deltaTime;

            Debug.Log("speen");

            transform.Rotate(0, yRot * Time.deltaTime, 0);
        }
    }
}
