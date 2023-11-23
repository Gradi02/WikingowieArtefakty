using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class TimeManager : NetworkBehaviour
{
    public TextMeshProUGUI timeTMP;
    public TextMeshProUGUI dayTMP;

    public TextMeshProUGUI DayChangeTMP;
    public TextMeshProUGUI PreviousDayTMP;
    public TextMeshProUGUI NextDayTMP;

    public TextMeshProUGUI LandTMP;
    public TextMeshProUGUI DescTMP;
    public TextMeshProUGUI ClockTMP;
    public TextMeshProUGUI AccDayTMP;

    public Transform Sun;
    public GameObject Player;

    private float delay = 1f;
    //int hour = 8;
    //int min = 0;
    //int day = 1;

    private NetworkVariable<int> n_hour = new NetworkVariable<int>(8, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<int> n_min = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<int> n_day = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private bool paused = false; 

    //public float fogDensity;
    void Start()
    {
        //fogDensity = RenderSettings.fogDensity;
        PreviousDayTMP.gameObject.SetActive(false);
        NextDayTMP.gameObject.SetActive(false);
        DayChangeTMP.gameObject.SetActive(false);
        SetTimeDataClientRpc();

        LandTMP.gameObject.transform.localPosition = new Vector3(0, +100, 0);
        DescTMP.gameObject.transform.localPosition = new Vector3(0, +150, 0);
        ClockTMP.gameObject.transform.localPosition = new Vector3(0, +200, 0);
        AccDayTMP.gameObject.transform.localPosition = new Vector3(0, +250, 0);
    }
    
    public void StartDayOne()
    {
        StartCoroutine(StartTransInfo());

        if (IsServer)
        {
            LeanTween.rotateAround(Sun.gameObject, Vector3.right, 360, ((delay * 48) - 0.01f));
            n_hour.Value = 8;
            StartCoroutine(Timer());
        }
    }
    void DayChange()
    {
        PreviousDayTMP.gameObject.SetActive(true);
        NextDayTMP.gameObject.SetActive(true);
        DayChangeTMP.gameObject.SetActive(true);
        PreviousDayTMP.text = (n_day.Value - 1).ToString();
        NextDayTMP.text = n_day.Value.ToString();
        LeanTween.rotateAround(Sun.gameObject, Vector3.right, 360, ((delay * 48) - 0.01f));
        LeanTween.value(0f, 1f, 1.0f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float alpha) => UpdateTextAlpha(DayChangeTMP, alpha));

        LeanTween.value(0f, 1f, 1.0f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float alpha) => UpdateTextAlpha(PreviousDayTMP, alpha));
        StartCoroutine(Cooldown());
    }

    private void Update()
    {
        //pauza
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("KLIKAM PAUZE");
            if (paused == false)
            {
                //Time.timeScale = 0f;
                paused = true;
            }

            else if (paused == true)
            {
                //Time.timeScale = 1f;
                paused = false;
            }
        }
    }


    private void UpdateTextAlpha(TextMeshProUGUI textObject, float alpha)
    {
        Color color = textObject.color;
        color.a = alpha;
        textObject.color = color;
    }

    IEnumerator StartTransInfo()
    {
        yield return new WaitForSeconds(0.2f);
        LeanTween.moveLocalY(AccDayTMP.gameObject, -250f, 0.5f).setEase(LeanTweenType.easeInCirc);
        yield return new WaitForSeconds(0.2f);
        LeanTween.moveLocalY(ClockTMP.gameObject, -198f, 0.5f).setEase(LeanTweenType.easeInCirc);
        yield return new WaitForSeconds(0.2f);
        LeanTween.moveLocalY(DescTMP.gameObject, -150f, 0.5f).setEase(LeanTweenType.easeInCirc);
        yield return new WaitForSeconds(0.2f);
        LeanTween.moveLocalY(LandTMP.gameObject, -100f, 0.5f).setEase(LeanTweenType.easeInCirc);
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(0.5f);
        LeanTween.moveLocalY(PreviousDayTMP.gameObject, -150f, 1f).setEase(LeanTweenType.easeInCirc);//easeinsine
        LeanTween.moveLocalY(NextDayTMP.gameObject, 0, 1f).setEase(LeanTweenType.easeInCirc);
        yield return new WaitForSeconds(0.2f);
        LeanTween.value(0f, 1f, 2.0f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float alpha) => UpdateTextAlpha(NextDayTMP, alpha));
        

        yield return new WaitForSeconds(0.5f);
        LeanTween.value(1f, 0f, 0.5f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float alpha) => UpdateTextAlpha(PreviousDayTMP, alpha));
        yield return new WaitForSeconds(2.5f);
        LeanTween.value(1f, 0f, 1.0f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float alpha) => UpdateTextAlpha(DayChangeTMP, alpha));
        LeanTween.value(1f, 0f, 1.0f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float alpha) => UpdateTextAlpha(NextDayTMP, alpha));
        yield return new WaitForSeconds(2f);
        LeanTween.moveLocalY(PreviousDayTMP.gameObject, 0f, 0.1f).setEase(LeanTweenType.easeInCirc);
        LeanTween.moveLocalY(NextDayTMP.gameObject, 150f, 0.1f).setEase(LeanTweenType.easeInCirc);
        yield return new WaitForSeconds(0.1f);
        PreviousDayTMP.gameObject.SetActive(false);
        NextDayTMP.gameObject.SetActive(false);
        DayChangeTMP.gameObject.SetActive(false);
    }
    IEnumerator SunMove()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds((delay-0.01f)/10);
            //RenderSettings.fogDensity += 0.1f;
            //Debug.Log("Fog Density: " + RenderSettings.fogDensity);
            LeanTween.rotateAround(Sun.gameObject, Vector3.right, 360, ((delay*48)-0.01f));
            //Sun.Rotate(Vector3.right, 360f / 480f);
        }
    }

    IEnumerator Timer()
    {
        while (true) {
            int temp = 0;

            if (n_hour.Value > 23)
            {
                n_hour.Value = 0;
                n_day.Value++;
                SetTimeDataServerRpc();
                SetNextDayAnimationServerRpc();
            }

            while (temp < 2)
            {
                yield return new WaitForSeconds(delay);
                temp++;
                n_min.Value += 30;
                if(n_min.Value == 60) n_min.Value = 0;
                SetTimeDataServerRpc();
            }
            n_min.Value = 0;
            n_hour.Value++;
            SetTimeDataServerRpc();
        }
    }

    [ServerRpc]
    void SetTimeDataServerRpc()
    {
        SetTimeDataClientRpc();
    }

    [ClientRpc]
    void SetTimeDataClientRpc()
    {
        timeTMP.text = n_hour.Value.ToString() + ":" + n_min.Value.ToString("D2");
    }

    [ServerRpc]
    void SetNextDayAnimationServerRpc()
    {
        SetNextDayAnimationClientRpc();
    }

    [ClientRpc]
    void SetNextDayAnimationClientRpc()
    {
        DayChange();
        dayTMP.text = "day " + n_day.Value.ToString();
    }
}
