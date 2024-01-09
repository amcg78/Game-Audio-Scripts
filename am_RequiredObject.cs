using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script describes a collection object that must be acquired before interacting with another object
//This script should be used in tandem with am_RequirementToDestroy
public class am_RequiredObject : MonoBehaviour
{
    //Game Objects
    public GameObject checkingObject;     //the object that holds am_RequirementToDestroy

    //Wwise Event
    public AK.Wwise.Event pickUp;   //sound when the object is picked up

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter()
    {
            //tell the other objects this object has been collected and play the sound
            checkingObject.gameObject.GetComponent<am_RequirementToDestroy>().updatePosession();
            AkSoundEngine.SetState("ObjectCollection", "True");
            pickUp.Post(gameObject);
            Destroy(gameObject);

    }
}
