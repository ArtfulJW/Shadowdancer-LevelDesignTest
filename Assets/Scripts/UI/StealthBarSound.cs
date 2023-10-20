using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StealthBarSound : MonoBehaviour
{
    PlayerInformation player;
    
    public int maximum;
    public int current;
    public int warnThreshold;
    public int threshold;
    public Image mask;
    public Image fill;
    public Color low;
    public Color mid;
    public Color high;

    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInformation>();
        current = player.GetSoundIntensity();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        current = player.GetSoundIntensity();
        GetCurrentFill();
    }

    void GetCurrentFill() {

        if (current > maximum)
            current = maximum;
        
        float fillAmount = (float)current / (float)maximum;

        if (current < warnThreshold)
            fill.color = low;
        else if (current <= warnThreshold)
            fill.color = mid;
        else
            fill.color = high;
            
        //gradually fill the bar
        mask.fillAmount = Mathf.Lerp(mask.fillAmount, fillAmount, Time.deltaTime * 5f);

        // mask.fillAmount = fillAmount;
        
        // fill.color = Color.Lerp(low, high, fillAmount);

                
    }
}
