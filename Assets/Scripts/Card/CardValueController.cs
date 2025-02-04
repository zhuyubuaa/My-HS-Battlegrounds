using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardInfo {
    // Start is called before the first frame update

    public int HP = 2;

    public int Attack = 1;

    public int Cost = 3;

    public bool isSpell = false; // else Minion

    public CardInfo(int hp, int attack) {
        HP = hp;
        Attack = attack;
        isSpell = false;
    }

    public CardInfo(int cost) {
        Cost = cost;
        isSpell = true;
    }
}

public class CardValueController : Singleton<CardValueController> {
    public GameObject HPText;

    public GameObject AttackText;

    public GameObject CostText;

    private TextMeshProUGUI HPTextComp;

    private TextMeshProUGUI AttackTextComp;

    private TextMeshProUGUI CostTextComp;

    public CardInfo PrimaryCardInfo;

    public int HpStatus;

    public int AttackStatus;

    public int CostStatus;

    public void UpdateHp(int value) {
        HpStatus = value;
        HPTextComp.text = HpStatus.ToString();
    }

    public void UpdateAttack(int value) {
        AttackStatus = value;
        AttackTextComp.text = AttackStatus.ToString();
    }
    public void UpdateCost(int value) {
        CostStatus = value;
        CostTextComp.text = CostStatus.ToString();
    }

    public static int GetHPOf(GameObject o) => o.GetComponent<CardValueController>().HpStatus;

    public static int GetAttackOf(GameObject o) => o.GetComponent<CardValueController>().AttackStatus;

    public static int GetCostOf(GameObject o) => o.GetComponent<CardValueController>().CostStatus;

    public static bool IsSpell(GameObject o) => o.GetComponent<CardValueController>().PrimaryCardInfo.isSpell;

    public static void SetHPOf(GameObject o, int value) => o.GetComponent<CardValueController>().UpdateHp(value);

    public static void SetAttackOf(GameObject o, int value) => o.GetComponent<CardValueController>().UpdateAttack(value);

    public static void SetCostOf(GameObject o, int value) => o.GetComponent<CardValueController>().UpdateAttack(value);

    public static void ModifyValuesAfterHit(Transform hitter, Transform hittee) {
        var hitterO = hitter.gameObject;
        var hitteeO = hittee.gameObject;
        var erHP = GetHPOf(hitterO);
        var eeHP = GetHPOf(hitteeO);
        var erAttack = GetAttackOf(hitterO);
        var eeAttack = GetAttackOf(hitteeO);
        var erLeftHP = erHP - eeAttack;
        var eeLeftHP = eeHP - erAttack;
        SetHPOf(hitterO, erLeftHP);
        SetHPOf(hitteeO, eeLeftHP);   
    }

    // Start is called before the first frame update
    private void Start() {
        HPTextComp = HPText.GetComponent<TextMeshProUGUI>();
        AttackTextComp = AttackText.GetComponent<TextMeshProUGUI>();
        CostTextComp = CostText.GetComponent<TextMeshProUGUI>();

        if (PrimaryCardInfo.isSpell) {
            HPText.SetActive(false);
            AttackText.SetActive(false);

            UpdateCost(PrimaryCardInfo.Cost);
            CostText.SetActive(true);
        } else {
            CostStatus = 3;
            CostText.SetActive(false);

            UpdateHp(PrimaryCardInfo.HP);
            UpdateAttack(PrimaryCardInfo.Attack);
            HPText.SetActive(true);
            AttackText.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
