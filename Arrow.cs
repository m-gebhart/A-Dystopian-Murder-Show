using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arrow : MonoBehaviour {

	/* this arrow is simply part of the UI: if the score increases / enemies are attacked, this up_arrow is shown next to the current score display */
    public float noFadeTime;
    public float fadeOutTime = 0.5f;
    private float originNoFadeTime;
    private bool fadedIn = false;
    private Image arrow;
    private SpriteRenderer circle;

	void Start () {
        originNoFadeTime = noFadeTime;
        arrow = GameObject.Find("/Overlay/Arrow").GetComponent<Image>();
        circle = GameObject.Find("/Playermanager/PlayerLaurel/Circle").GetComponent<SpriteRenderer>();
        arrow.enabled = false;
    }
	
	void Update () {
        if (fadedIn && !circle.enabled)
            noFadeTime -= Time.deltaTime;
        if (fadedIn && noFadeTime <= 0f)
        {
            fadedIn = false;
            StartCoroutine(FadingOut(arrow));
        }
	}

    IEnumerator FadingOut(Image sprite)
    {
        Color tempColor = sprite.color;
        bool fadingOut = true;
        while (fadingOut && tempColor.a > 0f)
        {
            tempColor.a -= Time.deltaTime / fadeOutTime;
            sprite.color = tempColor;

            if (tempColor.a <= 0f)
            {
                sprite.enabled = false;
                tempColor.a = 1f;
                sprite.color = tempColor;
                fadingOut = false;
            }
            yield return null;
        }
    }

    void FadeArrowIn()
    {
        if (!fadedIn)
        {
            noFadeTime = originNoFadeTime;
            fadedIn = true;
            arrow.enabled = true;
        }
    }
}
