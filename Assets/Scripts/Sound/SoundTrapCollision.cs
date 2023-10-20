using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrapCollision : MonoBehaviour
{

    //Player game object
    public PlayerInformation player;

    public int WaterCollisionSoundIntensity = 5;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInformation>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("Player is in water");
            if (player.GetSoundIntensity() < WaterCollisionSoundIntensity)
                player.SetSoundIntensity(WaterCollisionSoundIntensity);
            // player.walkSound.enabled = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            //Debug.Log("Player is in water");
            if(player.GetSoundIntensity() < WaterCollisionSoundIntensity)
                player.SetSoundIntensity(WaterCollisionSoundIntensity);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // player.walkSound.enabled = true;
            player.SetSoundIntensity(2);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
