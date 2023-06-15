using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPbar : MonoBehaviour
{
    [SerializeField] public Unit unit;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        SetBar();
    }

    public void SetBar() 
    {
        slider.maxValue = unit.MaxHP;
        slider.value = unit.curHP;
        unit.OnHpChanged.AddListener(SetValue);
    }

    public void SetValue(int value)
    {
        slider.value = value;
    }
}
