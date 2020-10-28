using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_BarController : MonoBehaviour
{
    [SerializeField]
    private float currentHP;
    
    [SerializeField] 
    private float maxHP;

    [SerializeField]
    private Image hpBarContent;

    private CharacerterStats charStats;

    void Start()
    {
        charStats = GetComponent<CharacerterStats>();
        maxHP = charStats.MaxHealth.StatValue;
        currentHP = charStats.CurrentHealth.StatValue;
    }

    void Update()
    {
        HandleBar();
    }

    private void HandleBar()
    {
        maxHP = charStats.MaxHealth.StatValue;
        currentHP = charStats.CurrentHealth.StatValue;
        hpBarContent.fillAmount = HpToAmount(currentHP, maxHP);
    }

    private float HpToAmount(float curHp, float maxHp)
    {
        return curHp * 1 / maxHp;
    }

}
