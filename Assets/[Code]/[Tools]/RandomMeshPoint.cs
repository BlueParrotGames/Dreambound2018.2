using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMeshPoint : MonoBehaviour
{
    public MeshFilter filter;
    private Vector3 randomPoint;
    public List<Vector3> points;

    public float result;
    public float density;
    public float radius = 0.5f;

    public void CalculatePoints()
    {
        points.Clear();

        float r = Mathf.RoundToInt(Area());
            Debug.Log("Unrounded Area size: " + Area());
            Debug.Log("Rounded Area size: " + r);

        for (int i = 0; i < r; i++)
        {
            Vector3 point = GetRandomPointOnMesh(filter.sharedMesh);
            point += filter.transform.position;
            
            points.Add(point);
        }

        if (points.Count == r)
        {
            foreach(Vector3 p in points)
            {
                StartCoroutine(CheckPoint(p));
            }
        }
    }

    public IEnumerator CheckPoint(Vector3 p)
    {
        for (int i = 0; i < points.Count; i++)
        {
            float dist = Vector3.Distance(p, points[i]);
            if (dist < radius)
            {
                Debug.Log((dist < radius).ToString());

                Debug.Log("Problematic point: " + " A: " + p + " B: " + points[i] + " Distance: " + Vector3.Distance(p, points[i]) + " ...Removing the point.");
            }
        }
        yield return null;
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