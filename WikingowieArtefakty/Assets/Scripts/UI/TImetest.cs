using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class TImetest : MonoBehaviour
{
    float startTime;
    float currentTime;
    float cooldown;
    int min;
    int hour;
    int temp;

    public TextMeshProUGUI timer;
    void Start()
    {
        startTime = 0;   
        cooldown = 0.5f;
        temp = 0;
        min = 0;
        hour = 8;
    }

    void FixedUpdate()
    {
        timer.text = hour.ToString() +" "+ min.ToString();
        currentTime += 0.5f * Time.deltaTime;
        if(currentTime > cooldown)
        {
            min += 30;
            if(min == 60) min = 0;
            temp++;
            cooldown += 0.5f;
        }

        if(temp == 2)
        {
            temp = 0;
            if(hour==24) hour = 0;
            hour++;
        }
    }


}
