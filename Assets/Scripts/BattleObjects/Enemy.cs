using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Enemy : Unit, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    Image targeting;
    bool isTargeted = false;

    private void Awake()
    {
        targeting = GetComponentInChildren<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        targeting.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    #region BattleScean Point Funcs
    public void OnPointerEnter(PointerEventData eventData)
    {
    
        targeting.gameObject.SetActive(true);

        if (isTargeted) 
        {
            targeting.color = AdjustAlpha(140);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isTargeted)
        {
            targeting.gameObject.SetActive(false);
        }


    }

    public Color AdjustAlpha(int alpha) 
    {
        Color temp = targeting.color;
        temp.a = alpha;

        return temp;
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (!isTargeted)
        { // Ÿ�����̾ȵǾ�������

            if (BattleManager.Instance.GetTargetedEnemy() == null)
            {//��Ʋ �Ŵ����� Ÿ���� Ȯ��
                isTargeted = true;

                targeting.gameObject.SetActive(true);
                targeting.color = AdjustAlpha(255); // ���İ�����

                BattleManager.Instance.SetTaget(this);
            }
            else //Ÿ�����̵Ǿ�������
            {
                //����Ÿ���É���� Ǯ���ش�.
                BattleManager.Instance.GetTargetedEnemy().targeting.gameObject.SetActive(false);
                BattleManager.Instance.GetTargetedEnemy().isTargeted = false;

                //���� ������� Ÿ�� ���İ�����
                Color tempTargetColor = BattleManager.Instance.GetTargetedEnemy().targeting.color;
                tempTargetColor.a = 140;

                BattleManager.Instance.GetTargetedEnemy().targeting.color = tempTargetColor;

                //���� Ÿ�� ���İ�����
                targeting.gameObject.SetActive(true);
                targeting.color = AdjustAlpha(255);

                isTargeted = true;
                
                targeting.gameObject.SetActive(true);

                BattleManager.Instance.SetTaget(this);

            }

        }
   
    

    }

    #endregion
    
    
    public override void Attack(int damage, Unit target)
    {
        target.TakeDamage(damage);
    }

    public override void DoSkill(int damage, Unit target)
    {
        target.TakeDamage(damage);
    }

    public override void TakeDamage(int damage)
    {
        base.HP -= damage;

        if (base.HP < 0)
        {
            base.HP = 0;
            base.isDied = true;
        }
    }

    public override bool IsDie()
    {
        //������ �ൿ �߰�
        
        return base.IsDie();
    }
}
