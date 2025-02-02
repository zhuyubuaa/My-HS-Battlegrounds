using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : Singleton<EnemyController>
{
    public Transform EnemyPanel;

    public GameObject CardModel;

    private void GenerateRandomEnemies(int num) {
        for (int i = 0; i < num; i++) {
            var newMinion = Instantiate(CardModel, EnemyPanel);
            var info = new CardInfo(Random.Range(1, 6), Random.Range(1, 6));
            newMinion.GetComponent<CardValueController>().cardInfo = info;
        }
    }

    private void OnEnable() {
        EnemyPanel.GetComponent<Image>().color = Color.clear;
        GenerateRandomEnemies(3);
    }

    private void OnDisable() {
        EnemyPanel.GetComponent<Image>().color = Color.white;
        for (int i = 0; i < EnemyPanel.childCount; i++) {
            Destroy(EnemyPanel.GetChild(i).gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
