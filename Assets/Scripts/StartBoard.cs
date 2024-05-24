using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class StartBoard : MonoBehaviour
{
    public static event Action<Board> OnStartBoard;
    public string fenString;

    [SerializeField] Color lightColor, darkColor, pieceWhiteColor = Color.white, pieceBlackColor = Color.black;
    [SerializeField] Sprite[] pieceSprites;
    [SerializeField] Vector3 pieceScale;

    Board ChessBoard;
    GameObject PiecesParent, SquaresParent;

    private void Start()
    {
        PiecesParent = new("Pieces Parent");
        SquaresParent = new("Squares Parent");
        NewBoard();
        OnStartBoard(ChessBoard);
    }

    private void Update()
    {
        foreach (Square square in ChessBoard.Squares)
        {
            if (square.color != lightColor && square.color != darkColor)
                square.SetColor(square.isLight ? lightColor : darkColor);
            if (square.piece)
                square.piece.transform.localScale = pieceScale;
        }

        foreach (Piece piece in ChessBoard.whitePieces)
            if (piece.color != pieceWhiteColor)
                piece.SetColor(pieceWhiteColor);
        foreach (Piece piece in ChessBoard.blackPieces)
            if (piece.color != pieceBlackColor)
                piece.SetColor(pieceBlackColor);
    }

    void NewBoard()
    {
        ChessBoard = new Board(darkColor, lightColor);
        SpawnSquares();
        SpawnPieces();
    }

    void SpawnSquares()
    {
        ChessBoard.Squares ??= new Square[64];
        for (int i = 0; i < 64; i++)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.name = "Square " + (i + 1);
            obj.transform.SetParent(SquaresParent.transform);

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
                {
                    Destroy(obj);
                    square.piece = null;
                    continue;
                }

                piece.rank = square.rank;
                piece.file = square.file;
                piece.isWhite = square.rank == 1 || square.rank == 2;
                piece.isTurn = piece.isWhite;

                piece.name = (piece.isWhite ? "White" : "Black") + " " + piece.type.ToString();

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
            PieceType.Pawn => pieceSprites[5],
            PieceType.Rook => pieceSprites[4],
            PieceType.Knight => pieceSprites[3],
            PieceType.Bishop => pieceSprites[2],
            PieceType.King => pieceSprites[0],
            PieceType.Queen => pieceSprites[1],
            _ => null
        };

        return piece;
    }

    public static Piece[] ConvertTo(Piece _)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Piece");
        Piece[] arr = new Piece[objects.Length];
        int i = 0;
        foreach (GameObject obj in objects)
        {
            if (obj.TryGetComponent<Piece>(out Piece Tcomp))
                arr[i] = Tcomp;
        }
        return arr;
    }
    public static Square[] ConvertTo(Square _)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Square");
        Square[] arr = new Square[objects.Length];
        int i = 0;
        foreach (GameObject obj in objects)
        {
            if (obj.TryGetComponent<Square>(out Square Tcomp))
                arr[i] = Tcomp;
        }
        return arr;
    }
}

public struct Board
{
    public Color lightColor, darkColor;
    public Square[] Squares;
    public List<Piece> whitePieces;
    public List<Piece> blackPieces;
    public bool isWhitesTurn;

    public Board(Color lColor, Color dColor)
    {
        lightColor = lColor;
        darkColor = dColor;
        Squares = new Square[64];
        whitePieces = new List<Piece>();
        blackPieces = new List<Piece>();
        isWhitesTurn = true;
    }
}
