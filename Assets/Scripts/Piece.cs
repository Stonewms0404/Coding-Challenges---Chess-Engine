using Unity.VisualScripting;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Sprite sprite;
    public Color color;
    public PieceType type;
    public int file, rank;
    public bool isWhite, selected;

    SpriteRenderer rend;

    private void Start()
    {
        MovePiece(rank, file);

        rend = gameObject.AddComponent<SpriteRenderer>();
        rend.sprite = sprite;
        tag = "Piece";

        gameObject.AddComponent<BoxCollider2D>();
    }
    public void SetColor(Color color)
    {
        this.color = color;
        rend.color = color;
    }

    public void MovePiece(int newRank, int newFile)
    {
        rank = newRank;
        file = newFile;

        transform.position = new(file - 1, rank - 1, -1);
    }

    private void OnMouseOver()
    {
        rend.color = Color.red;
    }
    private void OnMouseExit()
    {
        if (!selected)
            rend.color = color;

    }

    private void OnMouseDown()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Square");
        Square[] squares = new Square[objects.Length];
        int i = 0;
        foreach (GameObject obj in objects)
            squares[i] = obj.GetComponent<Square>();

        foreach (Square square in squares)
            if (square != null)
            {
                square.SetColor(square.color);
                square.isLegal = false;
            }

        selected = !selected;
        objects = GameObject.FindGameObjectsWithTag("Piece");
        Piece[] pieces = new Piece[objects.Length];
        foreach (Piece piece in pieces)
            if (piece != null && piece != this)
            {
                piece.selected = false;
                piece.SetColor(piece.color);
            }

        if (!selected)
            return;

        Square[] legalSquares = CheckMoves.GetLegalMoves(this);

        foreach (Square square in legalSquares)
            if (square != null)
                square.SelectSquare(true);
    }
}


public enum PieceType
{
    Pawn,
    Knight,
    Bishop,
    Rook,
    King,
    Queen,
    Empty
}