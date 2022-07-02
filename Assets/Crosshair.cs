using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour {

    public LayerMask targetMask;
    public SpriteRenderer dot;
    public Color dotHighlightColor;
    Color originDotColor;

    void Start() {
        Cursor.visible = false;
        originDotColor = dot.color;
    }

    void Update() {
        transform.Rotate(Vector3.forward * -40 * Time.deltaTime);

    }

    public void DetectTargets(Ray ray) {
        if (Physics.Raycast(ray, 100, targetMask)) {
            dot.color = dotHighlightColor;
        } else {
            dot.color = originDotColor;
        }
    }
}
