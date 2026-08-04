using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private void Update()
    {
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                MovePiece();
            }
        }
    }

    private void MovePiece()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        if (controller.GetComponent<Controller>().HasPlayerMoved())
        {
            return;
        }

        if (attack)
        {
            GameObject cp = controller.GetComponent<Controller>().GetPosition(matrixX, matrixY);

            // if (cp.name == "white_king") controller.GetComponent<Controller>().SetGameOver(true);
            // if (cp.name == "black_king") controller.GetComponent<Controller>().SetGameOver(true);

            Destroy(cp);
        }

        controller.GetComponent<Controller>().SetPositionEmpty(
            reference.GetComponent<ChessmanScript>().GetXBoard(),
            reference.GetComponent<ChessmanScript>().GetYBoard());

        reference.GetComponent<ChessmanScript>().SetXBoard(matrixX);
        reference.GetComponent<ChessmanScript>().SetYBoard(matrixY);
        //reference.GetComponent<ChessmanScript>().SetCoords();
        reference.GetComponent<ChessmanScript>().MoveToCoords(matrixX, matrixY, 0.2f);

        controller.GetComponent<Controller>().SetPlayerHasMoved(true);

        controller.GetComponent<Controller>().SetPosition(reference);

        //controller.GetComponent<Controller>().NextTurn();

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
