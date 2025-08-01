using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    public int maxArrows;
    public float rangeAttackDistance;
    public float meleeAttackDistance;
    public int meleeKnifeDamage;
    public int meleeKickDamage;
    public float distanceToArrows;
    public float distanceToKnife;

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
            maxArrows = this.maxArrows,
            rangeAttackDistance = this.rangeAttackDistance,
            meleeAttackDistance = this.meleeAttackDistance,
            meleeKickDamage = this.meleeKickDamage,
            meleeKnifeDamage = this.meleeKnifeDamage,
            distanceToArrows = this.distanceToArrows,
            distanceToKnife = this.distanceToKnife,
            distanceToPlayer = this.distanceToPlayer
        };
    }
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        WorldModel other = (WorldModel)obj;

        return Mathf.Approximately(life, other.life) &&
               Mathf.Approximately(maxLife, other.maxLife) &&
               alive == other.alive &&
               weapon == other.weapon &&
               arrows == other.arrows &&
               maxArrows == other.maxArrows &&
               Mathf.Approximately(rangeAttackDistance, other.rangeAttackDistance) &&
               Mathf.Approximately(meleeAttackDistance, other.meleeAttackDistance) &&
               meleeKnifeDamage == other.meleeKnifeDamage &&
               meleeKickDamage == other.meleeKickDamage &&
               Mathf.Approximately(distanceToArrows, other.distanceToArrows) &&
               Mathf.Approximately(distanceToKnife, other.distanceToKnife) &&
               Mathf.Approximately(distanceToPlayer, other.distanceToPlayer);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(
            HashCode.Combine(life, maxLife, alive, weapon, arrows, maxArrows, rangeAttackDistance, meleeAttackDistance),
            HashCode.Combine(meleeKnifeDamage, meleeKickDamage, distanceToArrows, distanceToKnife, distanceToPlayer)
        );
    }
}