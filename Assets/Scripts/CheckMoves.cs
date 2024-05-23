using System.Collections.Generic;
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
                // Is the pawn on the starting square?
                if (piece.rank == 2)
                {
                    legal = new Square[4];

                    //Two Squares in front of the pawn
                    candidate = FindSquare(piece.rank + 2, piece.file);
                    if (ValidateSquare(candidate, piece.isWhite))
                        legal[3] = candidate;
                }
                else
                    legal = new Square[3];

                //Square one up and one right
                candidate = FindSquare(
                    piece.isWhite ? piece.rank + 1 : piece.rank - 1,
                    piece.isWhite ? piece.file + 1 : piece.file - 1);
                if (ValidateSquare(candidate, piece.isWhite))
                    legal[0] = candidate;

                //Square one up and one right
                candidate = FindSquare(
                    piece.isWhite ? piece.rank - 1 : piece.rank + 1,
                    piece.isWhite ? piece.file - 1 : piece.file + 1);
                if (ValidateSquare(candidate, piece.isWhite))
                    legal[1] = candidate;

                //Square in front of the pawn
                candidate = FindSquare(piece.isWhite ? piece.rank + 1 : piece.rank - 1, piece.file);
                if (ValidateSquare(candidate, piece.isWhite))
                    legal[2] = candidate;
                break;
            case PieceType.Rook:
                legal = new Square[14];

                //Search Rank left side
                for (int i = 0; i < piece.file; i++)
                {
                    candidate = FindSquare(piece.rank, i);
                    if (ValidateSquare(candidate, piece.isWhite))
                    {
                        legal[legal.GetLastIndex()] = candidate;
                    }
                    else
                        break;
                }
                //Search Rank right side
                for (int i = piece.file; i < 8; i++)
                {
                    candidate = FindSquare(piece.rank, i);
                    if (ValidateSquare(candidate, piece.isWhite))
                        legal[legal.GetLastIndex()] = candidate;
                    else
                        break;
                }

                //Search File
                break;
            case PieceType.Knight:
                break;
            case PieceType.Bishop:
                break;
            case PieceType.Queen:
                break;
            case PieceType.King:
                break;
            default:
                break;
        }

        return legal;
    }

    static Square FindSquare(int rank, int file)
    {
        Collider[] hitObjects = Physics.OverlapSphere(new Vector3(file, rank), 0.5f);
        if (hitObjects.Length == 0)
            return null;
        foreach (Collider collider in hitObjects)
        {
            if (collider.TryGetComponent<Square>(out Square square))
                return square;
        }
        return null;
    }

    static bool ValidateSquare(Square square, bool isWhite)
    {
        if (square == null)
            return false;
        if (square.piece == null)
            return false;
        if (square.piece.isWhite == isWhite)
            return false;

        return true;
    }
}
