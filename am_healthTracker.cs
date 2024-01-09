using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is a basic health tracker that can tackle injuries to the player and change sounds concurrently.
/// A health parameter is consistently sent to Wwise to update a parameter that will affect health related sounds (i.e a heartbeat at low health).
/// This script must be called from the object that initiates the injury to the player but will track and replenish to full health health automatically.
/// </summary>
public class HealthTracker : MonoBehaviour
{

    float health = 100f; //start at full health
    public AK.Wwise.RTPC healthParam;
    public AK.Wwise.Event heartbeatOn;
    public AK.Wwise.Event heartbeatOff;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Update the health parameter in Wwise
    void FixedUpdate()
    {
        //Ensure upper boundary
        if (health > 100f)
        {
            health = 100f;
            heartbeatOff.Post(gameObject); //turn off heartbeat at full health
        }
        

        if (health < 100f)
        {
            
            //Ensure lower boundary
            if (health <= 0f) health = 0f;

            //health increases slower if above half health
            if (health > 50f)
            {
                health = health + 0.08f;
                healthParam.SetValue(gameObject, health);
            }
            if (health <= 50f)
            {
                health = health + 1.2f;
                healthParam.SetValue(gameObject, health);
            }
        }
    }

    //drop the health after a collision or injury. Call this from 'harmful' objects.
    public void HealthDrop()
    {
        health = health - 20f; //per injury. Ensure harmful object cannot be interacted with instantly after collision.
        heartbeatOn.Post(gameObject); //start heartbeat sound upon injury.
        healthParam.SetValue(gameObject, health);

    }
}
