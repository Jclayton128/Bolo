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
    SceneLoader sl;

    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AllegianceManager>();
        sl = FindObjectOfType<SceneLoader>();
        SetButtonFlags();
    }

    private void SetButtonFlags()
    {
        foreach (Button button in buttons)
        {
            button.GetComponent<Image>().sprite = am.GetFlagOfAllegiance(button.gameObject.GetComponent<IFF>().GetIFFAllegiance());
        }
    }

    private void Update()
    {
        
    }

    public void SelectAFlag(int chosenIFF)
    {
        am.SetPlayerIFF(chosenIFF);
        //Debug.Log("FBM is setting allegiance of: " + chosenIFF);
        sl.LoadSecondScene();
        
    }

}
