/* Michael Gebhart
 * Cologne Game Lab
 * BA 1 - Ludic Game 2018 / 2019 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnMouse : MonoBehaviour {

    private Text text;
    private Color originColor;

	void Start () {
        text = this.gameObject.GetComponent<Text>();
        originColor = text.color;
	}
	
    void OnMouseOver() { changeColor(); }

    void OnMouseExit() { text.color = originColor; }

    void changeColor() { text.color = Color.red; } //sometimes directly called by Event Triggers
}