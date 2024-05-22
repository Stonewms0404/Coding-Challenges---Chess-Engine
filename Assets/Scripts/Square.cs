using UnityEngine;

public class Square : MonoBehaviour
{
    public Color color;
    public int index;
    public bool isLight;

    public void SetColor(Color color)
    {
        this.color = color;
        GetComponent<MeshRenderer>().material.SetColor("_Color", color);
    }
}
