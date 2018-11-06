using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMeshPoint : MonoBehaviour
{
    public MeshFilter filter;
    private Vector3 randomPoint;
    public List<Vector3> points;
    public List<Vector3> ripPoints;

    public float result;
    public float density;

    int pointsToAdd;
    int attempts = 0;
    float radius = 0.5f;

    public void CalculatePoints()
    {
        radius = 0.5f;
        attempts = 0;
        points.Clear();

        Debug.Log(radius);
        float r = Mathf.RoundToInt(Area());
            //Debug.Log("Unrounded Area size: " + Area());
            Debug.Log("Rounded Area size: " + r);

        for (int i = 0; i < r; i++)
        {
            Vector3 point = GetRandomPointOnMesh(filter.sharedMesh);
            point += filter.transform.position;
            
            points.Add(point);
        }

        CheckRadius();
        pointsToAdd = (int)(r - points.Count);
        //Debug.Log("dit moet " + (points.Count * points.Count) + " zijn");
       
        while(pointsToAdd > 0 && attempts < 2500)
        {
            pointsToAdd--;
            Vector3 point = GetRandomPointOnMesh(filter.sharedMesh);
            point += filter.transform.position;

            points.Add(point);

            CheckRadius();
            attempts++;
            Debug.Log("Attempted while " + attempts + " times" );
        }

        if(attempts == 2500)
        {
            Debug.LogError("Radius is too high to process, run ended after 2500 attempts.");
            points.Clear();
        }
    }

    private void CheckRadius()
    {
        //Gemaakt door Laurens 
        for (int x = 0; x < points.Count; x++)
        {
            for (int j = 0; j < points.Count; j++)
            {
                float dist = (points[x] - points[j]).magnitude;

                if (dist < radius && x != j)
                {
                    //Debug.Log("distance under 0.5f " + dist + " removing point");
                    points.RemoveAt(j);
                    pointsToAdd++;
                }
            }
        }
    }

    public void OnDrawGizmos()
    {
        foreach (Vector3 pos in points)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(pos, 0.2f);
        }
    }

    Vector3 GetRandomPointOnMesh(Mesh mesh)
    {
        float[] sizes = GetTriSizes(mesh.triangles, mesh.vertices);
        float[] cumulativeSizes = new float[sizes.Length];
        float total = 0;

        for (int i = 0; i < sizes.Length; i++)
        {
            total += sizes[i];
            cumulativeSizes[i] = total;
        }

        float randomsample = Random.value * total;
        int triIndex = -1;

        for (int i = 0; i < sizes.Length; i++)
        {
            if (randomsample <= cumulativeSizes[i])
            {
                triIndex = i;
                break;
            }
        }
        //als je 10 bomen in spawnt wil je een grotere radius dan .5
        if (triIndex == -1) Debug.LogError("triIndex should never be -1");

        Vector3 a = mesh.vertices[mesh.triangles[triIndex * 3]];
        Vector3 b = mesh.vertices[mesh.triangles[triIndex * 3 + 1]];
        Vector3 c = mesh.vertices[mesh.triangles[triIndex * 3 + 2]];

        float r = Random.value;
        float s = Random.value;

        if (r + s >= 1)
        {
            r = 1 - r;
            s = 1 - s;
        }
        Vector3 pointOnMesh = a + r * (b - a) + s * (c - a);
        return pointOnMesh;
    }

    float Area()
    {
        Vector3[] vert = filter.sharedMesh.vertices;
        int[] triangles = filter.sharedMesh.triangles;

        result = 0f;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            result += (Vector3.Cross(vert[triangles[i + 1]] - vert[triangles[i]],
                vert[triangles[i + 2]] - vert[triangles[i]])).magnitude;
        }
        return result *= density;
    }

    float[] GetTriSizes(int[] tris, Vector3[] verts)
    {
        int triCount = tris.Length / 3;
        float[] sizes = new float[triCount];
        for (int i = 0; i < triCount; i++)
        {
            sizes[i] = .5f * Vector3.Cross(verts[tris[i * 3 + 1]] - verts[tris[i * 3]], verts[tris[i * 3 + 2]] - verts[tris[i * 3]]).magnitude;
        }
        return sizes;
    }
}
//Als David vader wordt ga ik zijn kinderen Davidson noemen <3