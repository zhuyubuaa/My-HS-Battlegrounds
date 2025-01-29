using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopController : Singleton<ShopController> {
    // Start is called before the first frame update

    public GameObject CardModel;

    private Transform sellPanel;

    public TextMeshProUGUI mt;

    public TextMeshProUGUI ft;

    public TextMeshProUGUI ut;

    public TextMeshProUGUI st;

    private int moneyUpperBound;

    private int moneyLeft;

    private int freshCost;

    private int upgradeCost;

    private int shopLevel;

    private void UpdateMoneyText() {
        mt.text = moneyLeft + "/" + moneyUpperBound;
    }
    private void UpdateFreshText() {
        ft.text = freshCost.ToString();
    }
    private void UpdateUpgradeText() {
        ut.text = upgradeCost.ToString();
    }

    private void UpdateShopLevelText() {
        st.text = shopLevel.ToString();
    }

    public bool CheckBuyCondition(int v = 3) => moneyLeft >= v;

    public void SellCard() {
        moneyLeft++;
        UpdateMoneyText();
    }

    public void BuyCard(int v = 3) {
        moneyLeft -= v;
        UpdateMoneyText();
    }

    public void FreshShop() {
        for (int i = 0; i < sellPanel.childCount; i++) {
            Destroy(sellPanel.GetChild(i).gameObject);
        }
        GenerateRandomMinion(4);
        GenerateRandomSpell();
    }

    public void UpgradeShop() {
        shopLevel++;
        UpdateShopLevelText();
        upgradeCost++;
        UpdateUpgradeText();
    }


    public void FreshOnClick() {
        if (moneyLeft >= freshCost) {
            moneyLeft -= freshCost;
            UpdateMoneyText();
            FreshShop();
        } else {
            // ...
        }
    }

    public void UpgradeOnClick() {
        if (moneyLeft >= upgradeCost) {
            moneyLeft -= upgradeCost;
            UpdateMoneyText();
            UpgradeShop();
        } else {
            // ...
        }
    }

    private void Awake() {
        // 放随从
        sellPanel = transform.Find("SellPanel");
        GenerateRandomMinion(4);
        GenerateRandomSpell();

        // 初始化金钱
        moneyUpperBound = 6;
        moneyLeft = 6;
        UpdateMoneyText();
        freshCost = 1;
        UpdateFreshText();
        upgradeCost = 1;
        UpdateUpgradeText();
        shopLevel = 1;
        UpdateShopLevelText();
    }

    private void GenerateRandomSpell() {
            var newSpell = Instantiate(CardModel, sellPanel);
            var info = new CardInfo(Random.Range(1, 4));
            newSpell.GetComponent<CardValueController>().cardInfo = info;
    }

    private void GenerateRandomMinion(int num) {
        for (int i = 0; i < num; i++) {
        var newMinion = Instantiate(CardModel, sellPanel);
        var info = new CardInfo(Random.Range(1, 6), Random.Range(1, 6));
        newMinion.GetComponent<CardValueController>().cardInfo = info;
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
