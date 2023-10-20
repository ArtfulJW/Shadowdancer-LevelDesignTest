using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class InstantiateLight : MonoBehaviour
{

    //1. Be able to calculate brightness from a point of light to get the 0 to 1 clamped value
    //2. Programatically add Lights to GameManager Singleton
    //3. (Level Designer Requirements)
    //    a.Level Designer simply places these assets
    //    b.Level Designer is able to easily modify this prefab light's brightness
    //4. Add a "light" Tag

    // Need a reference to the light we add later programatically.
    [SerializeField]
    Light light;

    SphereCollider lightSphereCollider;

    GameObject instantiatedPrefab;

    [SerializeField]
    float minimumRange;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        light.range = minimumRange;
        light.intensity = minimumRange;
        lightSphereCollider = light.GetComponent<SphereCollider>();
        lightSphereCollider.radius = minimumRange;

        // Instantiate Light at LightInstatiater_Handler position.
        // Save reference to instantiated GameObject<Light>
        light = Instantiate(light, this.transform.position, Quaternion.identity);

        // Add light to GameManager Singleton
        GameManager.Instance.addLight(light);
    }

    // Update is called once per frame
    void Update()
    {
        
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
        return (light.intensity / (4 * Mathf.PI * Mathf.Pow(calcDistFromLight(givenLight.transform.position, PlayerPosition), 2)));
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
        return (Vector3.Distance(GivenPosition, LightPosition));
    }

}
