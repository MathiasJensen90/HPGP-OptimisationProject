using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAllocatorSpending : MonoBehaviour
{
    public delegate void MemoryLeakEvent();
    public static event MemoryLeakEvent onAllocationEvent;

    private void Start()
    {
        InvokeRepeating("CreateAlocObject", .1f, .1f);  
    }

    private void CreateAlocObject()
    {
        GameObject obj = new GameObject("GenereatedAllocObject");
        var renderer = obj.AddComponent<MeshRenderer>();
        renderer.material.mainTexture = GenerateTexture();
        obj.AddComponent<MeshFilter>().mesh = GenerateMesh();
        onAllocationEvent += renderer.ToggleRenderer;
    }
    
    private Texture2D GenerateTexture()
    {
        // Simulate a large texture (2048x2048)
        Texture2D texture = new Texture2D(2048, 2048);
        texture.Apply();
        return texture;
    }

    private Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh();
        int gridSize = 100;
        Vector3[] vertices = new Vector3[gridSize * gridSize];
        
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                vertices[i * gridSize + j] = new Vector3(i, 0, j);
            }
        }
        
        int[] triangles = new int[(gridSize - 1) * (gridSize - 1) * 6];
    
        int t = 0; 
        for (int i = 0; i < gridSize - 1; i++)
        {
            for (int j = 0; j < gridSize - 1; j++)
            {
                int vertexIndex = i * gridSize + j;

                // Define the first triangle (bottom-left to top-right)
                triangles[t++] = vertexIndex;
                triangles[t++] = vertexIndex + gridSize;
                triangles[t++] = vertexIndex + 1;

                // Define the second triangle (top-right to bottom-right)
                triangles[t++] = vertexIndex + 1;
                triangles[t++] = vertexIndex + gridSize;
                triangles[t++] = vertexIndex + gridSize + 1;
            }
        }
    
        // Assign vertices and triangles to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        
        mesh.RecalculateNormals();
    
        return mesh;
    }

    private void OnDestroy()
    {
        CancelInvoke();
    }
}

public static class RendererExtensions
{
    public static void ToggleRenderer(this MeshRenderer renderer)
    {
        renderer.enabled = !renderer.enabled;
    }
}