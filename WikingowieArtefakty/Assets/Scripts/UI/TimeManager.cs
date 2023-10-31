using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public TextMeshProUGUI timeTMP;
    public TextMeshProUGUI dayTMP;

    public TextMeshProUGUI DayChangeTMP;
    public TextMeshProUGUI PreviousDayTMP;
    public TextMeshProUGUI NextDayTMP;

    private float delay = 0.5f;
    int hour = 20;
    int min = 0;
    int day = 1;
    void Start()
    {
        PreviousDayTMP.gameObject.SetActive(false);
        NextDayTMP.gameObject.SetActive(false);
        DayChangeTMP.gameObject.SetActive(false);
        timeTMP.text = hour.ToString() +":"+min.ToString();
        StartCoroutine(Timer());

    }
    //
    void DayChange()
    {
        PreviousDayTMP.gameObject.SetActive(true);
        NextDayTMP.gameObject.SetActive(true);
        DayChangeTMP.gameObject.SetActive(true);

        LeanTween.value(0f, 1f, 1.0f).setEase(LeanTweenType.easeOutQuad)
            .setOnUpdate((float alpha) => UpdateTextAlpha(DayChangeTMP, alpha));

        LeanTween.value(0f, 1f, 1.0f).setEase(LeanTweenType.easeOutQuad)
            .setOnUpdate((float alpha) => UpdateTextAlpha(PreviousDayTMP, alpha));

        LeanTween.value(0f, 1f, 1.0f).setEase(LeanTweenType.easeOutQuad)
            .setOnUpdate((float alpha) => UpdateTextAlpha(NextDayTMP, alpha));
        StartCoroutine(Cooldown());
    }

    private void UpdateTextAlpha(TextMeshProUGUI textObject, float alpha)
    {
        Color color = textObject.color;
        color.a = alpha;
        textObject.color = color;
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(0.5f);
        LeanTween.moveLocalY(PreviousDayTMP.gameObject, -150f, 1f).setEase(LeanTweenType.easeInCirc);
        LeanTween.moveLocalY(NextDayTMP.gameObject, 0, 1f).setEase(LeanTweenType.easeInCirc);

        yield return new WaitForSeconds(0.5f);
        LeanTween.value(1f, 0f, 2.0f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float alpha) => UpdateTextAlpha(PreviousDayTMP, alpha));
        yield return new WaitForSeconds(2.5f);
        LeanTween.value(1f, 0f, 2.0f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float alpha) => UpdateTextAlpha(DayChangeTMP, alpha));
        LeanTween.value(1f, 0f, 2.0f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float alpha) => UpdateTextAlpha(NextDayTMP, alpha));
        yield return new WaitForSeconds(2f);
        PreviousDayTMP.gameObject.SetActive(false);
        NextDayTMP.gameObject.SetActive(false);
        DayChangeTMP.gameObject.SetActive(false);
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
                DayChange();
                dayTMP.text = "day " + day.ToString();
            }

            while (temp < 2)
            {
                yield return new WaitForSeconds(delay);
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
