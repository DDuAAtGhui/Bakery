using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [Header("Money")]
    [SerializeField] TextMeshProUGUI moneyText;

    private void Update()
    {
        moneyText.text = GameManager.instance.playerMoney.ToString();
    }
}
