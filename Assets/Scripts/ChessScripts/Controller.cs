using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    public GameObject chesspiece;

    //Matrices needed, positions of each of the GameObjects
    //Also separate arrays for the players in order to easily keep track of them all
    //Keep in mind that the same objects are going to be in "positions" and "playerBlack"/"playerWhite"
    private GameObject[,] positions = new GameObject[8, 8];
    private GameObject[] playerBlack = new GameObject[16];
    private GameObject[] playerWhite = new GameObject[16];
    private GameObject WinningPiece;
    private int challengeNum;
    private bool hasPlayerMoved = false;
    private bool playerUseWinPos = false;


    //current turn
    private string currentPlayer = "white";

    //Game Ending
    private bool gameOver = false;

    public void Start()
    {
        // Sets a random seed based on the player's PC current clock miliseconds
        int uniqueSeed = (int)System.DateTime.Now.Ticks;
        UnityEngine.Random.InitState(uniqueSeed);

        challengeNum = UnityEngine.Random.Range(1, 6); // Picks a random 1 out of the 5 chess puzzles

        switch(challengeNum)
        {
            case 1:
                ChessChallenge1();
                break;

            case 2:
                ChessChallenge2();
                break;

            case 3:
                ChessChallenge3();
                break;

            case 4:
                ChessChallenge4();
                break;

            case 5:
                ChessChallenge5();
                break;
        }

        //Set all piece positions on the positions board
        for (int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]);
        }

        for (int i = 0; i < playerWhite.Length; i++)
        {
            SetPosition(playerWhite[i]);
        }
    }

    public void Update()
    {
        if (gameOver == true && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            gameOver = false;

            SceneManager.LoadScene("ChessMinigame"); //Restarts the game by loading the scene over again
        }

        // Switch-case to check for winning condition based on chosen challenge
        switch(challengeNum)
            {
                case 1: 
                    if (GetPosition(3, 4) == WinningPiece && !playerUseWinPos)
                    {
                        Debug.Log("WINNN");
                        playerUseWinPos = true;
                        StartCoroutine(PlaySequence1());
                        //SetGameOver(true);
                    }
                    break;

                case 2: 
                    if (GetPosition(2, 2) == WinningPiece && !playerUseWinPos)
                    {
                        Debug.Log("WINNN");
                        playerUseWinPos = true;
                        StartCoroutine(PlaySequence2());
                        //SetGameOver(true);
                    }
                    break;               

                case 3:
                    if (GetPosition(6, 3) == WinningPiece && !playerUseWinPos)
                    {
                        Debug.Log("WINNN");
                        playerUseWinPos = true;
                        StartCoroutine(PlaySequence3());
                        //SetGameOver(true);
                    }
                    break;

                case 4:
                    if (GetPosition(3, 7) == WinningPiece && !playerUseWinPos)
                    {
                        Debug.Log("WINNN");
                        playerUseWinPos = true;
                        StartCoroutine(PlaySequence4());
                        //SetGameOver(true);                        
                    }
                    break;

                case 5:
                    if (GetPosition(7, 7) == WinningPiece && !playerUseWinPos)
                    {
                        Debug.Log("WINNN");
                        playerUseWinPos = true;
                        StartCoroutine(PlaySequence5());
                    }
                    break;

                default:
                    Debug.Log("Losee"); break;

            }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            CheckMouseClick();
        }

    }

    public bool HasPlayerMoved()
    {
        return hasPlayerMoved;
    }

    public void SetPlayerHasMoved(bool val)
    {
        hasPlayerMoved = val;
    }

    private void CheckMouseClick()
    {
        // Get mouse position on screen
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        // Convert screen position into world position
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Check what object is at that position
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

    }

    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        ChessmanScript cm = obj.GetComponent<ChessmanScript>(); //We have access to the GameObject, we need the script
        cm.name = name; //This is a built in variable that Unity has, so we did not have to declare it before
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.Activate(); //It has everything set up so it can now Activate()
        return obj;
    }

    public void SetPosition(GameObject obj)
    {
        ChessmanScript cm = obj.GetComponent<ChessmanScript>();

        //Overwrites either empty space or whatever was there
        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) return false;
        return true;
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void SetGameOver(bool val)
    {
        gameOver = val;
    }

    public void NextTurn()
    {
        if (currentPlayer == "white")
        {
            currentPlayer = "black";
        }
        else
        {
            currentPlayer = "white";
        }
    }

    public void MovePieceTo(GameObject piece, int targetX, int targetY, float moveDuration = 0.25f)
    {

        ChessmanScript cm = piece.GetComponent<ChessmanScript>();
        if (cm == null) return;

        int currentX = cm.GetXBoard();
        int currentY = cm.GetYBoard();

        // 1. Check if an enemy piece exists at target location and capture it
        GameObject targetPiece = GetPosition(targetX, targetY);
        if (targetPiece != null && targetPiece != piece)
        {
            Destroy(targetPiece);
        }

        // 2. Clear old matrix slot
        SetPositionEmpty(currentX, currentY);

        // 3. Move piece script values and start smooth slide
        cm.MoveToCoords(targetX, targetY, moveDuration);

        // 4. Update matrix slot to store piece at new position
        SetPosition(piece);
    }
    
    public IEnumerator PlaySequence1()
    {
        yield return new WaitForSeconds(0.5f);

        // 1. Move Black Bishop at (5, 4) to (4, 5) 
        GameObject blackBishop = GetPosition(5, 4);
        MovePieceTo(blackBishop, 4, 5, 0.3f);

        yield return new WaitForSeconds(0.8f); // Pause between moves

        // 2. Move Black Pawn at (4, 6) to (4, 4) -> Black King's Pawn e7-e5
        GameObject whiteBishop = GetPosition(3, 4);
        MovePieceTo(whiteBishop, 5, 4, 0.3f);

        yield return new WaitForSeconds(0.8f);

        // 3. Move White Knight at (6, 0) to (5, 2) -> Nf3
        GameObject blackKing = GetPosition(6, 7);
        MovePieceTo(blackKing, 7, 7, 0.3f);

        yield return new WaitForSeconds(0.8f);

        GameObject whiteRook = GetPosition(4, 7);
        MovePieceTo(whiteRook, 5, 7, 0.3f);
    }

    public IEnumerator PlaySequence2() // TRUE
    {
        yield return new WaitForSeconds(0.5f);

        GameObject blackKing = GetPosition(4, 7);
        MovePieceTo(blackKing, 2, 7, 0.3f);

        GameObject blackRook = GetPosition(0, 7);
        MovePieceTo(blackRook, 3, 7, 0.3f);

        yield return new WaitForSeconds(0.8f);

        GameObject whiteBishop = GetPosition(5, 0);
        MovePieceTo(whiteBishop, 3, 2, 0.3f);

        yield return new WaitForSeconds(0.8f);

        GameObject blackPawn = GetPosition(0, 6);
        MovePieceTo(blackPawn, 0, 5, 0.3f);

        yield return new WaitForSeconds(0.8f);

        GameObject whiteKing = GetPosition(4, 0);
        MovePieceTo(whiteKing, 6, 0, 0.3f);

        GameObject whiteRook = GetPosition(7, 0);
        MovePieceTo(whiteRook, 5, 0, 0.3f);

        yield return new WaitForSeconds(0.8f);

        blackRook = GetPosition(3, 7);
        MovePieceTo(blackRook, 4, 7, 0.3f);

        yield return new WaitForSeconds(0.8f);

        whiteRook = GetPosition(5, 0);
        MovePieceTo(whiteRook, 5, 6, 0.3f);

        yield return new WaitForSeconds(0.8f);

        blackRook = GetPosition(4, 7);
        MovePieceTo(blackRook, 4, 3, 0.3f);

        yield return new WaitForSeconds(0.8f);

        whiteKing = GetPosition(6, 0);
        MovePieceTo(whiteKing, 7, 0, 0.3f);

        yield return new WaitForSeconds(0.8f);

        blackPawn = GetPosition(7, 6);
        MovePieceTo(blackPawn, 7, 5, 0.3f);

        yield return new WaitForSeconds(0.8f);

        whiteBishop = GetPosition(3, 2);
        MovePieceTo(whiteBishop, 5, 0, 0.3f);

        yield return new WaitForSeconds(0.8f);

        GameObject blackKnight = GetPosition(0, 4);
        MovePieceTo(blackKnight, 1, 2, 0.3f);

        yield return new WaitForSeconds(0.8f);

        whiteRook = GetPosition(5, 6);
        MovePieceTo(whiteRook, 5, 7, 0.3f);

    }

    public IEnumerator PlaySequence3()
    {
        yield return new WaitForSeconds(0.5f);

        GameObject blackKing = GetPosition(7, 4);
        MovePieceTo(blackKing, 7, 3, 0.3f);

        yield return new WaitForSeconds(0.8f);

        GameObject whiteQueen = GetPosition(5, 5);
        MovePieceTo(whiteQueen, 7, 5, 0.3f);

        yield return new WaitForSeconds(0.8f);

        GameObject blackKnight = GetPosition(6, 6);
        MovePieceTo(blackKnight, 7, 4, 0.3f);

        yield return new WaitForSeconds(0.8f);

        whiteQueen = GetPosition(7, 5);
        MovePieceTo(whiteQueen, 7, 4, 0.3f);
    }

    public IEnumerator PlaySequence4()
    {
        yield return new WaitForSeconds(0.5f);

        GameObject blackBishop = GetPosition(0, 3);
        MovePieceTo(blackBishop, 4, 7, 0.3f);

        yield return new WaitForSeconds(0.8f);

        GameObject whiteRook = GetPosition(3, 7);
        MovePieceTo(whiteRook, 4, 7, 0.3f);

        yield return new WaitForSeconds(0.8f);

        GameObject blackRook = GetPosition(5, 6);
        MovePieceTo(blackRook, 5, 7, 0.3f);

        yield return new WaitForSeconds(0.8f);

        whiteRook = GetPosition(4, 7);
        MovePieceTo(whiteRook, 5, 7, 0.3f);

    }

    public IEnumerator PlaySequence5()
    {
        yield return new WaitForSeconds(0.5f);

        GameObject blackKing = GetPosition(5, 7);
        MovePieceTo(blackKing, 4, 6, 0.3f);

        yield return new WaitForSeconds(0.8f);

        GameObject whiteQueen = GetPosition(7, 7);
        MovePieceTo(whiteQueen, 6, 6, 0.3f);

        yield return new WaitForSeconds(0.8f);

        blackKing = GetPosition(4, 6);
        MovePieceTo(blackKing, 3, 7, 0.3f);

        yield return new WaitForSeconds(0.8f);

        whiteQueen = GetPosition(6, 6);
        MovePieceTo(whiteQueen, 2, 6, 0.3f);
    }

    private void CompleteChessSet()
    {
        playerWhite = new GameObject[] { Create("white_rook", 0, 0), Create("white_knight", 1, 0),
        Create("white_bishop", 2, 0), Create("white_queen", 3, 0), Create("white_king", 4, 0),
        Create("white_bishop", 5, 0), Create("white_knight", 6, 0), Create("white_rook", 7, 0),
        Create("white_pawn", 0, 1), Create("white_pawn", 1, 1), Create("white_pawn", 2, 1),
        Create("white_pawn", 3, 1), Create("white_pawn", 4, 1), Create("white_pawn", 5, 1),
        Create("white_pawn", 6, 1), Create("white_pawn", 7, 1) };
        
        playerBlack = new GameObject[] { Create("black_rook", 0, 7), Create("black_knight",1,7),
        Create("black_bishop",2,7), Create("black_queen",3,7), Create("black_king",4,7),
        Create("black_bishop",5,7), Create("black_knight",6,7), Create("black_rook",7,7),
        Create("black_pawn", 0, 6), Create("black_pawn", 1, 6), Create("black_pawn", 2, 6),
        Create("black_pawn", 3, 6), Create("black_pawn", 4, 6), Create("black_pawn", 5, 6),
        Create("black_pawn", 6, 6), Create("black_pawn", 7, 6) };
    }



    private void ChessChallenge1()
    {
        // WHITE TO MOVE: Check the king with the white_bishop

        currentPlayer = "white";

        WinningPiece = Create("white_bishop", 5, 2);

        playerWhite = new GameObject[] { Create("white_king", 7, 0), Create("white_pawn", 0, 1), Create("white_pawn", 7, 1),
        Create("white_pawn", 2, 2), WinningPiece, Create("white_rook", 0, 6), Create("white_rook", 4, 7)};
            
        playerBlack = new GameObject[] { Create("black_rook", 1, 1), Create("black_knight", 3, 2), Create("black_pawn", 2, 4),
        Create("black_bishop", 5, 4), Create("black_pawn", 3, 5), Create("black_pawn", 6, 5), Create("black_pawn", 7, 6),
        Create("black_rook", 5, 7), Create("black_king", 6, 7)};
    }

    private void ChessChallenge2() // TRUE chess 
    {
        currentPlayer = "white";

        WinningPiece = Create("white_knight", 1, 0);

        playerWhite = new GameObject[] { WinningPiece, Create("white_queen", 3, 0), Create("white_king", 4, 0),
        Create("white_bishop", 5, 0), Create("white_pawn", 6, 1), Create("white_pawn", 7, 1), Create("white_pawn", 6, 2),
        Create("white_pawn", 3, 3), Create("white_pawn", 4, 3), Create("white_rook", 7, 0)};
            
        playerBlack = new GameObject[] { Create("black_rook", 0, 7), Create("black_king", 4, 7), Create("black_pawn", 0, 6),
        Create("black_bishop", 1, 6), Create("black_pawn", 2, 6), Create("black_pawn", 3, 6), Create("black_pawn", 5, 6),
        Create("black_pawn", 6, 6), Create("black_pawn", 7, 6), Create("black_knight", 0, 4)};
    }

    private void ChessChallenge3()
    {
        currentPlayer = "white";

        WinningPiece = Create("white_pawn", 6, 2);

        playerWhite = new GameObject[] { Create("white_pawn", 5, 1), Create("white_king", 7, 1), WinningPiece,
        Create("white_pawn", 7, 2), Create("white_pawn", 0, 2), Create("white_queen", 5, 5)};
            
        playerBlack = new GameObject[] { Create("black_queen", 0, 1), Create("black_pawn", 3, 3), Create("black_pawn", 6, 4),
        Create("black_king", 7, 4), Create("black_pawn", 0, 5), Create("black_knight", 6, 6), Create("black_pawn", 7, 6)};
    }

    private void ChessChallenge4()
    {
        // White Rook (3,1) takes Black Rook (3, 7)
        currentPlayer = "white";

        WinningPiece = Create("white_rook", 3, 1);

        playerWhite = new GameObject[] { Create("white_queen", 1, 0), Create("white_king", 3, 0), Create("white_rook", 7, 0),
        Create("white_pawn", 0, 1), WinningPiece, Create("white_pawn", 5, 1), Create("white_pawn", 1, 2),
        Create("white_pawn", 7, 2), Create("white_pawn", 2, 3), Create("white_bishop", 7, 6)};
            
        playerBlack = new GameObject[] { Create("black_queen", 2, 2), Create("black_bishop", 0, 3), Create("black_pawn", 1, 3),
        Create("black_knight", 5, 3), Create("black_pawn", 0, 4), Create("black_pawn", 4, 5), Create("black_rook", 5, 6), Create("black_pawn", 6, 6),
        Create("black_rook", 3, 7), Create("black_king", 7, 7)};
    }

    private void ChessChallenge5()
    {
        // White Queen to check black king
        currentPlayer = "white";

        WinningPiece = Create("white_queen", 7, 6);

        playerWhite = new GameObject[] { Create("white_king", 1, 0), Create("white_pawn", 1, 1), Create("white_pawn", 2, 1),
        Create("white_pawn", 5, 1), Create("white_pawn", 0, 2), Create("white_bishop", 5, 3), Create("white_knight", 6, 4),
        WinningPiece};
            
        playerBlack = new GameObject[] { Create("black_pawn", 3, 2), Create("black_knight", 2, 3), Create("black_pawn", 6, 3),
        Create("black_pawn", 1, 4), Create("black_queen", 3, 4), Create("black_pawn", 5, 4), Create("black_pawn", 0, 5), Create("black_pawn", 4, 5),
        Create("black_pawn", 6, 6), Create("black_rook", 0, 7), Create("black_rook", 4, 7), Create("black_king", 5, 7)};
    }

}