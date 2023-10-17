using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantSystem : MonoBehaviour
{
    [SerializeField] GameObject _merchantCanvas;
    [SerializeField] CharacterController _character;

    void Awake()
    {
        _merchantCanvas.SetActive(false);
    }
    public void EnableCanvas()
    {
        _character.enabled = false;
        _merchantCanvas.SetActive(true);
    }
    public void DisabledCanvas()
    {
        _character.enabled = true;
        _merchantCanvas.SetActive(false);
    }
}
