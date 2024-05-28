using System;
using Unity.VisualScripting;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public static event Action<Piece> OnEnPassant;

    public Sprite sprite;
    public Color color;
    public PieceType type;
    public int file, rank;
    public bool isWhite, selected, isTurn, EnPassant;

    SpriteRenderer rend;

    private void Start()
    {
        transform.position = new(file - 1, rank - 1, -1);

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

    public Piece OnMovePiece(int newRank, int newFile)
    {
        rank = newRank;
        file = newFile;

        transform.position = new(file - 1, rank - 1, -1);

        SetAllDefault();
        if (ExtraMethods.AbsDist(rank, newRank) == 2 && type == PieceType.Pawn)
            EnPassant = true;

        if (EnPassant)
            OnEnPassant(this);

        foreach(Square square in Square.GetAllSquares())
            square.CheckPiece();

        return this;
    }

    public void Capture()
    {
        Destroy(rend);
        Destroy(gameObject);
    }

    private void OnMouseOver()
    {
        if (!isTurn)
            return;
        rend.color = isWhite ? Color.red : new(0.75f, 0, 0, 1);
    }
    private void OnMouseExit()
    {
        if (!selected)
            rend.color = color;
    }

    private void OnMouseDown()
    {
        SetAllDefault();

        if (!isTurn)
            return;
        selected = !selected;

        if (!selected)
            return;
        Square.MovePiece += OnMovePiece;

        Square[] legalSquares = CheckMoves.GetLegalMoves(this);

        foreach (Square square in legalSquares)
            if (square != null)
                square.SelectSquare(true);
    }

    private void OnDestroy()
    {
        SetAllDefault();
        Square.MovePiece -= OnMovePiece;
    }
    public static void SetAllDefault()
    {
        Square[] squares = Square.GetAllSquares();
        foreach (Square square in squares)
            if (square != null)
            {
                square.SetColor(square.color);
                square.isLegal = false;
            }

        Piece[] pieces = Piece.GetAllPieces();
        foreach (Piece piece in pieces)
            if (piece != null)
            {
                piece.selected = false;
                Square.MovePiece -= piece.OnMovePiece;
                piece.SetColor(piece.color);
            }
    }

    public static Piece[] GetAllPieces()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Piece");
        Piece[] pieces = new Piece[objects.Length];

        for (int i = 0; i < objects.Length; i++)
            pieces[i] = objects[i].GetComponent<Piece>();

        return pieces;
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