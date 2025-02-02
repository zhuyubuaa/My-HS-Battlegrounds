using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoCanvasController : MonoBehaviour
{
    private static InfoCanvasController infoCanvasController;

    // Start is called before the first frame update
    private void Start()
    {
        if (infoCanvasController == null) {
            infoCanvasController = this;
            DontDestroyOnLoad(infoCanvasController.gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
