using UnityEngine;

public class TowerUpgrade
{
    // The tower's price
    public int Price { get; private set; }
    // The tower's damage
    public float Damage { get; private set; }
    // The tower's debuff duration
    public float DebuffDuration { get; private set; }

    // The proc chance of the debuff
    public float ProcChance { get; private set; }
    // The ice tower's slowing factor
    public int SlowFactor { get; private set; }
    // How often will the debuff tick
    public float TickTime { get; private set; }
    // How often will the debuff tick
    public float TickDmg { get; private set; }
    // Special damage
    public float SpecialDamage { get; private set; }

    // Constructor used by Morty and Sauce
    public TowerUpgrade(int price, float damage, float debuffduration, float tickTime, float tickDmg, float splashDamage)
    {
        this.Price = price;
        this.Damage = damage;
        this.DebuffDuration = debuffduration;
        this.TickTime = tickTime;
        this.TickDmg = tickDmg;
        this.SpecialDamage = splashDamage;
    }

    // Constructor used by the PicklRick
    public TowerUpgrade(int price, float damage, float debuffduration, float tickTime, float tickDmg)
    {
        this.Price = price;
        this.Damage = damage;
        this.DebuffDuration = debuffduration;
        this.TickTime = tickTime;
        this.TickDmg = tickDmg;
    }

    /// Constructor used by Sauce
    public TowerUpgrade(int price, float damage, float debuffduration, float tickTime, int slowingFactor)
    {
        this.Price = price;
        this.Damage = damage;
        this.DebuffDuration = debuffduration;
        this.TickTime = tickTime;
        this.SlowFactor = slowingFactor;
    }

    /// Constructor used by the Spaceship
    public TowerUpgrade(int price, float damage, float debuffduration, float procChance)
    {
        this.Price = price;
        this.Damage = damage;
        this.DebuffDuration = debuffduration;
        this.ProcChance = procChance;
    }
}
   
