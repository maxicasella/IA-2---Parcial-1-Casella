using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] float _shootLife;
    [SerializeField] float _speed;
    [SerializeField] float _lifeTime;
    [SerializeField] float _dmg;

    Rigidbody _rb;
    float _time;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        _time += Time.deltaTime;
        Vector3 movement = transform.forward * _speed;

        _rb.velocity = movement;
        if (_time >= _lifeTime) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            var player = collision.gameObject.GetComponent<Entity>();
            player.TakeDamage(_dmg);
            Destroy(gameObject);
        }
    }
}
