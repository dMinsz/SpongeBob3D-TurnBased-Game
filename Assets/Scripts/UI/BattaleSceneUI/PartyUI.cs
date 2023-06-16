using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PartyUI : BaseUI
{
    [HideInInspector] private PlayableDatas playerDatas;

    protected override void Awake()
    {
        DataSetUp();
    }

    public void SetHpValue(int value)
    {
        texts["HP"].text = value.ToString();
    }

    public void SetSpValue(int value)
    {
        texts["SP"].text = value.ToString();
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

            partyElement.sliders["HPSlider"].value = playerDatas.players[tempi].MaxHP;
            partyElement.sliders["SPSlider"].value = playerDatas.players[tempi].MaxSP;


            //Setting Slider HP, SP bar
            partyElement.sliders["HPSlider"].GetComponent<HPbar>().unit = BattleManager.Instance.playerUnits[tempi];
            partyElement.sliders["SPSlider"].GetComponent<SPbar>().unit = BattleManager.Instance.playerUnits[tempi];

            partyElement.sliders["HPSlider"].GetComponent<HPbar>().SetBar();
            partyElement.sliders["SPSlider"].GetComponent<SPbar>().SetBar();

            partyElement.SetEvent(BattleManager.Instance.playerUnits[tempi].OnHpChanged, BattleManager.Instance.playerUnits[tempi].OnSpChanged);

            tempi++;
        }
        
    }
}
