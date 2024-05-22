using UnityEngine;

public class StartBoard : MonoBehaviour
{
    [SerializeField] Color lightColor, darkColor;

    Board ChessBoard;

    private void Start()
    {
        NewBoard();
    }

    private void Update()
    {
        foreach (Square square in ChessBoard.Squares)
            if (square.color != lightColor || square.color != darkColor)
                square.SetColor(square.isLight ? lightColor : darkColor);
    }

    void NewBoard()
    {
        ChessBoard = new Board(darkColor, lightColor);
        SpawnPieces();
    }

    void SpawnPieces()
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
}

public struct Board
{
    public Color lightColor, darkColor;
    public Square[] Squares;

    public Board(Color lColor, Color dColor)
    {
        lightColor = lColor;
        darkColor = dColor;
        Squares = new Square[64];
    }
}
public struct Piece
{

}

public enum PieceType
{

}