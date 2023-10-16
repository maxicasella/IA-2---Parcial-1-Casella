using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Character_HUD : MonoBehaviour
{
    [SerializeField] Image _lifeBarFull;
    [SerializeField] Image _staminaBarFull;
    [SerializeField] TextMeshProUGUI _durabilityWeaponTxt;

    public void UpdateUI(float life, float stamina, BaseWeapon weapon)
    {
        UpdateLifeBar(life);
        UpdateStaminaBar(stamina);
        UpdateWeaponDurability(weapon);
    }

    void UpdateLifeBar(float amount)
    {
        _lifeBarFull.fillAmount = amount;
    }
    void UpdateStaminaBar(float amount)
    {
        _staminaBarFull.fillAmount = amount;
    }

    void UpdateWeaponDurability(BaseWeapon weapon)
    {
        var amount = weapon.Durability;

        _durabilityWeaponTxt.text = amount.ToString();
    }
}
