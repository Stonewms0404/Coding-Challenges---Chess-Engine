using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isWhitesTurn;
    Board currBoard;

    private void OnEnable()
    {
        StartBoard.OnStartBoard += GetChessBoard;
        Square.EndTurn += Square_EndTurn;
        Piece.OnEnPassant += Piece_EnPassant;
    }

    private void OnDisable()
    {
        StartBoard.OnStartBoard -= GetChessBoard;
        Square.EndTurn -= Square_EndTurn;
        Piece.OnEnPassant += Piece_EnPassant;
    }

    private void Square_EndTurn(bool isWhite)
    {
        isWhitesTurn = !isWhite;
        foreach (Piece piece in currBoard.whitePieces) 
            piece.isTurn = isWhitesTurn;
        foreach (Piece piece in currBoard.blackPieces) 
            piece.isTurn = !isWhitesTurn;

        //Updates every square to check if there are pieces on them and updates them
        foreach (Square square in Square.GetAllSquares())
        {
            Collider[] hitObjects = Physics.OverlapSphere(square.transform.position, 0.25f);
            foreach (Collider hitObject in hitObjects)
            {
                hitObject.TryGetComponent<Piece>(out Piece piece);
                if (piece == null)
                    square.piece = null;
                else 
                    square.piece = piece;
            }
        }
    }

    private void Piece_EnPassant(Piece EPpiece)
    {
        foreach (Piece piece in Piece.GetAllPieces())
            piece.EnPassant = false;
        EPpiece.EnPassant = true;
    }

    private void GetChessBoard(Board board)
    {
        currBoard = board;
    }
}
