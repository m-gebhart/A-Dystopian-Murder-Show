/* Michael Gebhart
 * Cologne Game Lab 
 * BA 1 - Ludic Game 2018/2019
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour {

    /* This classed will always be called, if input for other classes's functions are needed */

    public static bool playerAlive; //to differentiate between play-levels and menu screens

    public static bool JumpInput()
    {
        if (playerAlive && 
            (Input.GetKeyDown(KeyCode.Space) 
            || Input.GetKeyDown(KeyCode.W) 
            || Input.GetKeyDown(KeyCode.UpArrow) 
            || Input.GetButtonDown("XBoxA")))
            return true;
        else 
            return false;
    }

    static bool rightTriggerDown; //alternative input for attacking by right trigger

    public static bool AttackInput()
    {
        if (playerAlive)
        {
            if (Input.GetKeyDown(KeyCode.E)
            || Input.GetKeyDown(KeyCode.F)
            || Input.GetMouseButtonDown(0)
            || Input.GetKeyDown(KeyCode.KeypadEnter)
            || Input.GetButtonDown("XBoxX"))
                return true;
            //creating a DownFunction for Xbox Triggers
            else if (!rightTriggerDown && Input.GetAxis("XBoxRightTrigger") > 0)
            {
                rightTriggerDown = true;
                return true;
            }
            else if (rightTriggerDown && Input.GetAxis("XBoxRightTrigger") == 0)
            {
                rightTriggerDown = false;
                return false;
            }
            else
                return false;
        }
        else
            return false;
    }

    static bool leftTriggerDown; //alternative input for shapeshifting by left trigger

    public static bool ShapeshiftInput()
    {
        if (playerAlive)
        {
            if (Input.GetKeyDown(KeyCode.Q)
            || Input.GetButtonDown("XBoxB"))
                return true;
            //creating a DownFunction for Xbox Triggers
            else if (!leftTriggerDown && Input.GetAxis("XBoxLeftTrigger") > 0)
            {
                leftTriggerDown = true;
                return true;
            }
            else if (leftTriggerDown && Input.GetAxis("XBoxLeftTrigger") == 0)
            {
                leftTriggerDown = false;
                return false;
            }
            else
                return false;
        }
        else
            return false;
    }

    static bool verticalAxisDown;

    public static bool MoveRightInput()
    {
        if (playerAlive && 
            (Input.GetKey(KeyCode.D) 
            || Input.GetKey(KeyCode.RightArrow) 
            || Input.GetAxis("XBoxHorizontal") > 0))
            return true;
        else
            return false;
    }

    public static bool MoveLeftInput()
    {
        if (playerAlive && 
            (Input.GetKey(KeyCode.A) 
            || Input.GetKey(KeyCode.LeftArrow) 
            || Input.GetAxis("XBoxHorizontal") < 0))
            return true;
        else
            return false;
    }

    //Game Over Screen
    public static bool RespawnInput()
    {
        if (!playerAlive && 
            (Input.GetKeyDown(KeyCode.R) 
            || Input.GetButtonDown("XBoxX")))
            return true;
        else
            return false;
    }

    //Getting into Elevator
    public static bool LeaveLevelInput()
    {
        if (playerAlive && (Input.GetKeyDown(KeyCode.S)
            || Input.GetKeyDown(KeyCode.DownArrow)
            || Input.GetButtonDown("XBoxY")))
            return true;
        else
            return false;
    }

    //Elevator and Game Over Screen
    public static bool EscapeInput()
    {
        if ((!playerAlive || (playerAlive && SceneManager.GetSceneByName("Elevator").isLoaded)) &&
            (Input.GetKey(KeyCode.Escape)
            || Input.GetKey(KeyCode.B)
            || Input.GetButtonDown("XBoxB")))
            return true;
        else
            return false;
    }


    //Menu Game Pad Arrow Controls

    static bool axisUpDown;

    public static bool UpInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            return true;
        else if (!axisUpDown && (Input.GetAxis("XBoxVertical") == -1 || Input.GetAxis("XBoxDPadVertical") == 1))
        {
            axisUpDown = true;
            return true;
        }
        else if (axisUpDown && (Input.GetAxis("XBoxVertical") == 0 && Input.GetAxis("XBoxDPadVertical") == 0))
        {
            axisUpDown = false;
            return false;
        }
        else
            return false;
    }

    static bool axisDownDown;

    public static bool DownInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
            return true;
        else if (!axisDownDown && (Input.GetAxis("XBoxVertical") == 1 || Input.GetAxis("XBoxDPadVertical") == -1))
        {
            axisDownDown = true;
            return true;
        }
        else if (axisDownDown && (Input.GetAxis("XBoxVertical") == 0 && Input.GetAxis("XBoxDPadVertical") == 0))
        {
            axisDownDown = false;
            return false;
        }
        else
            return false;
    }


    static bool axisRightDown;
    public static bool RightInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            return true;
        else if (!axisRightDown && (Input.GetAxis("XBoxHorizontal") == 1 || Input.GetAxis("XBoxDPadHorizontal") == 1))
        {
            axisRightDown = true;
            return true;
        }
        else if (axisRightDown && Input.GetAxis("XBoxHorizontal") != 1 && Input.GetAxis("XBoxDPadHorizontal") != 1)
        {
            axisRightDown = false;
            return false;
        }
        else
            return false;
    }

    static bool axisLeftDown;
    public static bool LeftInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            return true;
        else if (!axisLeftDown && (Input.GetAxis("XBoxHorizontal") == -1 || Input.GetAxis("XBoxDPadHorizontal") == -1))
        {
            axisLeftDown = true;
            return true;
        }
        else if (axisLeftDown && Input.GetAxis("XBoxHorizontal") != -1 && Input.GetAxis("XBoxDPadHorizontal") != -1)
        {
            axisLeftDown = false;
            return false;
        }
        else
            return false;
    }

    public static bool ApplyInput()
    {
        if (Input.GetKeyDown(KeyCode.Space)
            || Input.GetKeyDown(KeyCode.Return)
            || Input.GetButtonDown("XBoxA"))
            return true;
        else
            return false;
    }
}