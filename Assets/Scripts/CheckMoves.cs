using System;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
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

                HorizonalSquares(piece, ref legal);
                break;
            case PieceType.Knight:
                legal = new Square[8];

                //Search the flattened L by rotating it using the coordinate system
                for (int i = 0; i < 4; i++)
                {
                    candidate = Square.GetSquareAtPos(
                        piece.rank + (i % 4 == 1 || i % 4 == 2 ? -2 : 2),
                        piece.file + (i % 4 == 2 || i % 4 == 3 ? -1 : 1));
                    if (ValidateSquare(candidate, piece.isWhite))
                    {
                        if (candidate.piece == null)
                            legal[legal.GetLastIndex()] = candidate;
                    }
                }

                //Search the heightened L by rotating it using the coordinate system
                for (int i = 0; i < 4; i++)
                {
                    candidate = Square.GetSquareAtPos(
                        piece.rank + (i % 4 == 1 || i % 4 == 2 ? -1 : 1),
                        piece.file + (i % 4 == 2 || i % 4 == 3 ? -2 : 2));
                    if (ValidateSquare(candidate, piece.isWhite))
                    {
                        if (candidate.piece == null)
                            legal[legal.GetLastIndex()] = candidate;
                    }
                }
                break;
            case PieceType.Bishop:
                legal = new Square[13];

                DiagonalSquares(piece, ref legal);
                break;
            case PieceType.Queen:
                legal = new Square[27];

                HorizonalSquares(piece, ref legal);
                DiagonalSquares(piece, ref legal);
                break;
            case PieceType.King:
                legal = new Square[8];
                Square[] kingCandidates = new Square[8];

                //Getting all candidate squares that the king can move to
                kingCandidates[0] = Square.GetSquareAtPos(piece.rank + 1, piece.file - 1);
                kingCandidates[1] = Square.GetSquareAtPos(piece.rank + 1, piece.file);
                kingCandidates[2] = Square.GetSquareAtPos(piece.rank + 1, piece.file + 1);
                kingCandidates[3] = Square.GetSquareAtPos(piece.rank, piece.file - 1);
                kingCandidates[4] = Square.GetSquareAtPos(piece.rank, piece.file + 1);
                kingCandidates[5] = Square.GetSquareAtPos(piece.rank - 1, piece.file - 1);
                kingCandidates[6] = Square.GetSquareAtPos(piece.rank - 1, piece.file);
                kingCandidates[7] = Square.GetSquareAtPos(piece.rank - 1, piece.file + 1);

                //Checking all candidate squares for the king to move to
                foreach (var kingCandidate in kingCandidates)
                    if (ValidateSquare(kingCandidate, piece.isWhite) && (kingCandidate.piece == null))
                        legal[legal.GetLastIndex()] = kingCandidate;
                break;
            default:
                return null;
        }

        return legal;
    }

    static void DiagonalSquares(Piece piece, ref Square[] legal)
    {
        Square candidate;

        //Up to the Right Diagonal
        int i = 0, j = 0;
        while (true)
        {
            candidate = Square.GetSquareAtPos(piece.rank + ++i, piece.file + ++j);
            if (ValidateSquare(candidate, piece.isWhite))
            {
                legal[legal.GetLastIndex()] = candidate;
                if (candidate.piece != null)
                    break;
                continue;
            }
            break;
        }

        //Up to the Left Diagonal
        i = 0; j = 0;
        while (true)
        {
            candidate = Square.GetSquareAtPos(piece.rank + --i, piece.file + ++j);
            if (ValidateSquare(candidate, piece.isWhite))
            {
                legal[legal.GetLastIndex()] = candidate;
                if (candidate.piece != null)
                    break;
                continue;
            }
            break;
        }

        //Down to the Right Diagonal
        i = 0; j = 0;
        while (true)
        {
            candidate = Square.GetSquareAtPos(piece.rank + ++i, piece.file + --j);
            if (ValidateSquare(candidate, piece.isWhite))
            {
                legal[legal.GetLastIndex()] = candidate;
                if (candidate.piece != null)
                    break;
                continue;
            }
            break;
        }

        //Down to the Left Diagonal
        i = 0; j = 0;
        while (true)
        {
            candidate = Square.GetSquareAtPos(piece.rank + --i, piece.file + --j);
            if (ValidateSquare(candidate, piece.isWhite))
            {
                legal[legal.GetLastIndex()] = candidate;
                if (candidate.piece != null)
                    break;
                continue;
            }
            break;
        }
    }

    static void HorizonalSquares(Piece piece, ref Square[] legal)
    {
        Square candidate;
        
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
