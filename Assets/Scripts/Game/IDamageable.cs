using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for Taking/Giving damage and Die
/// </summary>
public interface IDamageable

{
    void GiveDamage();
    void TakeDamage(float dmg);
    void Die();
}
