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
    [SerializeField] Transform[] queueStack = null;
    [SerializeField] Image soldierIcon = null;
    [SerializeField] Image tankIcon = null;
    [SerializeField] Image missileVanIcon = null;
    [SerializeField] Image helicopterIcon = null;

    //param
    float lerpSpeed = 400f; //pixels per second. panel is 200 pixel wide

    //hood
    public bool isExtended { get; private set; } = false;


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
        if (Input.GetKeyUp(KeyCode.Tab))
        {
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
