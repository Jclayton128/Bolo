using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthHider : MonoBehaviour
{
    //init
    SpriteRenderer[] srs;
    

    // Start is called before the first frame update
    void Start()
    {
        srs = transform.root.GetComponentsInChildren<SpriteRenderer>();
        MakeObjectInvisible();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MakeObjectInvisible()
    {
        if (transform.root.GetComponentInChildren<IFF>().isPlayer == false)
        {
            foreach (SpriteRenderer thisSR in srs)
            {
                thisSR.enabled = false;
            }
        }
    }

    public void MakeObjectVisible()
    {
        foreach (SpriteRenderer thisSR in srs)
        {
            thisSR.enabled = true;
        }
    }
}
