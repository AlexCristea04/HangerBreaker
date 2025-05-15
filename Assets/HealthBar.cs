using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;

    public void setHealth(int health) { 
        slider.value = health;

        if (slider.value == 1) {
            fill.color = Color.red;
        }
        else {
            fill.color = Color.white;        
        }
    }

    public void setMaxHealth(int newMaxHealth)
    {
        slider.maxValue = newMaxHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

}
