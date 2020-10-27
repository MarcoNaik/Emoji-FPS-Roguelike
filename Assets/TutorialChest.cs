using System.Collections.Generic;
using Items;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


    public class TutorialChest : Interactable
    {
        public int cost;
        public TextMeshProUGUI textCost;
        public GameObject pistolItem;

        protected void Awake()
        {
            textCost.text = cost.ToString();
        }

        private void Start()
        {
            cost *= FindObjectOfType<LevelManager>().Stage;
        }

        private void Open()
        {
            if (PlayerController.Instance.money<cost) return;
            
            Drop(pistolItem);
            gameObject.tag = "Untagged";
            transform.GetChild(0).gameObject.SetActive(false);
            PlayerController.Instance.RemoveMoney(cost);
        }

        

        void Drop(GameObject item)
        {
            GameObject drop = Instantiate(item);
            drop.transform.position = transform.position + Vector3.up;
        }

        public override void OnInteract()
        {
            Open();
        }
    }
