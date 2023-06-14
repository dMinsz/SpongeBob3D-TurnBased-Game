using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartyElementUI : BaseUI
{
    
    public Dictionary<string, Image> Images = new Dictionary<string, Image>();
    protected override void Awake()
    {
        base.BindChildren();
        BindChildren();
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
