using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCalculator : MonoBehaviour
{

    [SerializeField]
    public Light light;

    private PlayerInformation player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInformation>();
    }

    // Update is called once per frame
    void Update()
    {
        /* 
         * Testing API Call (GameManager.removeLight), employing loose coupling such that player can call however they like
         * Remove testing Feature after Player is capable to sending API Call to Remove a light.
         * Testing Conclusion: Feature works as expected.
         */
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Please Remove LightID: " + GameManager.Instance.getLights()[0].gameObject.GetInstanceID().ToString());
            GameManager.Instance.removeLight(GameManager.Instance.getLights()[0]);
        }

        /* 
         * Testing API Call (GameManager.disableLight), employing loose coupling such that player can call however they like
         * Remove testing Feature after Player is capable to sending API Call to Disable a light.
         * Testing Conclusion: Feature works as expected.
         */
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Please Disable LightID: " + GameManager.Instance.getLights()[0].gameObject.GetInstanceID().ToString());
            GameManager.Instance.disableLight(GameManager.Instance.getLights()[0]);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Ray ray = new Ray(transform.position, calculateDirection(transform.position, other.transform.position));
            RaycastHit raycastHit;
            Physics.Raycast(ray, out raycastHit);

            if (raycastHit.collider.gameObject.tag == "Player")
            {
                Debug.DrawLine(transform.position, other.transform.position, Color.red);
                // Debug.Log(Mathf.InverseLerp(0, 1, calculateBrightness(light, other.transform.position)));
                player.SetLuminance(Mathf.InverseLerp(0, 1, calculateBrightness(light, other.transform.position)));
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player")
            player.SetLuminance(0f);
    }

    /*
     * Calculates the Brightness of a position as lit by a light.
     * Parameters:
     * Luminence
     * Distance (from Light)
     * 
     * Returns: Brightness of GivenPosition, lit by givenLight
     */
    public float calculateBrightness(Light givenLight, Vector3 PlayerPosition)
    {
        return (givenLight.intensity / (4 * Mathf.PI * Mathf.Pow(calcDistFromLight(givenLight.transform.position, PlayerPosition), 2)));
    }

    /*
     * Calculates the distance from 2 points in space.
     * Parameters:
     * 1. Light 
     * 2. Given Position (Vector3)
     * 
     * Returns:
     * Float: Distance between the Given Position (Vector3) to the Light.
     */
    public float calcDistFromLight(Vector3 LightPosition, Vector3 GivenPosition)
    {
        return (Vector3.Distance(GivenPosition, LightPosition)/light.range);
    }

    public RaycastHit raycastCheck(Ray ray, Collider other)
    {
        ray = new Ray(this.transform.position, other.transform.position);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        return hit;
    }
    
    public Vector3 calculateDirection(Vector3 pointA, Vector3 pointB)
    {
        return (pointB - pointA).normalized;
    }

}
