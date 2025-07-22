using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    None,
    Knife,
    Bow
}
public class WorldModel 
{
    //Vida agente
    public float life;
    public float maxLife;

    //Jugador
    public bool alive;

    //Armas - distancias y daños
    public string weapon;
    public int arrows;
    public float rangeAttackDistance;
    public float meleeAttackDistance;
    public float meleeKnifeDamage;
    public float meleeKickDamage;

    //Distancia jugador
    public float distanceToPlayer;
    public WorldModel Clone()
    {
        return new WorldModel
        {
            life = this.life,
            maxLife = this.maxLife,
            alive = this.alive,
            weapon = this.weapon,
            arrows = this.arrows,
            rangeAttackDistance = this.rangeAttackDistance,
            meleeAttackDistance = this.meleeAttackDistance,
            meleeKickDamage = this.meleeKickDamage,
            meleeKnifeDamage = this.meleeKnifeDamage,
            distanceToPlayer = this.distanceToPlayer
        };
    }
}