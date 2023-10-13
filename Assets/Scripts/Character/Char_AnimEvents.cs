using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_AnimEvents : MonoBehaviour
{
    [SerializeField] Animator _myAnim;
    [SerializeField] GameObject _swordCollider;
    void FinishAttack()
    {
        _myAnim.SetBool("Attack", false);
    }
    void EnableSwCollider()
    {
        _swordCollider.SetActive(true);
    }
    void DisabledSwCollider()
    {
        _swordCollider.SetActive(false);
    }
}
