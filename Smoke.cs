using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : Splatter {

    private SpriteRenderer renderer;
    public float growthSpeed;
    public float maxScale;

    void Start()
    {
        renderer = this.gameObject.GetComponent<SpriteRenderer>();
        renderer.material.shader = Shader.Find("GUI/Text Shader");
        renderer.color = Color.white;
    }

    void Update()
    {
        if (transform.localScale.x < maxScale)
            transform.localScale += new Vector3(growthSpeed, growthSpeed, 0);
    }
}
