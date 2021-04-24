using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthSeeker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.root == transform.root) { return; } //dont detect oneself;

        Debug.Log("detection");

        if (transform.root.tag == "Player")
        {
            collision.GetComponent<StealthHider>().MakeObjectVisible();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("back to hiding");

        if (transform.root.tag == "Player" )
        {
            collision.GetComponent<StealthHider>().MakeObjectInvisible();
        }
    }
}
