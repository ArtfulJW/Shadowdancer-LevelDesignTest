using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StealthBarLight : MonoBehaviour
{
    PlayerInformation player;
    
    public float maximum;
    public float current;
    public float threshold;
    public Image mask;
    public Image fill;
    public Color low;
    public Color high;

    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInformation>();
        current = player.GetLuminance();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        current = player.GetLuminance();
        GetCurrentFill();
    }

    void GetCurrentFill() {

        if (current > maximum)
            current = maximum;
        
        float fillAmount = (float)current / (float)maximum;

        if (current < threshold) {fill.color = low;
            // Debug.Log("Low color " + fill.color);
            }
            
        else {fill.color = high;
            // Debug.Log("High color " + fill.color);
            }
            
            
        //gradually fill the bar
        mask.fillAmount = Mathf.Lerp(mask.fillAmount, fillAmount, Time.deltaTime * 5f);

        // mask.fillAmount = fillAmount;
        
        // fill.color = Color.Lerp(low, high, fillAmount);

                
    }
}
