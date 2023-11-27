using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_AnimEvents : MonoBehaviour
{
    [SerializeField] Animator _myAnim;
    [SerializeField] GameObject _swordCollider;
    [SerializeField] GameObject _slash;
    [SerializeField] AudioSource _audioSlash;
    void FinishAttack()
    {
        _slash.SetActive(false);
        _myAnim.SetBool("Attack", false);
    }
    void EnableSwCollider()
    {
        _audioSlash.Play();
        _slash.SetActive(true);
        _swordCollider.SetActive(true);
    }
    void DisabledSwCollider()
    {
        _swordCollider.SetActive(false);
    }
}
