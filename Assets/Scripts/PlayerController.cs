using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : UnitBehaviour
{
    public int money=0;

    public int Money => money;

    public float damage =1 ;
    public float hpReg = 1;
    public float critic = 1;
    public float shootSpd = 1;
    
    public static PlayerController Instance { get; protected set; }
 
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    protected override void Start()
    {

        MaxHp = 100;
        CurrentHp = MaxHp;
        StartCoroutine(HealthRegeneration());
    }

    protected override void Die()
    {
        StopAllCoroutines();
        Destroy(FindObjectOfType<DontDestoy>().gameObject);
        Destroy(gameObject);
        SceneManager.LoadScene(0);
    }

    public void AddMoney(int unitMoney)
    {
        money += unitMoney;
    }
    public void RemoveMoney(int cost)
    {
        money -= cost;
    }

    public void AddMaxHpBy(int power)
    {
        MaxHp += power;
    }

    IEnumerator HealthRegeneration()
    {
        yield return new WaitForSeconds(hpReg);

        if(CurrentHp < MaxHp) CurrentHp++;

        StartCoroutine(HealthRegeneration());

    }

    public void HealthRegUpgrade()
    {
        hpReg = hpReg * 0.9f;
    }
    
    public void DamageUpgrade()
    {
        damage += 0.2f;
    }
    public void DefenseUpgrade()
    {
        Defense += 5;
    }
    
    public void ShootSpdUpgrade()
    {
        shootSpd = shootSpd * 0.9f;
    }
    
    public void CriticUpgrade()
    {
        critic = critic * 0.9f;
    }
    
    public void SpdUpgrade()
    {
        GetComponent<PlayerMovement>().moveSpeed++;
    }
    
}
