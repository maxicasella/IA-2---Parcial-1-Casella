using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObjects : MonoBehaviour,IDamageable
{
    [SerializeField] Miscellaneous _resource;
    [SerializeField] GameObject _hitParticles;
    [SerializeField] AudioSource _audio;
    [SerializeField] float _maxLife;
    float _life;

    void Awake()
    {
        _life = _maxLife;
    }
    public void AddResources()
    {
        var value = Random.Range(_resource.minValueAdded, _resource.maxValueAdded);
        InventoryManager.InventoryInstance.AddItem(_resource, value);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            Instantiate(_hitParticles, transform);
            _audio.Play();
            var collision = other.gameObject.GetComponent<BaseWeapon>();
            collision.Durability(0.5f);
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
