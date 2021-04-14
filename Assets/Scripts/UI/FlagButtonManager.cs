using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class FlagButtonManager : MonoBehaviour
{
    //init
    [SerializeField] List<Button> buttons = new List<Button>();
    AllegianceManager am;

    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AllegianceManager>();
        SetButtonFlags();
    }

    private void SetButtonFlags()
    {
        foreach (Button button in buttons)
        {
            button.GetComponent<Image>().sprite = am.GetFlagOfAllegiance(buttons.IndexOf(button));
        }
    }

    private void Update()
    {
        
    }

    public void SelectAFlag(Button button)
    {
        int allegiance = buttons.IndexOf(button);
        am.playerAllegiance = allegiance;
        Debug.Log("ready to start, allegiance: " + am.playerAllegiance);
        
    }

}
