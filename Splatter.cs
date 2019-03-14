/* Michael Gebhart
 * Cologne Game Lab
 * BA 1 - Ludic Game 2018 / 2019 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Splatter : MonoBehaviour {

    /*Splatter is instantiated every time an enemy dies, showing blood (Bananas, Bat, Lobster), Oil (Cannon, Bombs) or Jam (Pie) that fades out with time*/

    public float noFadeTime;
    public float fadeOutTime;

    void LateUpdate()
    {
        FadeTimer();
    }

    void FadeTimer()
    {
        noFadeTime -= Time.deltaTime;
        if (noFadeTime < 0f)
            StartCoroutine(FadingOut(this.gameObject.GetComponent<SpriteRenderer>()));
    }

        IEnumerator FadingOut (SpriteRenderer sprite)
    {
        Color tempColor = sprite.color;
        while (tempColor.a > 0f)
        {
            tempColor.a -= Time.deltaTime / fadeOutTime;
            sprite.color = tempColor;

            if (tempColor.a <= 0f)
                Destroy(this.gameObject);
            yield return null;
        }
	}
}
