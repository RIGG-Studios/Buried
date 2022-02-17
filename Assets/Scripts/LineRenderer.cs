using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRenderer : MonoBehaviour
{
    public List<Transform> connectionPointsA;
    public List<Transform> connectionPointsB;

    public List<Material> materials;

    List<GameObject> emptysForMeshes;

    public float lineThickness;

    private void Start()
    {
        emptysForMeshes = new List<GameObject>();
    }

    public void DrawLineBetweenPoints(Vector3 point1, Vector3 point2, Material material, int index)
    {
        GameObject empty = null;

        if(emptysForMeshes.Count < index + 1)
        {
            empty = new GameObject();
            empty.transform.position = transform.position;

            empty.AddComponent<MeshFilter>();
            empty.AddComponent<MeshRenderer>();

            emptysForMeshes.Add(empty);
        }
        else if(emptysForMeshes.Count >= index + 1)
        {
            empty = emptysForMeshes[index].gameObject;
        }

        MeshFilter filter = empty.GetComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4];
        int[] triangles = new int[6];

        Vector3 point1PositionToRepo = point1 - transform.position;
        Vector3 point2PositionToRepo = point2 - transform.position;

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

        empty.GetComponent<MeshRenderer>().material = material;

        filter.mesh = mesh;
    }

    void Update()
    {
        for(int i = 0; i < connectionPointsA.Count; i++)
        {
            DrawLineBetweenPoints(connectionPointsA[i].position, connectionPointsB[i].position, materials[i], i);
        }
    }
}
