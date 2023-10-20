using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This class stores all information on the player's state.
I.E. luminance and soundIntensity, while also exposing methods
 to set and get those attributes.
*/
public class PlayerInformation : MonoBehaviour
{

    private float luminance;
    private int soundIntensity;

    private int floorSoundIntensity; // intensity of what the player is moving on

    void Start() {
        luminance = 0.0f;
        soundIntensity = 0;
        floorSoundIntensity = 0;
    }
    
    public float GetLuminance() {
        return luminance;
    }

    public void SetLuminance(float newLuminance) {
        luminance = newLuminance;
        Debug.Log("Player Illumination Set: " + luminance); 
    }
    

    public int GetSoundIntensity()
    {
        return soundIntensity;
    }

    public void SetSoundIntensity(int intensity)
    {
        int sanitizedIntensity = Mathf.Clamp(intensity, 0, 10);
        soundIntensity = sanitizedIntensity;
    }

    public int GetFloorSoundIntensity() {
        return floorSoundIntensity;
    }

    public void SetFloorSoundIntensity(int intensity) {
        floorSoundIntensity = intensity;
    }

    public void SetDominatingSoundIntensity(int movementSoundIntensity) {
        if (movementSoundIntensity > floorSoundIntensity)
            SetSoundIntensity(movementSoundIntensity);
        else 
            SetSoundIntensity(floorSoundIntensity);
    }
}
