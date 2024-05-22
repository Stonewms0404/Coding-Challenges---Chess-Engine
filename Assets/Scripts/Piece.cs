using UnityEngine;

public class Piece : MonoBehaviour
{
    public Sprite sprite;
    public PieceType type;
    public int file, rank;
    public bool isWhite;

    private void Start()
    {
        transform.position = new(file - 1, rank - 1, -1);
        SpriteRenderer rend = gameObject.AddComponent<SpriteRenderer>();
        rend.sprite = sprite;
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