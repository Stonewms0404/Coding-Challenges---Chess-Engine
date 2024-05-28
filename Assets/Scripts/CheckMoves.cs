using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public static class CheckMoves
{
    public static Square[] GetLegalMoves(Piece piece)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Square");
        Square[] squares = new Square[64], legal = null;

        for (int i = 0; i < objs.Length; i++)
            squares[i] = objs[i].GetComponent<Square>();

        Square candidate;
        switch (piece.type)
        {
            case PieceType.Pawn:
                legal = new Square[4];

                //Square one up and one right
                candidate = Square.GetSquareAtPos(
                    piece.isWhite ? piece.rank + 1 : piece.rank - 1,
                    piece.isWhite ? piece.file + 1 : piece.file - 1);
                if (ValidateSquare(candidate, piece.isWhite))
                {
                    if (IsEnPassant(piece.rank, piece.isWhite ? piece.file + 1 : piece.file - 1, piece.isWhite) && candidate.piece == null)
                        legal[0] = candidate;
                    else if (candidate.piece != null)
                        legal[0] = candidate;
                }

                //Square one up and one left
                candidate = Square.GetSquareAtPos(
                    piece.isWhite ? piece.rank + 1 : piece.rank - 1,
                    piece.isWhite ? piece.file - 1 : piece.file + 1);
                if (ValidateSquare(candidate, piece.isWhite) && candidate.piece)
                {
                    if (IsEnPassant(piece.rank, piece.isWhite ? piece.file - 1 : piece.file + 1, piece.isWhite) && candidate.piece == null)
                        legal[1] = candidate;
                    else if (candidate.piece != null)
                        legal[1] = candidate;
                }

                //Square in front of the pawn
                candidate = Square.GetSquareAtPos(piece.isWhite ? piece.rank + 1 : piece.rank - 1, piece.file);
                if (ValidateSquare(candidate, piece.isWhite) && (candidate.piece == null))
                    legal[2] = candidate;

                // Is the pawn on the starting square?
                if ((piece.rank == 2 && piece.isWhite) || (piece.rank == 7 && !piece.isWhite))
                {
                    if (legal[2] == null)
                        break;
                    //Two Squares in front of the pawn
                    candidate = Square.GetSquareAtPos(
                    piece.isWhite ? piece.rank + 2 : piece.rank - 2, piece.file);
                    if (ValidateSquare(candidate, piece.isWhite) && (candidate.piece == null))
                        legal[3] = candidate;
                }
                break;
            case PieceType.Rook:
                legal = new Square[14];

                //Search Rank right side
                for (int i = piece.file + 1; i <= 8; i++)
                {
                    candidate = Square.GetSquareAtPos(piece.rank, i);
                    if (ValidateSquare(candidate, piece.isWhite))
                    {
                        legal[legal.GetLastIndex()] = candidate;
                        if (candidate.piece == null)
                            continue;
                        break;
                    }
                    break;
                }
                //Search Rank left side
                for (int i = piece.file - 1; i > 0; i--)
                {
                    candidate = Square.GetSquareAtPos(piece.rank, i);
                    if (ValidateSquare(candidate, piece.isWhite))
                    {
                        legal[legal.GetLastIndex()] = candidate;
                        if (candidate.piece == null)
                            continue;
                        break;
                    }
                    break;
                }

                //Search File Up
                for (int i = piece.rank + 1; i <= 8; i++)
                {
                    candidate = Square.GetSquareAtPos(i, piece.file);
                    if (ValidateSquare(candidate, piece.isWhite))
                    {
                        legal[legal.GetLastIndex()] = candidate;
                        if (candidate.piece == null)
                            continue;
                        break;
                    }
                    break;
                }
                //Search File Down
                for (int i = piece.rank - 1; i > 0; i--)
                {
                    candidate = Square.GetSquareAtPos(i, piece.file);
                    if (ValidateSquare(candidate, piece.isWhite))
                    {
                        legal[legal.GetLastIndex()] = candidate;
                        if (candidate.piece == null)
                            continue;
                        break;
                    }
                    break;
                }
                break;
            case PieceType.Knight:
                Vector2 horL = new(piece.file + 1, piece.rank + 2);
                Vector2 verL = new(piece.file + 2, piece.rank + 1);

                candidate = Square.GetSquareAtPos((int)horL.x, (int)horL.y);
                //if (ValidateSquare(candidate, piece.isWhite) || candidate.piece == null)
                    


                break;
            case PieceType.Bishop:
                break;
            case PieceType.Queen:
                break;
            case PieceType.King:
                break;
            default:
                return null;
        }

        return legal;
    }

    static bool ValidateSquare(Square square, bool isWhite)
    {
        if (square == null)
            return false;
        if (square.piece != null)
            if (square.piece.isWhite == isWhite)
                return false;

        return true;
    }

    static bool IsEnPassant(int rank, int file, bool isWhite)
    {
        Collider[] hitObjects = Physics.OverlapSphere(new Vector3(file - 1, rank - 1), 0.25f);
        if (hitObjects.Length == 0)
            return false;

        foreach (Collider collider in hitObjects)
        {
            if (collider.TryGetComponent<Square>(out Square square))
            {
                if (square.rank != rank && square.file != file)
                    continue;
                if (square.piece == null)
                    continue;

                return square.piece.EnPassant;
            }
        }

        return false;
    }
}
