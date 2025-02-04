using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardShowingController : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public float fadeInTime = 0.5f;

    public float fadeOutTime = 0.5f;

    public float waitTime = 2.0f;

    private void Start()
    {
        if (canvasGroup == null) {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }

    public IEnumerator FadeOut() {
        while (canvasGroup.alpha > 0.0f) {
            canvasGroup.alpha -= Time.deltaTime / fadeOutTime;
            Debug.Log(canvasGroup.alpha);
            yield return null;
        }
        canvasGroup.alpha = 0.0f;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        
    }
}
