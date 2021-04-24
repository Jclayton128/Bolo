using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPanel : MonoBehaviour
{
    //init
    [SerializeField] RectTransform panel = null;
    [SerializeField] RectTransform retractPos = null;
    [SerializeField] RectTransform extendPos = null;

    //param
    float lerpSpeed = 400f; //pixels per second. panel is 200 pixel wide

    //hood
    public bool isExtended = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ListenForKeyPressForTesting();
        UpdateUI();
    }

    private void ListenForKeyPressForTesting()
    {
        Debug.Log("listening");
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            Debug.Log("tab was pressed");
            isExtended = !isExtended;
        }
    }

    private void UpdateUI()
    {
        if (isExtended)
        {
            float x = Mathf.MoveTowards(panel.position.x, extendPos.position.x, lerpSpeed * Time.deltaTime);
            //x = Mathf.Clamp(x, retractPos.position.x, extendPos.position.x);
            panel.position = new Vector2(x, panel.position.y);
        }
        if (!isExtended)
        {
            float x = Mathf.MoveTowards(panel.position.x, retractPos.position.x, lerpSpeed * Time.deltaTime);
            //x = Mathf.Clamp(x, retractPos.position.x, extendPos.position.x);
            panel.position = new Vector2(x, panel.position.y);
        }

    }
}
