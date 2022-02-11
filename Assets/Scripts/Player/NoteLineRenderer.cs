using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteLineRenderer : Graphic
{
    public Transform position1;
    public Transform position2;
    public float thickness;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        Vector2 position1ToPosition2 = position2.position - position1.position;

        Vector2 position1ToPosition2PerpendicularUp = -new Vector2(position1ToPosition2.y, position1ToPosition2.x);

        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        vertex.position = new Vector3(0, 0);
        vh.AddVert(vertex);

        vertex.position = new Vector3(0, rectTransform.rect.height);
        vh.AddVert(vertex);

        vertex.position = new Vector3(rectTransform.rect.width, rectTransform.rect.height);
        vh.AddVert(vertex);

        vertex.position = new Vector3(rectTransform.rect.width, 0);
        vh.AddVert(vertex);

        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(2, 3, 0);
    }
}
