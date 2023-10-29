using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Time : MonoBehaviour
{
    public TextMeshProUGUI timeTMP;
    public TextMeshProUGUI dayTMP;
    public float delay = 2;
    int hour = 20;
    int min = 0;
    int day = 1;
    void Start()
    {
        timeTMP.text = hour.ToString() +":"+min.ToString();
        StartCoroutine(Timer());
    }



    IEnumerator Timer()
    {
        while (true) { 
            int temp = 0;

            if (hour > 23)
            {
                hour = 0;
                timeTMP.text = hour.ToString() + ":" + min.ToString("D2");
                day++;
                dayTMP.text = "day " + day.ToString();
            }

            while (temp < 2)
            {
                yield return new WaitForSeconds(delay);
                //Debug.Log(min);
                temp++;
                min += 30;
                if(min==60)min= 0;
                timeTMP.text = hour.ToString() + ":" + min.ToString("D2");
            }
            min = 0;
            hour++;
            timeTMP.text = hour.ToString() + ":" + min.ToString("D2");

        }
    }
}
