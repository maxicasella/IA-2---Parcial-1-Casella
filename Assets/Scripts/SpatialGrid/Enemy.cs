using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IGridEntity 
{
    
    public event Action<IGridEntity> OnMove;

    [SerializeField] SpatialGrid _spatialGrid;
    [SerializeField] int _maxHp;
    public int hp;
    public int damage;

    public int CurrentLife { get { return hp; } }
    
    public Vector3 Position {
        get => transform.position;
        set => transform.position = value;
    }
    
    public float UpdateUILife()
    {
        float life = hp;
        float maxLife = _maxHp;

        return (life / maxLife);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            var collision = other.gameObject.GetComponent<BaseWeapon>();
            collision.Durability(0.5f);
            TakeDamage(collision.NormalDamage);
        }
    }
    public void TakeDamage(int value)
    {
        hp -= value;
        if (hp <= 0)
        {
            _spatialGrid.Remove(this);
            Destroy(this.gameObject);
        }
    }
}