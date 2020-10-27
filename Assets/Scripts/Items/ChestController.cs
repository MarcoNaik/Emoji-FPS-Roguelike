using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Items
{
    public class ChestController : Interactable
    {
        private DropsManager dropsManager;
        public int cost;
        public TextMeshProUGUI textCost;

        protected void Awake()
        {
            dropsManager = FindObjectOfType<DropsManager>();
            
        }

        private void Start()
        {
            cost *= FindObjectOfType<LevelManager>().Stage;
            textCost.text = cost.ToString();
        }

        private bool open = false;
        private void Open()
        {
            if (open || PlayerController.Instance.money<cost) return;
            GameObject drop;
            if (Random.value > 0.9)
            {
                if (dropsManager.guns.Count > 0)
                {
                    drop = PickFrom(dropsManager.guns);
                    dropsManager.guns.Remove(drop);
                }
                else
                {
                    drop = PickFrom(dropsManager.items);
                }
            }
            else
            {
                drop = PickFrom(dropsManager.items);
            }
        
            Drop(drop);
            open = true;
            gameObject.tag = "Untagged";
            transform.GetChild(0).gameObject.SetActive(false);
            PlayerController.Instance.RemoveMoney(cost);
        }

        GameObject PickFrom(List<GameObject> pool)
        {
            return pool[Random.Range(0, pool.Count-1)];
        }

        void Drop(GameObject item)
        {
            GameObject drop = Instantiate(item);
            drop.transform.position = transform.position + Vector3.up;
        }

        public override void OnInteract()
        {
            Debug.Log("chest interacting");
            Open();
        }
    }
}
