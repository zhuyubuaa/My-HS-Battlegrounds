using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ActionController : MonoBehaviour
{
    public GameObject OnFightPanel;

    public GameObject EnemyPanel;

    private List<Transform> MyFightingMinions = new();

    private List<Transform> Enemies = new();

    private int speed = 300;

    // Start is called before the first frame update
    void Start()
    {
        OnFightPanel = GameObject.Find("OnFightPanel");
        if (EnemyPanel == null) {
            EnemyPanel = GameObject.Find("EnemyPanel");
        }
        for (int i = 0; i < OnFightPanel.transform.childCount; i++) {
            MyFightingMinions.Add(OnFightPanel.transform.GetChild(i));
        }
        for (int i = 0; i < EnemyPanel.transform.childCount; i++) {
            Enemies.Add(EnemyPanel.transform.GetChild(i));
        }
        Vector3 primaryLocation = MyFightingMinions[0].position;
        StartCoroutine(Hit(MyFightingMinions[0], Enemies[0], primaryLocation));
    }

    private IEnumerator Hit(Transform self, Transform target, Vector3 pl) {
        while (Vector3.Distance(self.position, target.position) > 0.1f) {
            self.position = Vector3.MoveTowards(self.position, target.position, speed * Time.deltaTime);
            speed += 10;
            yield return null;
        }
        CardValueController.ModifyValuesAfterHit(self, target);
        StartCoroutine(Return(self, target, pl));
    }

    private IEnumerator Return(Transform self, Transform target, Vector3 pl) {
        float smoothTime = 0.2f;
        Vector3 velocity = Vector3.zero;
        while(Vector3.Distance(self.position, pl) > 0.1f) {
            self.position = Vector3.SmoothDamp(self.position, pl, ref velocity, smoothTime, Mathf.Infinity);
            yield return null;
        }
        if (CardValueController.GetHPOf(self.gameObject) <= 0) {
            MinionDie(self);
        }
        if (CardValueController.GetHPOf(target.gameObject) <= 0) {
            MinionDie(target);
        }
    }

    private void MinionDie(Transform m) {
        var mShowComp = m.GetComponent<CardShowingController>();
        StartCoroutine(mShowComp.FadeOut());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
