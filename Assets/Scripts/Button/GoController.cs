using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoController : MonoBehaviour
{
    public GameObject InfoCanvas;

    public void GoOnClick() {
        if (false /*TODO: freeze*/) {

        } else {
            ShopController.Instance.gameObject.SetActive(false);
        }
        SceneManager.LoadScene("FightingScene");
    }
}
