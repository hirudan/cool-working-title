using Actor;
using UI.Health;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracks player health and sets player health in PlayerStats
/// </summary>
public class PlayerLiving : Living
{
    public int totalHealth;
    public HealthUi healthBar;
    protected override void Start()
    {
        base.Start();
        this.health = totalHealth;
    }

    public override void TakeDamage(int amt)
    {
        base.TakeDamage(amt);
        healthBar.setUIState(this.health, this.totalHealth);
    }
}
