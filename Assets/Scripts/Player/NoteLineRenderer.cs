using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteLineRenderer : Graphic
{
    public List<Vector2> points;
    public Vector2Int gridSize;
    public float thickness;

    float width;
    float height;

    float unitWidth;
    float unitHeight;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        width = rectTransform.rect.width;
        height = rectTransform.rect.height;

        unitWidth = width / gridSize.x;
        unitHeight = height / gridSize.y;

        if(points.Count < 2)
        {
            return;
        }

        for(int i = 0; i < points.Count; i++)
        {
            Vector2 point = points[i];
            DrawVerticesForPoint(point, vh);
        }

        for(int v = 0; v < points.Count - 1; v++)
        {
            int index = v * 2;

            vh.AddTriangle(index + 0, index + 1, index + 3);
            vh.AddTriangle(index + 3, index + 2, index + 0);
        }
    }

    void DrawVerticesForPoint(Vector2 point, VertexHelper vh)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        vertex.position = new Vector3(-thickness/2, 0);
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);
        vh.AddVert(vertex);

        vertex.position = new Vector3(thickness / 2, 0);
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);
        vh.AddVert(vertex);
    }
}
