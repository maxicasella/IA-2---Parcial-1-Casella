using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantInteraction : MonoBehaviour
{
    [SerializeField] MerchantSystem _merchant;
    void OnMouseDown()
    {
        _merchant.EnableCanvas();
    }
}
