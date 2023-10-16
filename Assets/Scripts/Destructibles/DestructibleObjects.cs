using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObjects : MonoBehaviour,IDamageable
{
    [SerializeField] Miscellaneous _resource;
    [SerializeField] float _maxLife;
    float _life;

    void Awake()
    {
        _life = _maxLife;
    }
    void AddResources()
    {
        var value = Random.Range(_resource.minValueAdded, _resource.maxValueAdded);
        InventoryManager.InventoryInstance.AddItem(_resource, value);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            var collision = other.gameObject.GetComponent<BaseWeapon>();
            AddResources();
            TakeDamage(collision.NormalDamage);
        }
    }
    public void TakeDamage(float value)
    {
        _life -= value;
        if (_life <= 0) Destroy(this.gameObject);
    }
}
