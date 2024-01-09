using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This code operates a door where an object must be in posession to use (like an ID card). 
/// The required object must be 'scanned' through a checking object to unlock, open and close the door
/// Once interacted with, the door cannot be interacted with for 5 seconds to allow for playback to cease.
/// This script should be used in tandem with am_RequiredObject
/// </summary>

//this code contains elements from Jules Rawlinson's script ISE_FPS_CallbackShowHideObject

public class am_RequirementToDestroy : MonoBehaviour
{
    //private variables
    bool hasRequirement = false;     //has the first ID been collected?
    float pauseTime = 0.0f;      //prevents door opening and shutting instantly

    public GameObject door;          //door

    public bool destroyCheckingObject = false;   //this object
    public bool destroyDoor = false;             //destroys the door completely
    bool interactionAllowed = true;

    public AK.Wwise.Event doorSound;

    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.SetState("Door_State", "RequirementObjectUnobtained");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    void FixedUpdate()
    {
        
        if (interactionAllowed == false) InteractionOff(); //stop interactions for 5 seconds
    }

    //Turn off interaction with the scanning object for 5 seconds
    void InteractionOff()
    {
        pauseTime += 0.02f; // +0.02s per fixed update.

        if (pauseTime >= 5.0f) //door cant open or shut for 5 seconds
        {
            interactionAllowed = true;  //door can now be interacted with
            pauseTime = 0f;
        }
    }

    //allow initial interaction. Triggered from required object script.
    public void updatePosession()
    {
        hasRequirement = true;
    }

    void OnTriggerEnter()
    {
        //can the door be opened?
        if (hasRequirement == true)
        {
            //ensure object collected state is changed
            AkSoundEngine.SetState("ObjectCollection", "True");

            //choose whether door is active or not
            if (door.activeInHierarchy) AkSoundEngine.SetState("Door_State", "Closed");
            else AkSoundEngine.SetState("Door_State", "Open");

            //borrowed from other script
            if (interactionAllowed == true)// this event has not yet been triggered
            {
                // post the event, adding the callback arguments
                doorSound.Post(gameObject, (uint)AkCallbackType.AK_EndOfEvent, EventCallback, this);
                interactionAllowed = false;// this event can't be triggered again
            }
        }
        else //no it cannot open or close
        {
            AkSoundEngine.SetState("Door_State", "RequirementObjectUnobtained");
            AkSoundEngine.SetState("ObjectCollection", "False");
            doorSound.Post(gameObject);
            
        }
        

     
        
    }

    //after door sound
    private void EventCallback(object in_cookie, AkCallbackType in_type, object in_callbackInfo)

    {
        //once open or close sound is finished
        if (in_type == AkCallbackType.AK_EndOfEvent)

        {
            //open or shut door
            if (door.activeInHierarchy)//if it's already active, hide it (or destroy it if required)
            {
                if (destroyDoor == true) Destroy(door);
                else door.SetActive(false);
            }
            else door.SetActive(true);// else it's not active so show it.

            // finally destroy the checking object if required
            if (destroyCheckingObject == true)
            { 
                Destroy(gameObject);
                AkSoundEngine.SetState("Door_State", "Open"); //door is permanently open.
            }
            

            
        }

    }
}
