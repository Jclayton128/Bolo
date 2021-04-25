using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MoneyHolder : MonoBehaviour
{
    //init
    public TextMeshProUGUI moneyBar;


    //param

    //hood
    public int money = 0;
    void Start()
    {
        moneyBar = FindObjectOfType<UIManager>().GetMoneyCounter(gameObject);
        UpdateUI();

    }

    public void Reinitialize()
    {
        Start();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetMoneyAmount()
    {
        return money;
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (!moneyBar) { return; }
        moneyBar.text = "$ " + money.ToString();
    }
}
