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

    public CardInfo cardInfo;

    public static int GetHPOf(GameObject o) => o.GetComponent<CardValueController>().cardInfo.HP;

    public static int GetAttackOf(GameObject o) => o.GetComponent<CardValueController>().cardInfo.Attack;

    public static int GetCostOf(GameObject o) => o.GetComponent<CardValueController>().cardInfo.Cost;

    public static bool IsSpell(GameObject o) => o.GetComponent<CardValueController>().cardInfo.isSpell;

    // Start is called before the first frame update
    private void Start() {
        HPTextComp = HPText.GetComponent<TextMeshProUGUI>();
        AttackTextComp = AttackText.GetComponent<TextMeshProUGUI>();
        CostTextComp = CostText.GetComponent<TextMeshProUGUI>();

        if (cardInfo.isSpell) {
            HPText.SetActive(false);
            AttackText.SetActive(false);

            CostTextComp.text = cardInfo.Cost.ToString();
            CostText.SetActive(true);
        } else {
            CostText.SetActive(false);

            HPTextComp.text = cardInfo.HP.ToString();
            AttackTextComp.text = cardInfo.Attack.ToString();
            HPText.SetActive(true);
            AttackText.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
