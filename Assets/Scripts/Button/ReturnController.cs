using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnController : MonoBehaviour
{
    public void ReturnOnClick() {
        SceneManager.LoadScene("ShopScene");
        ShopController.Instance.OpenShop();
    }
}
