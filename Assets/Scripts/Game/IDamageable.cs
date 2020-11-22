using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable

{
    void GiveDamage();
    void TakeDamage(float dmg);
    void Die();
}
