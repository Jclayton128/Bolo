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
    public int iffAllegiance;

    private void Start()
    {
        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
        {
            if (GetComponent<ControlSource>()) { return; }  //things with a control source don't have an allegiance flag.
            flagSR = sr;
        }

        am = FindObjectOfType<AllegianceManager>();
        if (transform.root.tag == "Player")
        {
            flagImage = FindObjectOfType<UIManager>().GetFlag(transform.root.gameObject);
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
