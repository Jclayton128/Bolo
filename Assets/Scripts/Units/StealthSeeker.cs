using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthSeeker : MonoBehaviour
{
    //init
    ControlSource cs;
    void Start()
    {
        cs = GetComponentInParent<ControlSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.root == transform.root) { return; } //dont detect oneself;

        //Debug.Log("detection");

        if (transform.root.tag == "Player")
        {
            //Debug.Log($"detected {collision.transform.root.name} and trying to make it visible to player");
            collision.GetComponent<StealthHider>().MakeObjectVisible();
        }
        cs.RequestScan();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("back to hiding");

        if (transform.root.tag == "Player" )
        {
            collision.GetComponent<StealthHider>().MakeObjectInvisible();
        }
    }
}
