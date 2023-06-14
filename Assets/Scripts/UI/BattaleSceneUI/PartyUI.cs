using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PartyUI : BaseUI
{

    [HideInInspector] private PlayableDatas playerDatas;

    //protected Dictionary<string, Image> Images;
    
    protected override void Awake()
    {
        DataSetUp();
        //BindChildren();
        //base.BindChildren();
        //buttons["CloseButton"].onClick.AddListener(() => { GameManager.UI.CloseWindowUI(this); });
    }


    public void UISetUp() 
    {

    }
    public void DataSetUp() 
    {
        
        playerDatas = GameManager.Resource.Load<PlayableDatas>("Datas/PlayableDatas"); ;
        int count = playerDatas.players.Length;
        int tempi = 0;

        while (tempi < count)
        {
            
            PartyElementUI partyElement = GameManager.Resource.Instantiate<PartyElementUI>("PreFabs/UI/PartyElement");
            partyElement.transform.SetParent(transform, false);
            partyElement.Images["Character"].sprite = playerDatas.players[tempi].CharacterIamge;
            partyElement.texts["Name"].text = playerDatas.players[tempi].name;
            partyElement.texts["HP"].text = playerDatas.players[tempi].MaxHP.ToString();
            partyElement.texts["SP"].text = playerDatas.players[tempi].MaxSP.ToString();
            tempi++;
        }
        
    }
}
