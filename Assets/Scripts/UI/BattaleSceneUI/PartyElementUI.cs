using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class PartyElementUI : BaseUI
{
    
    public Dictionary<string, Image> Images = new Dictionary<string, Image>();
    protected override void Awake()
    {
        base.BindChildren();
        BindChildren();


    }

    public void SetEvent(UnityEvent<int> hpEvnet, UnityEvent<int> spEvent) 
    {
        hpEvnet.AddListener(SetHpValue);
        spEvent.AddListener(SetSpValue);
    }

    public void SetHpValue(int value)
    {
        texts["HP"].text = value.ToString();
    }

    public void SetSpValue(int value)
    {
        texts["SP"].text = value.ToString();
    }


    protected override void BindChildren()
    {
        transforms = new Dictionary<string, RectTransform>();
        //Images = new Dictionary<string, Image>();

        RectTransform[] children = GetComponentsInChildren<RectTransform>();
       
        foreach (RectTransform child in children)
        {
            string key = child.gameObject.name; 

            if (transforms.ContainsKey(key))
                continue;

            transforms.Add(key, child);


            Image image = child.GetComponent<Image>();
            if (image != null)
            {
                Images.Add(key, image);
            }


        }
    }

}
