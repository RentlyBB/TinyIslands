using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerDotCanvasCreater : MonoBehaviour {

    public Canvas canvas;

    public Color color;
    
    // Start is called before the first frame update
    void Start() {

        if (canvas != null) {
            canvas.GetComponentInChildren<Image>().color = this.color;
            canvas.worldCamera = Camera.main;
            Instantiate(canvas, gameObject.transform);
        }

    }

}