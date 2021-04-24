using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HouseHolder : MonoBehaviour
{
    //init
    TextMeshProUGUI houseCounter;


    //param

    //hood
    public int numberOfHouses = 0;

    // Start is called before the first frame update
    void Start()
    {
        houseCounter = FindObjectOfType<UIManager>().GetHouseCounter(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DecrementHouseCount()
    {
        numberOfHouses--;
        UpdateUI();

    }

    public void IncrementHouseCount()
    {
        numberOfHouses++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (!houseCounter) { return; }
        houseCounter.text = numberOfHouses.ToString();

    }


}
