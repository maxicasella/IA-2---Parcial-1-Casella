using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }


    [Header("Components")]
    [SerializeField] GameObject _lossCanvas;
    [SerializeField] Entity _player;
    [SerializeField] CharacterController _controller;

    void Awake()
    {
        if (Instance) Destroy(gameObject);
        else Instance = this;
    }

    public void GameOver()
    {
        if(_player.Life <= 0)
        {
            _controller.enabled = false;
            _lossCanvas.SetActive(true);
        }
    }
}
