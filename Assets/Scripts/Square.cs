using UnityEngine;

public class Square : MonoBehaviour
{
    public Color color;
    public int index, file, rank;
    public bool isLight;
    public bool hasPiece;
    public Piece piece;

    public void SetColor(Color color)
    {
        this.color = color;
        GetComponent<MeshRenderer>().material.SetColor("_Color", color);
    }
}
