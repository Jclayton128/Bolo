using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MoneyHolder : MonoBehaviour
{
    //init
    TextMeshProUGUI moneyBar;


    //param

    //hood
    public int money = 0;
    void Start()
    {
       if (transform.root.GetComponentInChildren<IFF>().isPlayer == true)
        {
            moneyBar = GameObject.FindGameObjectWithTag("MoneyBar").GetComponent<TextMeshProUGUI>();
            moneyBar.text = "$ " + money.ToString();
        }
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
        if (moneyBar)
        {
            moneyBar.text = "$ " +money.ToString();
        }
    }
}
