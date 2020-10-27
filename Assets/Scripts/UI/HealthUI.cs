using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    private TextMeshProUGUI text;
    public bool money;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        text.text = money ? PlayerController.Instance.Money.ToString() : PlayerController.Instance.GetHp().ToString();
    }
}
