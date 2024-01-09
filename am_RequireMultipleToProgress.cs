using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This script runs a 'scanner' which required 4 objects to be collected 1 at a time and scanned to progress.
/// If the object is interacted with before collecting another object, no progression occurs. 
/// Objects must be collected and scanned one at a time as in tandem with am_MultipleRequirements.
/// </summary>
public class am_RequireMultipleToProgress : MonoBehaviour
{
    //private variables
    private float collectedObjects = 1;      //tracks how many objects collected
    private bool repeatedTry = false;   //tracks whether the player is scanning the same object
    bool finished = false;              //used to start the next cutscene and turn the object 'off' before the audio is finished
    bool active = true;                 //turns the object on or off
    bool musicTriggered = false;        //triggers music only once at the end of the cutscene

    //audio event
    public AK.Wwise.Event scan;          //audio to play when 'scanning' of object is occuring

    //Public variables

    //game objects to collect
    public GameObject collection1;          
    public GameObject collection2;
    public GameObject collection3;
    public GameObject collection4;

    public GameObject GlobalTracker; //object that tracks game progression

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Allows use of the scanner once triggered
    public void UpdateNumberOfCollectedObjects()
    {
        collectedObjects += 1;
        repeatedTry = false; //player may scan object

        //turns off collection objects to force player to pick them up one at a time
        collection1.gameObject.GetComponent<am_MultipleRequirements>().OtherObjectPickedUp();
        collection2.gameObject.GetComponent<am_MultipleRequirements>().OtherObjectPickedUp();
        collection3.gameObject.GetComponent<am_MultipleRequirements>().OtherObjectPickedUp();
        collection4.gameObject.GetComponent<am_MultipleRequirements>().OtherObjectPickedUp();
    }

    //Main Code
    void OnTriggerEnter()
    {
        if (active == true)
        {
            //Player may scan object once and then will not be able to.
            if (repeatedTry == false)
            {
                if (collectedObjects == 1)
                {
                    AkSoundEngine.SetState("ObjectCollection", "Has_1");

                    repeatedTry = true;

                }

                if (collectedObjects == 2)
                {
                    AkSoundEngine.SetState("ObjectCollection", "Has_2");

                    repeatedTry = true;

                }

                if (collectedObjects == 3)
                {
                    AkSoundEngine.SetState("ObjectCollection", "Has_3");

                    repeatedTry = true;
                }

                if (collectedObjects == 4)
                {
                    AkSoundEngine.SetState("ObjectCollection", "Has_4");

                    repeatedTry = true;

                }

                if (collectedObjects == 5)
                {
                    AkSoundEngine.SetState("ObjectCollection", "Has_All");
                    AkSoundEngine.SetState("Music", "None");
                    repeatedTry = true;
                    

                }

                //trigger audio event upon scanning as long as this progression isn't completed
                if (finished == false)
                {
                    active = false; //don't allow repeated scanning until audio is finished
                    CutSceneProgress.gameObject.GetComponent<am_Destroy_Multiple>().ForceOn();
                    scan.Post(gameObject, (uint)AkCallbackType.AK_EndOfEvent, EventCallbackOne, this);

                }

            }
            else
            {
                //event if the same object is tried
                AkSoundEngine.SetState("ObjectCollection", "Needs_New");
                scan.Post(gameObject);
            }
        }
    }

    //events to occur after audio is finished
    private void EventCallbackOne(object in_cookie, AkCallbackType in_type, object in_callbackInfo)

    {

        if (in_type == AkCallbackType.AK_EndOfEvent)

        {
            //once all objects are collected, tell progression tracker object and don't allow interaction with scanner
            if (numberOfIDs == 5)
            {
                GlobalTracker.gameObject.GetComponent<am_Destroy_Multiple>().CollectionFinished();
                finished = true; //progression is completed
            }
            else { active = true; } //if not, allow scanner to be interacted with again

            //allows other collection objects to be picked up
            collection1.gameObject.GetComponent<am_collectMultiple>().OtherObjectTried();
            collection2.gameObject.GetComponent<am_collectMultiple>().OtherObjectTried();
            collection3.gameObject.GetComponent<am_collectMultiple>().OtherObjectTried();
            collection4.gameObject.GetComponent<am_collectMultiple>().OtherObjectTried();

            //trigger music when first interacted with ONLY.
            if (musicTriggered == false)
            {
                AkSoundEngine.SetState("Music", "Investigating");
                musicTriggered = true;
            }
        }

    }


  
}
