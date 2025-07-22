using System.Collections.Generic;
using System.Linq;

public class GOAPState
{
    public WorldModel worldModel;
    public GOAPAction generatingAction = null;
    public int step = 0;

    #region CONSTRUCTOR
    public GOAPState(WorldModel model, GOAPAction gen = null)
    {
        worldModel = model;
        generatingAction = gen;
        this.step = 0;
    }

    public GOAPState(GOAPState source, GOAPAction gen = null)
    {
        //foreach (var elem in source.values)
        //{
        //    if (values.ContainsKey(elem.Key))
        //        values[elem.Key] = elem.Value;
        //    else
        //        values.Add(elem.Key, elem.Value);
        //}
        //generatingAction = gen;
        this.worldModel = source.worldModel.Clone();
        this.generatingAction = gen;
        this.step = source.step;
    }
    #endregion

    public override bool Equals(object obj) =>
    obj is GOAPState other &&
    new[] {
            worldModel.life == other.worldModel.life,
            worldModel.alive == other.worldModel.alive,
            worldModel.weapon == other.worldModel.weapon,
            worldModel.arrows == other.worldModel.arrows,
            worldModel.distanceToPlayer == other.worldModel.distanceToPlayer
    }.All(b => b);
    public override int GetHashCode() =>
    new[] {
        worldModel.life.GetHashCode(),
        worldModel.alive.GetHashCode(),
        worldModel.weapon.GetHashCode(),
        worldModel.arrows.GetHashCode(),
        worldModel.distanceToPlayer.GetHashCode()
    }.Aggregate(17, (acc, h) => acc * 31 + h);

    public override string ToString() =>
    new[] {
        $"Alive: {worldModel.alive}",
        $"Life: {worldModel.life}",
        $"Weapon: {worldModel.weapon}",
        $"Arrows: {worldModel.arrows}",
        $"DistanceToPlayer: {worldModel.distanceToPlayer}"
    }.Aggregate($"---> {generatingAction?.name ?? "NULL"}\n", (acc, line) => acc + line + "\n");

}
