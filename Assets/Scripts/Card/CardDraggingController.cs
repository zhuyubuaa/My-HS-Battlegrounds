using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CardDraggingController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    public enum CardState {
        OnSell,
        InHand,
        OnFight
    }

    private CardState state = CardState.OnSell;

    public GameObject MyCardsUpperBound;

    public GameObject ShopLine;

    public GameObject MyCards;

    public GameObject OnFightPanel;

    public Vector2 primaryLocation;

    private bool isOnDrag = false;

    private UnityAction sellEvents = null;

    private UnityAction<int> buyEvents = null;

    public GameObject CanvObject;

    private Canvas canv;

    public RectTransform dragUI;

    private RectTransform dragObjRect = null;

    private bool canDrag;

    private bool LocateInShop() => ShopLine.transform.position.y <= transform.position.y;
    private bool LocateInHand() => transform.position.y <= MyCardsUpperBound.transform.position.y;
    private bool LocateOnFight() => ShopLine.transform.position.y > transform.position.y &&
                    transform.position.y > MyCardsUpperBound.transform.position.y;

    private void CancelMoving() => GetComponent<RectTransform>().anchoredPosition = primaryLocation;

    private bool IsWillingToSwapWith(Transform other) => transform != other &&
        Vector2.Distance(GetComponent<RectTransform>().anchoredPosition, other.GetComponent<RectTransform>().anchoredPosition) <= 1;

    private void willingDebug(Transform other) {
        if (transform != other) {
            Debug.Log("not self");
            Debug.Log(Vector2.Distance(GetComponent<RectTransform>().anchoredPosition, other.GetComponent<RectTransform>().anchoredPosition));
            Debug.Log("self: " + GetComponent<RectTransform>().anchoredPosition);
            Debug.Log("other: " + other.GetComponent<RectTransform>().anchoredPosition);
        }
    }

    private void SwapWith(Transform other) =>
        (other.GetComponent<RectTransform>().anchoredPosition, GetComponent<RectTransform>().anchoredPosition) = (primaryLocation, other.GetComponent<RectTransform>().anchoredPosition);

    private void SwapCheck() {
        var isSwapped = false;
        foreach (var child in OnFightPanel.GetComponentsInChildren<Transform>(true)) {
            //willingDebug(child);
            if (IsWillingToSwapWith(child)) {
                //Debug.Log("iswilling");
                SwapWith(child);
                isSwapped = true;
                break;
            }
        }
        if (!isSwapped) {
            //Debug.Log("notwilling");
            CancelMoving();
        }
    }

    // Start is called before the first frame update
    private void Awake() {
        if (MyCardsUpperBound == null) {
            MyCardsUpperBound = GameObject.Find("MyCardsUpperBound");
        }
        if (MyCards == null) {
            MyCards = GameObject.Find("CardsLayout");
        }
        if (OnFightPanel == null) {
            OnFightPanel = GameObject.Find("OnFightPanel");
        }
        if (ShopLine == null) {
            ShopLine = GameObject.Find("ShopLine");
        }
        sellEvents += ShopController.Instance.SellCard;
        buyEvents += ShopController.Instance.BuyCard;
    }

    private void Start() {
        CanvObject = GameObject.Find("InfoCanvas");
        if (SceneManager.GetActiveScene().name == "FightingScene") {
            // 只有生成的敌人会进入此分支，禁用拖拽
            enabled = false;
            return;
        }
        canv = CanvObject.GetComponent<Canvas>();
        dragObjRect = canv.transform as RectTransform;
    }


    // Update is called once per frame
    void Update() {

    }

    private void TryToSellCard() {
        if (CardValueController.IsSpell(gameObject)) {
            CancelMoving();
        } else {
            sellEvents.Invoke();
            Destroy(gameObject);
        }
    }

    private void TryToPutCard() {
        if (CardValueController.IsSpell(gameObject)) {
            // spell take effect
            Destroy(gameObject);
        } else {
            transform.SetParent(OnFightPanel.transform);
            state = CardState.OnFight;
        }
    }

    private void TryToBuyCard() {
        if (ShopController.Instance.CheckBuyCondition(CardValueController.GetCostOf(gameObject))) {
            buyEvents.Invoke(CardValueController.GetCostOf(gameObject));
            transform.SetParent(MyCards.transform);
            state = CardState.InHand;
        } else {
            CancelMoving();
            // Effect...
        }
    }

    public void OnBeginDrag(PointerEventData eventData) {
        primaryLocation = GetComponent<RectTransform>().anchoredPosition;
        //if (eventData.pointerEnter.tag == "item") {
        //    canDrag = true;
        //    dragUI = eventData.pointerEnter.GetComponent<RectTransform>();
        //} else {
        //    canDrag = false;
        //}
        canDrag = SceneManager.GetActiveScene().name == "ShopScene";
        if (canDrag)
            dragUI = gameObject.GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData) {
        if (!canDrag) { return; }
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle
            (dragObjRect, eventData.position, eventData.pressEventCamera, out Vector3 globalMousePos)) {

            dragUI.position = globalMousePos;
            dragUI.rotation = dragObjRect.rotation;
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (!canDrag) return;
        dragUI = null;
        switch (state) {
            case CardState.OnSell:
                if (LocateInHand()) {
                    TryToBuyCard();
                } else {
                    CancelMoving();
                }
                break;
            case CardState.InHand:
                if (LocateOnFight()) {
                    TryToPutCard();
                } else {
                    CancelMoving();
                }
                break;
            case CardState.OnFight:
                if (LocateInShop()) {
                    TryToSellCard();
                } else if (LocateOnFight()) {
                    SwapCheck();
                } else {
                    CancelMoving();
                }
                break;
            default:
                break;
        }
    }
}
