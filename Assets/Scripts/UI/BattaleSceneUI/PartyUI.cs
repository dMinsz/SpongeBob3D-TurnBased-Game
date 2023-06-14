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
        base.BindChildren();
        //buttons["CloseButton"].onClick.AddListener(() => { GameManager.UI.CloseWindowUI(this); });
    }

    //protected override void BindChildren()
    //{
    //    transforms = new Dictionary<string, RectTransform>();
    //    Images = new Dictionary<string, Image>();

    //    RectTransform[] children = GetComponentsInChildren<RectTransform>();
    //    // GetComponentsInChildren => baseUI �� �������� ���� ��� �ڽĵ��� �����´�
    //    // RectTransform�� UI�� ��� �ֱ⶧���� ��� ���� UI �ڽĵ��� childeren �� �־��ִ� �� �ȴ�.
    //    foreach (RectTransform child in children)
    //    {
    //        string key = child.gameObject.name; // ������Ʈ�� �̸��� Ű������ ����� ���̴�.

    //        if (transforms.ContainsKey(key))
    //            continue;

    //        transforms.Add(key, child);

    //        Image image = child.GetComponent<Image>();
    //        if (image != null)
    //        {
    //            Images.Add(key, image);
    //        }


    //    }
    //}

    public void DataSetUp() 
    {
        
        playerDatas = GameManager.Resource.Load<PlayableDatas>("Datas/PlayableDatas"); ;
        int count = playerDatas.players.Length;
        int tempi = 0;

        //var Canvas = GameObject.Find("BattleCanvas");

        //var BattleUI = Canvas.transform.Find("BattleUI");


        while (tempi < count)
        {
            
            PartyElementUI partyElement = GameManager.Resource.Instantiate<PartyElementUI>("PreFabs/UI/PartyElement");
            partyElement.transform.SetParent(transform, false);
            //partyElement.Bind();
            //Sprite test = playerDatas.players[tempi].CharacterIamge;
            partyElement.Images["Character"].sprite = playerDatas.players[tempi].CharacterIamge;
            //GameManager.Pool.GetUI(partyElement); // Add Object Pool

            tempi++;
        }
        
    }
}
