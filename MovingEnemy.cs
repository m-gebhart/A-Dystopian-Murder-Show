/* Michael Gebhart
 * Cologne Game Lab
 * BA 1 - Ludic Game 2018 / 2019 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : Enemy {

    /*this class adds abilities to every back-and-forth enemy instance of Banana, Lobsters */
    public bool movable;
    public float speed;
    public bool goingRight;

    protected void Move()
    {
        if (movable)
        {
            if (goingRight)
                rb2d.velocity = Vector2.right * speed;
            else if (!goingRight)
            {
                rb2d.velocity = Vector2.left * speed;
                SetLeft();
            }
        }
    }

    protected void ChangeDirection() //called by edge Triggers (located at the end of every platforms)
    {
        if (goingRight)
        {
            goingRight = false;
            SetLeft();
        }
        else if (!goingRight)
        {
            goingRight = true;
            SetRight();
        }
    }

    //change rotation leftwards
    public void SetLeft()
    {
        transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    //change rotation rightwards
    public void SetRight()
    {
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}