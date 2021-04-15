using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IFF : MonoBehaviour
{
    //init
    SpriteRenderer flagSR;
    AllegianceManager am;
    Image flagImage;

    //param
    int iffAllegiance;
    public bool isPlayer = false;

    private void Start()
    {
        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer sr)) //units don't have allegiance overtop?
        {
            if (GetComponent<ControlSource>()) { return; }  //things with a control source don't have an allegiance flag.
            flagSR = sr;
        }

        am = FindObjectOfType<AllegianceManager>();
        if (isPlayer)
        {
            flagImage = GameObject.FindGameObjectWithTag("OwnFlag").GetComponent<Image>();
            iffAllegiance = am.playerAllegiance;
            flagImage.sprite = am.GetFlagOfAllegiance(iffAllegiance);
        }
        SetFlag();
    }

    public void SetIFFAllegiance(int value)
    {
        iffAllegiance = value;
        SetFlag();
        //Debug.Log(gameObject.name + " is now aligned with: " + iffAllegiance);
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
