using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnife : MonoBehaviour
{
    [SerializeField] float _dmg;
    [SerializeField] Collider _collider;
    int _attackCounter;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            var player = other.GetComponent<Entity>();
            if (_attackCounter == 0 && player != null)
            {
                _attackCounter++;
                player.TakeDamage(_dmg);
                _collider.enabled = false;
            }
        }
    }

    public void ActivateCollider()
    {
        _attackCounter = 0;
        _collider.enabled = true;
    }
    public void DesactivatedCollider()
    {
        _collider.enabled = false;
    }
}
