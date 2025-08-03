using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeDamage : MonoBehaviour
{
    [SerializeField] float _dmg;
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
                _attackCounter = 0;
            }
        }
    }
}
