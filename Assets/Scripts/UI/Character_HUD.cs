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

    float _previousLife;
    float _previuosStamina;
    float _previousDurability;

    public void UpdateUI(float life, float stamina, Tools weapon)
    {
        if (_previousLife != life)
        {
            UpdateLifeBar(life);
            _previousLife = life;
        }

        if (_previuosStamina != stamina)
        {
            UpdateStaminaBar(stamina);
            _previuosStamina = stamina;
        }

        if (_previousDurability != weapon.Durability)
        {
            UpdateWeaponDurability(weapon);
            _previousDurability = weapon.Durability;
        }
    }

    void UpdateLifeBar(float amount)
    {
        _lifeBarFull.fillAmount = amount;
    }
    void UpdateStaminaBar(float amount)
    {
        _staminaBarFull.fillAmount = amount;
    }

    void UpdateWeaponDurability(Tools weapon)
    {
        var amount = weapon.Durability;

        _durabilityWeaponTxt.text = amount.ToString("00");
    }
}
