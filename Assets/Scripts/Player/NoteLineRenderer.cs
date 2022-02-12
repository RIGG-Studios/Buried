using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteLineRenderer : Graphic
{
    public List<Transform> points;
    public List<Vector2> pointPositions;

    Mesh currentHelper;

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

        float angle = 0;

        for(int i = 0; i < points.Count; i++)
        {
            if(i < points.Count - 1)
            {
                angle = GetAngle(pointPositions[i], pointPositions[i + 1]) + 45f;
            }
            Vector2 point = pointPositions[i];
            DrawVerticesForPoint(point, vh, angle);
        }

        for(int v = 0; v < points.Count - 1; v++)
        {
            int index = v * 2;

            vh.AddTriangle(index + 0, index + 1, index + 3);
            vh.AddTriangle(index + 3, index + 2, index + 0);
        }

        if(currentHelper != null)
        {
            vh.FillMesh(currentHelper);
        }
        else
        {
            currentHelper = new Mesh();
            vh.FillMesh(currentHelper);
        }
    }

    private void Update()
    {
        for(int i = 0; i < points.Count; i++)
        {
            if (pointPositions.Count < points.Count)
            {
                pointPositions.Add(Vector2.zero);
            }

            pointPositions[i] = points[i].position - canvas.transform.position;
        }

        if(currentHelper != null)
        {
            VertexHelper vh = new VertexHelper();

            width = rectTransform.rect.width;
            height = rectTransform.rect.height;

            unitWidth = width / gridSize.x;
            unitHeight = height / gridSize.y;

            if (points.Count < 2)
            {
                return;
            }

            float angle = 0;

            for (int i = 0; i < points.Count; i++)
            {
                if (i < points.Count - 1)
                {
                    angle = GetAngle(pointPositions[i], pointPositions[i + 1]) + 45f;
                }
                Vector2 point = pointPositions[i];
                DrawVerticesForPoint(point, vh, angle);
            }

            for (int v = 0; v < points.Count - 1; v++)
            {
                int index = v * 2;

                vh.AddTriangle(index + 0, index + 1, index + 3);
                vh.AddTriangle(index + 3, index + 2, index + 0);
            }

            if (currentHelper != null)
            {
                vh.FillMesh(currentHelper);
            }
            else
            {
                currentHelper = new Mesh();
                vh.FillMesh(currentHelper);
            }

            canvasRenderer.SetMesh(currentHelper);
        }
    }

    public float GetAngle(Vector2 current, Vector2 target)
    {
        return Mathf.Atan2(target.y - current.y, target.x - current.x) * (180 / Mathf.PI);
    } 

    void DrawVerticesForPoint(Vector2 point, VertexHelper vh, float angle)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        vertex.position = Quaternion.Euler(0,0,angle) *  new Vector3(-thickness/2, 0);
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);
        vh.AddVert(vertex);

        vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(thickness / 2, 0);
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);
        vh.AddVert(vertex);
    }
}
