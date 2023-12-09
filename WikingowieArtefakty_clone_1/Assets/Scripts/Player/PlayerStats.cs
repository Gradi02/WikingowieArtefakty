using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using Unity.Netcode;

public class PlayerStats : NetworkBehaviour
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
    public GameObject healthBar;
    /// /////////////////////////////////


    private void Start()
    {
        PPV = GameObject.FindGameObjectWithTag("ppv").GetComponent<PostProcessVolume>();
    }
    void FixedUpdate()
    {
        if (!IsOwner) return;

        if (PPV.profile.TryGetSettings(out Vig))
        {
            Vig.intensity.value = normalizedValue;
        }

        if (PPV.profile.TryGetSettings(out ColorGrade))
        {
            ColorGrade.saturation.value = 0 - normalizedValue * 100;
        }

        if (HpSlider != null && HpText != null)
        {
            normalizedValue = 1f - ( Health / MaxHealth);

            HpSlider.value = Health;
            HpText.text = Health.ToString("F0");

            if (Health < 0) Health = 0;
            if (Health > MaxHealth) Health = MaxHealth;

            Health += 0.02f;
        }
    }


    public void SetHealth()
    {
        HpSlider.minValue = 0;
        HpSlider.maxValue = MaxHealth;
    }


    [ContextMenu("pp")]
    public void HEALTHMINUS()
    {
        Health -= 5;       
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

    public void SetHPinfo(GameObject hb)
    {
        healthBar = hb;
        HpSlider = hb.GetComponent<Slider>();
        HpText = hb.transform.Find("value").GetComponent<TextMeshProUGUI>();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SyncBarNicksServerRpc()
    {
        SyncBarNicksClientRpc(GetComponent<NetworkObject>().NetworkObjectId, GetComponent<PlayerInfo>().GetNickname());
    }

    [ClientRpc]
    void SyncBarNicksClientRpc(ulong id, string nick)
    {
        NetworkManager.Singleton.SpawnManager.SpawnedObjects[id].GetComponent<PlayerStats>().SetNick(nick);
    }

    public void SetNick(string n)
    {
        healthBar.GetComponent<HealthController>().SetName(n);
    }
}
