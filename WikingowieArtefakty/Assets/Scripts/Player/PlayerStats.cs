using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public float Health = 20;
    public float MaxHealth = 20;
    public float Speed = 1;

    public int damage = 3;

    public float DashCooldown = 3;
    public float DashPower = 5;

    public float axeCooldown = 1;
    public float BowCooldown = 1;

    public float ChoppingSpeed = 1;
    public float MiningSpeed = 1;

    public float AxeCrit = 5;
    public float BowCrit = 5;
    public float ChoppingCrit = 0;
    public float MiningCrit = 0;

    public float ChanceOfRandomOre = 0;
    public int PotionHp = 20;

    /// ///////////////////////////////
    public PostProcessVolume PPV;
    public Slider HpSlider;
    public TextMeshProUGUI HpText;
    private Vignette Vig;
    private ColorGrading ColorGrade;

    /// /////////////////////////////////
    float normalizedValue;
    /// /////////////////////////////////
    public GameObject button;
    /// /////////////////////////////////
    private void Start()
    {
        SetHealth();
    }
    void FixedUpdate()
    {
        normalizedValue = 1f - ( Health / MaxHealth);

        HpSlider.value = Health;
        HpText.text = Health.ToString("F0");

        if(PPV.profile.TryGetSettings(out Vig))
        {
            Vig.intensity.value = normalizedValue;
        }

        if (PPV.profile.TryGetSettings(out ColorGrade))
        {
            ColorGrade.saturation.value = 0 - normalizedValue * 100;
        }



        if (Health<0) Health = 0;
        if(Health > MaxHealth) Health = MaxHealth;

        Health += 0.02f;
    }


    public void SetHealth()
    {
        HpSlider.minValue = 0;
        HpSlider.maxValue = MaxHealth;
    }

    public void HEALTHMINUS()
    {
        Health -= 1;
        
    }

    public void HpPotion()
    {
        if (Health < MaxHealth)
        {
            Health += PotionHp;
            if(Health > MaxHealth) Health = 20;
        }
    }

    public float GetHealth()
    {
        return Health;
    }
}
