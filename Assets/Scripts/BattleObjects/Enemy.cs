using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Enemy : Unit, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Animator animator;
    Image targeting;
    bool isTargeted = false;

    private void Awake()
    {
        
        var canvas = transform.Find("Canvas");
        targeting = canvas.Find("Targeting").GetComponent<Image>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (targeting != null) 
            targeting.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDie())
        {
            Die();
        }
    }
    #region BattleScean Point Funcs
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.Find("Canvas").gameObject.SetActive(true);
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

            if (GameManager.Battle.GetTargetedEnemy() == null)
            {//��Ʋ �Ŵ����� Ÿ���� Ȯ��
                isTargeted = true;

                targeting.gameObject.SetActive(true);
                targeting.color = AdjustAlpha(255); // ���İ�����

                GameManager.Battle.SetTaget(this);
            }
            else //Ÿ�����̵Ǿ�������
            {
                //����Ÿ���É���� Ǯ���ش�.
                GameManager.Battle.GetTargetedEnemy().targeting.gameObject.SetActive(false);
                GameManager.Battle.GetTargetedEnemy().isTargeted = false;

                //���� ������� Ÿ�� ���İ�����
                Color tempTargetColor = GameManager.Battle.GetTargetedEnemy().targeting.color;
                tempTargetColor.a = 140;

                GameManager.Battle.GetTargetedEnemy().targeting.color = tempTargetColor;

                //���� Ÿ�� ���İ�����
                targeting.gameObject.SetActive(true);
                targeting.color = AdjustAlpha(255);

                isTargeted = true;
                
                targeting.gameObject.SetActive(true);

                GameManager.Battle.SetTaget(this);

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

        if (base.HP <= 0)
        {
            base.HP = 0;
            base.isDied = true;
        }
    }

    public void Die()
    {
        //������ �ൿ �߰�
        Destroy(this.gameObject);
    }
}
