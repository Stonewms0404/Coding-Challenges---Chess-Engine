using System;
using UnityEngine;

public class Square : MonoBehaviour
{
    public static event Func<int, int, Piece> MovePiece;
    public static event Action<bool> EndTurn;

    public Color color;
    public int index, file, rank;
    public bool isLight, isLegal;
    public Piece piece;

    MeshRenderer rend;

    private void Start()
    {
        tag = "Square";
        rend = GetComponent<MeshRenderer>();
    }

    public void SetColor(Color color)
    {
        this.color = color;
        if (rend == null) rend = GetComponent<MeshRenderer>();
        rend.material.SetColor("_Color", color);
    }

    public void SelectSquare(bool selected)
    {
        isLegal = selected;
        if (isLegal) rend.material.SetColor("_Color", new(isLight ? 1 : .75f, isLight ? 1 : .75f, 0, 1));
        else rend.material.SetColor("_Color", color);
    }

    private void OnMouseDown()
    {
        if (!isLegal)
            return;

        if (piece != null)
            Destroy(piece.gameObject);
        piece = MovePiece(rank, file);
        EndTurn(piece.isWhite);
    }

    public static Square[] GetAllSquares()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Square");
        Square[] squares = new Square[objects.Length];

        for (int i = 0; i < objects.Length; i++)
            squares[i] = objects[i].GetComponent<Square>();

        return squares;
    }

    public static Square GetSquareAtPos(int rank, int file)
    {
        Collider[] hitObjects = Physics.OverlapSphere(new Vector3(file - 1, rank - 1), 0.25f);
        foreach (Collider collider in hitObjects)
        {
            if (collider.TryGetComponent<Square>(out Square square))
                return square;
        }
        return null;
    }
}
