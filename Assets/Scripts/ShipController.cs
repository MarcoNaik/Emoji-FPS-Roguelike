using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using TMPro;
using UnityEngine;

public class ShipController : Interactable
{
    
    public int cost;
    public TextMeshProUGUI textCost;
    public override void OnInteract()
    {
        if (PlayerController.Instance.money<cost) return;
        
        FindObjectOfType<EnemySpawnManager>().enabled = true;
        
        PlayerController.Instance.RemoveMoney(cost);
        
        LevelManager.Instance.LoadNextLevel();
        
    }

    private void Start()
    {
        cost *= FindObjectOfType<LevelManager>().Stage;
        
        textCost.text = cost.ToString();
    }

}
