using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Createpyramid : MonoBehaviour
{
    public float c;
    public float h;
    private Vector3[] p_vertices;
    private Vector3[] p_normals;
    private int[] p_triangles;
    public GameObject PickObj;
    private Vector3 mOffset;
    private Mesh p_mesh;
    
    public GameObject sphere;
    public Dictionary<int, List<int>> les_po = new Dictionary<int, List<int>>();

    // Use this for initialization
    void Start () {
        float w = -c / 2.0f;
        float W = c / 2.0f;
        Vector3 p0, p1, p2, p3, p4, p5, p6, p7;

        p0 = new Vector3(w, w, w);
        p1 = new Vector3(w, W, w);
        p2 = new Vector3(W, W, w);
        p3 = new Vector3(W, w, w);
        p4 = new Vector3(w, w, W);
        p5 = new Vector3(w, W, W);
        p6 = new Vector3(W, W, W);
        p7 = new Vector3(W, w, W);



        float cc = c / 2.0f;
        p_mesh = new Mesh();

        p_mesh.Clear();
        p_mesh.name = "MyMeshPyramide";
        p_vertices = new Vector3[8];
        // points de la base
        p_vertices[0] = new Vector3(-cc, 0, -cc);
        p_vertices[1] = new Vector3(-cc, 0, +cc);
        p_vertices[2] = new Vector3(+cc, 0, +cc);
        p_vertices[3] = new Vector3(+cc, 0, -cc);
        // autres points
        p_vertices[4] = new Vector3(-cc, h, -cc);
        p_vertices[5] = new Vector3(-cc, h, +cc);
        p_vertices[6] = new Vector3(+cc, h, +cc);
        p_vertices[7] = new Vector3(+cc, h, -cc);

        p_triangles = new int[] { 0, 2, 1, 2, 0, 3, 3, 0, 4, 4, 7, 3, 0, 1, 5, 5, 4, 0, 1, 2, 6, 6, 5, 1, 2, 3, 7, 7, 6, 2, 4, 5, 6, 6, 7, 4 };




        p_vertices = new Vector3[]{
            p0,p1,p2,p3,  // devant
            p4,p5,p1,p0,  // gauche
            p3,p2,p6,p7,  // Droite
            p7,p6,p5,p4,  // Derrière
            p1,p5,p6,p2,  // Dessus
            p4,p0,p3,p7   // dessous
            };
        p_normals = new Vector3[p_vertices.Length];
        // les indices des 3 vertices des 12 triangles (2 pour chaque face du cube)
        p_triangles = new int[12 * 3];
        int index = 0;
        for (int i = 0; i < 6; i++)   // 6 faces à 2 triangles
        {   // triangle 1
            p_triangles[index++] = i * 4;
            p_triangles[index++] = i * 4 + 1;
            p_triangles[index++] = i * 4 + 2;
            // triangle 2
            p_triangles[index++] = i * 4 + 2;
            p_triangles[index++] = i * 4 + 3;
            p_triangles[index++] = i * 4 + 0;


        }

        const float SEUIL_DISTANCE_VERTICES_SIMILAIRES = 0.0000000000001f;
        bool[] bool_vert = new bool[p_vertices.Length];
       
        float dist = 0;
        int index_vert = 0;
        int nb_pickingObjects = 0;
        int nb_po = 0;
        while (index_vert < p_vertices.Length) { 
            if (!bool_vert[index_vert])
            {
                bool_vert[index_vert] = true;
                // ajouter le PickObject à la position du vertex
                
                GameObject po = Instantiate(PickObj);
                po.GetComponent<drag>().cube = gameObject;
                po.transform.position = transform.TransformPoint(p_vertices[index_vert]);
                po.name = "po" + nb_po;
                nb_po++;
                //po.transform.position = transform.position + p_vertices[index_vert];
                // idem ci dessous
                
                // TransformPoint converts the vertex's local position into world space.
                les_po.Add(po.GetInstanceID(), new List<int> { index_vert });
                for(int i=index_vert;i< p_vertices.Length; i++)
                {
                    dist = Vector3.Distance(p_vertices[index_vert], p_vertices[i]);
                    if ( dist < SEUIL_DISTANCE_VERTICES_SIMILAIRES)
                    {
                        les_po[po.GetInstanceID()].Add(i);
                    }
                }
                

            }
        index_vert++;
        }


        p_mesh.vertices = p_vertices;
        p_mesh.triangles = p_triangles;
        GetComponent<MeshFilter>().mesh = p_mesh;
        p_mesh.RecalculateNormals();
    }

    public void bouger(GameObject po)
    {
        foreach (int i in les_po[po.GetInstanceID()])
        {
            p_vertices[i] = po.transform.position;
            print("po=" + po.name + "i=" + i);
        }
        p_mesh.vertices = p_vertices;
        p_mesh.triangles = p_triangles;
        GetComponent<MeshFilter>().mesh = p_mesh;
        p_mesh.RecalculateNormals();

    }
        
}
