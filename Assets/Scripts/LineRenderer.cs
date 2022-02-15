using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRenderer : MonoBehaviour
{
    public List<Transform> connectionPointsA;
    public List<Transform> connectionPointsB;

    public List<Material> materials;

    public float lineThickness;

    MeshFilter filter;

    void Start()
    {
        filter = GetComponent<MeshFilter>();
    }

    public void DrawLineBetweenPoints(Transform point1, Transform point2, Material material)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4];
        int[] triangles = new int[6];

        Vector3 point1PositionToRepo = point1.position - transform.position;
        Vector3 point2PositionToRepo = point2.position - transform.position;

        Vector3 point1ToPoint2 = point2PositionToRepo - point1PositionToRepo;
        Vector3 point2ToPoint1 = point1PositionToRepo - point2PositionToRepo;

        Vector3 crossProductForPoint1 = Vector3.Cross(point2ToPoint1, Vector3.forward);
        Vector3 crossProductForPoint2 = Vector3.Cross(point1ToPoint2, Vector3.forward);

        vertices[0] = crossProductForPoint1.normalized * lineThickness + point1PositionToRepo + new Vector3(0, 0, 5);
        vertices[1] = -crossProductForPoint2.normalized * lineThickness + point2PositionToRepo + new Vector3(0, 0, 5);

        vertices[2] = -crossProductForPoint1.normalized * lineThickness + point1PositionToRepo + new Vector3(0, 0, 5);
        vertices[3] = crossProductForPoint2.normalized * lineThickness + point2PositionToRepo + new Vector3(0, 0, 5);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        triangles[3] = 2;
        triangles[4] = 1;
        triangles[5] = 3;


        mesh.RecalculateNormals();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        GetComponent<MeshRenderer>().material = material;

        filter.mesh = mesh;
    }

    void Update()
    {
        for(int i = 0; i < connectionPointsA.Count; i++)
        {
            DrawLineBetweenPoints(connectionPointsA[i], connectionPointsB[i], materials[i]);
        }
    }
}
