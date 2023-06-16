using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleMenuButton : BaseUI, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TMP_Text text;
    public Image back;
    protected override void Awake()
    {
        base.Awake();

        if (text == null)
            text = GetComponentInChildren<TMP_Text>();
        if (back == null)
            back = GetComponentInChildren<Image>();

        //GetComponent<Button>().onClick.AddListener(GameManager.Battle.OnPlayerAttack);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.fontSize = 35;
        text.color = Color.white;
        back.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.fontSize = 30;
        text.color = Color.black;
        back.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        text.fontSize = 30;
        text.color = Color.black;
        back.gameObject.SetActive(false);
    }
}
