using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class procedural : MonoBehaviour
{

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    public int xSize = 20;
    public int zSize = 20;
    public LayerMask dragmask;
    //GameObject selectedobject;
    public int triangleIndex;
    bool isdragging;
    public int closest = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        isdragging = false;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
                //float y = 1.0f;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        

    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = mesh;

    }

    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.Log("hey");
           
            if(Physics.Raycast(ray, out hit))
            {
                Debug.Log("wsh");
                Debug.Log(hit.collider.gameObject.name);
                //selectedobject = hit.collider.gameObject;
                int actualTriangle = hit.triangleIndex;
                int indicevec1 = triangles[actualTriangle * 3];
                int indicevec2 = triangles[actualTriangle * 3+1];
                int indicevec3 = triangles[actualTriangle * 3+2];

                float dist1=Vector3.Distance(hit.point,vertices[indicevec1]);
                float dist2=Vector3.Distance(hit.point,vertices[indicevec2]);
                float dist3=Vector3.Distance(hit.point,vertices[indicevec3]);
                
                if(dist1<=dist2&&dist1<=dist3){closest=indicevec1;}
                else if(dist2<=dist1&&dist2<=dist3){closest=indicevec2;}
                else if(dist3<=dist1&&dist3<=dist2){closest=indicevec3;}
                //Debug.Log(closest);

                isdragging = true;
            }
        }
        if (isdragging==true)
        {
            Vector3 pos = mousepos();
            vertices[closest] = pos;
            UpdateMesh();
            Debug.Log(closest);
            
        }
        if (Input.GetMouseButtonUp(0))
        {
            isdragging = false;
        }
    }
    Vector3 mousepos()
    {
        return new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
    }
    

}
