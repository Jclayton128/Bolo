using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFF : MonoBehaviour
{
    //init
    SpriteRenderer flagSR;
    AllegianceManager am;

    //param
    public int iffAllegiance = 0;

    private void Start()
    {
        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer sr)) //units don't have allegiance overtop?
        {
            flagSR = sr;
        }
        am = FindObjectOfType<AllegianceManager>();
        SetFlag();
    }

    public void SetIFFAllegiance(int value)
    {
        iffAllegiance = value;
        SetFlag();
    }

    public int GetIFFAllegiance()
    {
        return iffAllegiance;
    }
    private void SetFlag()
    {
        if (!flagSR) { return; }
        flagSR.sprite = am.GetFlagOfAllegiance(iffAllegiance);
    }
}
