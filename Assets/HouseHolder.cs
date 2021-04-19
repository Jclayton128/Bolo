using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseHolder : MonoBehaviour
{
    //init

    //param

    //hood
    public int numberOfHouses = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DecrementHouseCount()
    {
        numberOfHouses--;
    }

    public void IncrementHouseCount()
    {
        numberOfHouses++;
    }

}
