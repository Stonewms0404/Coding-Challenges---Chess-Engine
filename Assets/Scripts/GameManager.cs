using System;
using UnityEditor.XR;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Camera mainCam;
    public bool isWhitesTurn, TwoPersonPlay;
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
        foreach (Piece piece in Piece.GetAllPieces())
            if (piece)
                piece.isTurn = piece.isWhite ? isWhitesTurn : !isWhitesTurn;

        //Flip perspective
        if (TwoPersonPlay)
        {
            mainCam.transform.rotation = Quaternion.Euler(0, 0, isWhitesTurn ? 0 : 180);
            foreach (Piece piece in Piece.GetAllPieces())
                piece.transform.rotation = Quaternion.Euler(0, 0, (isWhitesTurn) ? 0 : 180);
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
