using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    //Some functions will need reference to the controller
    public GameObject controller;

    //The Chesspiece that was tapped to create this MovePlate
    GameObject reference = null;

    //Location on the board
    int matrixX;
    int matrixY;

    //false: movement, true: attacking
    public bool attack = false;

    public void Start()
    {
        if (attack)
        {
            //Set to red
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        //Destroy the victim Chesspiece
        if (attack)
        {
            GameObject cp = controller.GetComponent<Controller>().GetPosition(matrixX, matrixY);

            Destroy(cp);
        }

        //Set the Chesspiece's original location to be empty
        controller.GetComponent<Controller>().SetPositionEmpty(reference.GetComponent<ChessmanScript>().GetXBoard(), 
            reference.GetComponent<ChessmanScript>().GetYBoard());

        //Move reference chess piece to this position
        reference.GetComponent<ChessmanScript>().SetXBoard(matrixX);
        reference.GetComponent<ChessmanScript>().SetYBoard(matrixY);
        reference.GetComponent<ChessmanScript>().SetCoords();

        //Update the matrix
        controller.GetComponent<Controller>().SetPosition(reference);

        //Switch Current Player
        controller.GetComponent<Controller>().NextTurn();

        //Destroy the move plates including self
        reference.GetComponent<ChessmanScript>().DestroyMovePlates();
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }
}
