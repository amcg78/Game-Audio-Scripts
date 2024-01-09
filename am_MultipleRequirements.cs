using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script must be used in tandem with am_RequireMultipleToProgress. This script must be attached to the 4 collection objects.
/// If another object is collected, this object will be harm the player until the picked up object is 'scanned' into the tracker object.
/// </summary>
public class am_MultipleRequirements : MonoBehaviour
{
    //Public Variables
    public GameObject ObjectTracker;
    public GameObject healthTracker;

    public AK.Wwise.Event pickUp;

    //Private Variables
    private bool oneItemCollected = true; //unlock this once this part of the story is reached
    private bool healthInteraction = true;
    private float timeHurt = 0f;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        //after 5 seconds the player may hurt themselves again
        if (healthInteraction == false)
        {
            timeHurt += 0.02f;

            if (timeHurt >= 3f)
            {
                timeHurt = 0f;
                healthInteraction = true;
            }
        }
    }
    
    //Turn off interactivity
    public void OtherObjectPickedUp()
    {
        oneItemCollected = true;
    }

    //Turn on interactivity. Must be triggered when this part of game is unlocked.
    public void OtherObjectTried()
    {
        oneItemCollected = false;
    }
    
    //Destroy and collect object if allowed. If not, harm player and wait 5 seconds until harming again.
    void OnTriggerEnter()
    {
        //if in freeplay (nothing needing scanned), this object will be collected and thus destroyed.
        if (oneItemCollected == false)
        {
            ObjectTracker.gameObject.GetComponent<am_RequireMultipleToProgress>().UpdateNumberOfCollectedObjects(); //tell tracker object is collected
            AkSoundEngine.SetState("ObjectCollection", "Needs_New");                                   //tell Wwise object is being collected (out of freeplay)
            pickUp.Post(gameObject);                                                                   //play picking up sound
            gameObject.SetActive(false);                                                               //turn off object


        }
        else
        {
            //if it is 5 seconds since the last one...
            if (healthInteraction == true)
            {
                //Play harming sound
                AkSoundEngine.SetState("ObjectCollection", "Already_Wearing"); 
                pickUp.Post(gameObject);

                //player hurts themself
                healthTracker.gameObject.GetComponent<HealthTracker>().HealthDrop();
                healthInteraction = false;
            }
        }
    }
}
