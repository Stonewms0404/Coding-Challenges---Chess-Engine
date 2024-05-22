using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class StartBoard : MonoBehaviour
{
    public string fenString;

    [SerializeField] Color lightColor, darkColor;
    [SerializeField] Sprite[] pieceSprites;
    [SerializeField] Vector3 pieceScale;

    Board ChessBoard;
    GameObject PiecesParent;

    private void Start()
    {
        PiecesParent = new("Pieces Parent");
        NewBoard();
    }

    private void Update()
    {
        foreach (Square square in ChessBoard.Squares)
        {
            if (square.color != lightColor || square.color != darkColor)
                square.SetColor(square.isLight ? lightColor : darkColor);
            if (square.piece)
                square.piece.transform.localScale = pieceScale;
        }
    }

    void NewBoard()
    {
        ChessBoard = new Board(darkColor, lightColor);
        SpawnSquares();
        SpawnPieces();
    }

    void SpawnSquares()
    {
        if (ChessBoard.Squares == null)
            ChessBoard.Squares = new Square[64];
        for (int i = 0; i < 64; i++)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Quad);
            obj.name = "Square " + (i + 1);

            obj.transform.position = new(i % 8, (int)(i / 8));
            Vector2 pos = obj.transform.position;
            Square square = obj.AddComponent<Square>();
            square.index = i + 1;
            square.rank = (int)(i / 8) + 1;
            square.file = (i % 8) + 1;
            if ((pos.y + pos.x) %  2 == 0)
            {
                square.SetColor(lightColor);
                square.isLight = true;
            }
            else
            {
                square.SetColor(darkColor);
                square.isLight = false;
            }

            ChessBoard.Squares[i] = square;
        }
    }

    void SpawnPieces()
    {
        if (fenString == null || fenString.Length == 0)
        {
            foreach (Square square in ChessBoard.Squares)
            {
                GameObject obj = new();
                Piece piece = obj.AddComponent<Piece>();
                //Spawn Pawn
                if (square.rank == 2 || square.rank == 7)
                    piece.type = PieceType.Pawn;
                //Spawn Rook
                else if ((square.file == 1 || square.file == 8) && (square.rank == 1 || square.rank == 8))
                    piece.type = PieceType.Rook;
                //SpawnKnight
                else if ((square.file == 2 || square.file == 7) && (square.rank == 1 || square.rank == 8))
                    piece.type = PieceType.Knight;
                //Spawn Bishop
                else if ((square.file == 3 || square.file == 6) && (square.rank == 1 || square.rank == 8))
                    piece.type = PieceType.Bishop;
                //Spawn King
                else if ((square.file == 4) && (square.rank == 1 || square.rank == 8))
                    piece.type = PieceType.King;
                //Spawn Queen
                else if ((square.file == 5) && (square.rank == 1 || square.rank == 8))
                    piece.type = PieceType.Queen;
                else
                    piece.type = PieceType.Empty;

                piece.rank = square.rank;
                piece.file = square.file;
                piece.isWhite = square.rank == 1 || square.rank == 2;

                piece.name = piece.isWhite ? "White" : "Black" + " " + piece.type.ToString();

                piece = SetPieceSprite(piece);
                if (piece.isWhite)
                    ChessBoard.whitePieces.Add(piece);
                else
                    ChessBoard.blackPieces.Add(piece);
                square.piece = piece;

                piece.gameObject.transform.SetParent(PiecesParent.transform, true);

            }
        }
    }

    private Piece SetPieceSprite(Piece piece)
    {
        piece.sprite = piece.type switch
        {
            PieceType.Pawn => piece.isWhite ? pieceSprites[5] : pieceSprites[11],
            PieceType.Rook => piece.isWhite ? pieceSprites[4] : pieceSprites[10],
            PieceType.Knight => piece.isWhite ? pieceSprites[3] : pieceSprites[9],
            PieceType.Bishop => piece.isWhite ? pieceSprites[2] : pieceSprites[8],
            PieceType.King => piece.isWhite ? pieceSprites[0] : pieceSprites[6],
            PieceType.Queen => piece.isWhite ? pieceSprites[1] : pieceSprites[7],
            _ => null
        };

        return piece;
    }
}

public struct Board
{
    public Color lightColor, darkColor;
    public Square[] Squares;
    public List<Piece> whitePieces;
    public List<Piece> blackPieces;

    public Board(Color lColor, Color dColor)
    {
        lightColor = lColor;
        darkColor = dColor;
        Squares = new Square[64];
        whitePieces = new List<Piece>();
        blackPieces = new List<Piece>();
    }
}
