using System;
using UnityEngine;

public class EnemyController : UnitBehaviour
{

    [SerializeField]
    private float movementSpeed;
    
    [SerializeField]
    private int damage;

    [SerializeField]
    private int unitMoney;

    private Vector3 movement;
    
    public HealthBar healthBar;
    
    public float attackColdown;
    
    private float coldownCounter;

    private bool aggro = false;
    private bool spawned = false;

    public void SetAggro()
    {
        aggro = true;
    }


    private void Awake()
    {
        coldownCounter = 0f;
    }

    public void Set(float spd, int dmg, int cash, int hp)
    {
        movementSpeed = spd ;
        damage = dmg;
        unitMoney = cash;
        MaxHp = hp;
        CurrentHp = MaxHp;
        aggro = true;
        spawned = true;
    }

    protected override void Start()
    {
        base.Start();
        healthBar.SetMaxHealth(MaxHp);
    }

    void Update()
    {
        healthBar.SetHealth(CurrentHp);
        if (!aggro)
        {
            if(Vector3.Distance(PlayerController.Instance.transform.position, transform.position) < 10) aggro = true;
        }
        else
        {
            transform.LookAt(PlayerController.Instance.transform.position);
            transform.position += transform.forward * movementSpeed *Time.deltaTime;
        }
        coldownCounter -= Time.deltaTime;
        
    }

    protected override void Die()
    {
        PlayerController.Instance.AddMoney(unitMoney);
        if (spawned && FindObjectOfType<EnemySpawnManager>() != null)
        {
            FindObjectOfType<EnemySpawnManager>().enemysAlive--;
            
        }
        
        Destroy(gameObject);
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && coldownCounter <= 0f)
        {
            coldownCounter = attackColdown;
            Attack(PlayerController.Instance);
            //rb.MovePosition(rb.position - movement * movementSpeed);
        }
    }


    private void Attack(PlayerController player)
    {
        player.TakeDamage(damage);
    }
}