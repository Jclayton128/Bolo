using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseHolder : MonoBehaviour
{
    //init
    private UIManager uim;

    //param

    //hood
    public int numberOfHouses = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.root.tag != "Player") { return; }
        uim = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DecrementHouseCount()
    {
        numberOfHouses--;
        uim.houseCounter.text = numberOfHouses.ToString();
    }

    public void IncrementHouseCount()
    {
        numberOfHouses++;
        uim.houseCounter.text = numberOfHouses.ToString();
    }

}
