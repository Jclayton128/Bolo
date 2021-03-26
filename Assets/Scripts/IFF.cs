using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFF : MonoBehaviour
{
    //param
    public int iffAllegiance = 0;

    public void SetIFFAllegiance(int value)
    {
        iffAllegiance = value;
    }

    public int GetIFFAllegiance()
    {
        return iffAllegiance;
    }
}
