using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePadButtons : MonoBehaviour {

    /*this class determines which buttons are displayed during tutorial levels*/

    public Sprite gamePadButton;

    void Start()
    {
        if (GamePadInUse.xboxPadInUse)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = gamePadButton;
        }
    }
}
