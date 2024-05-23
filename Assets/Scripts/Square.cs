using System;
using UnityEngine;

public class Square : MonoBehaviour
{
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
}
