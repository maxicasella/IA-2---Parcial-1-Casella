using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeCollider : MonoBehaviour
{
    [SerializeField] Collider _collider;
    public void ActivateCollider()
    {
        _collider.enabled = true;
    }
    public void DesactivatedCollider()
    {
        _collider.enabled = false;
    }
}
