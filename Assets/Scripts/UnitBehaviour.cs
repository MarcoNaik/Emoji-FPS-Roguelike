using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBehaviour : MonoBehaviour
{
    
    [SerializeField]
    protected int MaxHp;
    protected int CurrentHp;
    protected int Defense = 0;

    public int GetHp()
    {
        return CurrentHp;
    }
    protected virtual void Start()
    {
        CurrentHp = MaxHp;
    }
    

    protected abstract void Die();

    public void TakeDamage(int bulletDamage)
    {
        
        int damage = bulletDamage - Defense;
        if (damage <= 0) return;
        CurrentHp -= damage;
        if (CurrentHp <= 0) Die();
    }
}
