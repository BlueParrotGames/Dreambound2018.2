using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreambound.Geometry;

public class ShapeCreator : MonoBehaviour
{
    public MeshFilter filter;
    [HideInInspector]
    public List<Shape> shapes = new List<Shape>();
    [HideInInspector]
    public bool showShapes;

    public float handleRadius = .5f;
    public void UpdateMeshDisplay()
    {
        CompositeShape compShape = new CompositeShape(shapes);
        filter.mesh = compShape.GetMesh();
    }
}