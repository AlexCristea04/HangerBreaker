using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;

    public void setTime(float time) {  
        slider.value = time; 
    }

}
