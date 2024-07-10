using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    public float offsetx1;
    public float offsetx2;
    public float offsety;
    public float offsetz1;
    public float offsetz2;
    List<Vector3> vertices = new List<Vector3>();
    List<int> tries = new List<int>();

    void Start()
    {
        mesh=new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;


    }

    // Update is called once per frame
    void Update()
    {
        UpdateMesh();
    }

    void CreateMesh()
    {
        foreach(Transform child in transform)
        {
            //forward

            if (child.gameObject.GetComponent<VisibilityManager>().visibility==true) {
                vertices.Add(child.localPosition + (child.up * offsety + child.forward * offsetx2 - child.right * offsetz1));
                vertices.Add(child.localPosition + (child.up * offsety - child.forward * offsetx2 - child.right * offsetz1));
                vertices.Add(child.localPosition + (-child.up * offsety + child.forward * offsetx2 - child.right * offsetz1));
                vertices.Add(child.localPosition + (-child.up * offsety - child.forward * offsetx2 - child.right * offsetz1));

                //back

                vertices.Add(child.localPosition + (child.up * offsety + child.forward * offsetx2 + child.right * offsetz1));
                vertices.Add(child.localPosition + (child.up * offsety - child.forward * offsetx2 + child.right * offsetz1));
                vertices.Add(child.localPosition + (-child.up * offsety + child.forward * offsetx2 + child.right * offsetz1));
                vertices.Add(child.localPosition + (-child.up * offsety - child.forward * offsetx2 + child.right * offsetz1));


                //up

                vertices.Add(child.localPosition + (child.up * offsety + child.forward * offsetx1 + child.right * offsetz2));
                vertices.Add(child.localPosition + (child.up * offsety + child.forward * offsetx1 - child.right * offsetz2));
                vertices.Add(child.localPosition + (-child.up * offsety + child.forward * offsetx1 + child.right * offsetz2));
                vertices.Add(child.localPosition + (-child.up * offsety + child.forward * offsetx1 - child.right * offsetz2));


                //down

                vertices.Add(child.localPosition + (child.up * offsety - child.forward * offsetx1 + child.right * offsetz2));
                vertices.Add(child.localPosition + (child.up * offsety - child.forward * offsetx1 - child.right * offsetz2));
                vertices.Add(child.localPosition + (-child.up * offsety - child.forward * offsetx1 + child.right * offsetz2));
                vertices.Add(child.localPosition + (-child.up * offsety - child.forward * offsetx1 - child.right * offsetz2));
            }
        }
        for(int i=0;i<vertices.Count;i+=16)
        {
            //end caps
                tries.Add(1);
                tries.Add(0);
                tries.Add(12);

                tries.Add(13);
                tries.Add(1);
                tries.Add(12);

                tries.Add(12);
                tries.Add(0);
                tries.Add(5);

                tries.Add(0);
                tries.Add(9);
                tries.Add(5);

                tries.Add(5);
                tries.Add(9);
                tries.Add(4);

                tries.Add(9);
                tries.Add(8);
                tries.Add(4);
            

            if (i==vertices.Count-16)
            {
                tries.Add(i+2);
                tries.Add(i + 3);
                tries.Add(i + 14);

                tries.Add(i + 3);
                tries.Add(i + 15);
                tries.Add(i + 14);

                tries.Add(i + 2);
                tries.Add(i + 14);
                tries.Add(i + 7);

                tries.Add(i + 11);
                tries.Add(i + 2);
                tries.Add(i + 7);

                tries.Add(i + 11);
                tries.Add(i + 7);
                tries.Add(i + 6);

                tries.Add(i + 10);
                tries.Add(i + 11);
                tries.Add(i + 6);
            }

            //forward
            tries.Add(i);
            tries.Add(i + 1);
            tries.Add(i + 2);            

            tries.Add(i + 2);
            tries.Add(i + 1);
            tries.Add(i + 3);

            //back
            tries.Add(i + 6);
            tries.Add(i + 5);
            tries.Add(i + 4);

            tries.Add(i + 5);
            tries.Add(i + 6);
            tries.Add(i + 7);

            //up
            tries.Add(i + 8);
            tries.Add(i + 9);
            tries.Add(i + 10);

            tries.Add(i + 10);
            tries.Add(i + 9);
            tries.Add(i + 11);

            //down
            tries.Add(i + 13);
            tries.Add(i + 12);
            tries.Add(i + 14);

            tries.Add(i + 13);
            tries.Add(i + 14);
            tries.Add(i + 15);

            //diagonalUpforward

            tries.Add(i + 0);
            tries.Add(i + 2);
            tries.Add(i + 11);

            tries.Add(i + 9);
            tries.Add(i + 0);
            tries.Add(i + 11);

            //diagonalUpbackward

            tries.Add(i + 8);
            tries.Add(i + 10);
            tries.Add(i + 4);

            tries.Add(i + 4);
            tries.Add(i + 10);
            tries.Add(i + 6);

            //diagonaldownforward

            tries.Add(i + 3);
            tries.Add(i + 1);
            tries.Add(i + 15);

            tries.Add(i + 1);
            tries.Add(i + 13);
            tries.Add(i + 15);
            //diagonaldownbackward


            tries.Add(i + 5);
            tries.Add(i + 7);
            tries.Add(i + 14);

            tries.Add(i + 12);
            tries.Add(i + 5);
            tries.Add(i + 14);

            if (i < vertices.Count - 16)
            {

      
                //forwardConnections

                tries.Add(i + 2);
                tries.Add(i + 3);
                tries.Add(i + 16);

                tries.Add(i + 17);
                tries.Add(i + 16);
                tries.Add(i + 3);

                //backConnections

                tries.Add(i + 7);
                tries.Add(i + 6);
                tries.Add(i + 20);

                tries.Add(i + 7);
                tries.Add(i + 20);
                tries.Add(i + 21);

                //upConnections

                tries.Add(i + 10);
                tries.Add(i + 11);
                tries.Add(i + 24);

                tries.Add(i + 25);
                tries.Add(i + 24);
                tries.Add(i + 11);


                //downConnections

                tries.Add(i + 15);
                tries.Add(i + 14);
                tries.Add(i + 28);

                tries.Add(i + 15);
                tries.Add(i + 28);
                tries.Add(i + 29);

                //diagonalUpforwardConnections
                tries.Add(i + 2);
                tries.Add(i + 16);
                tries.Add(i + 25);

                tries.Add(i + 2);
                tries.Add(i + 25);
                tries.Add(i + 11);

                //diagonalUpbackwardConnections
                tries.Add(i + 24);
                tries.Add(i + 20);
                tries.Add(i + 6);

                tries.Add(i + 24);
                tries.Add(i + 6);
                tries.Add(i + 10);

                //diagonaldownForwardConnections
                tries.Add(i + 3);
                tries.Add(i + 15);
                tries.Add(i + 17);

                tries.Add(i + 29);
                tries.Add(i + 17);
                tries.Add(i + 15);

                //diagonaldownBackwardConnections
                tries.Add(i + 21);
                tries.Add(i + 14);
                tries.Add(i + 7);

                tries.Add(i + 14);
                tries.Add(i + 21);
                tries.Add(i + 28);

            }









        }
    }
    private void UpdateMesh()
    {
        mesh.Clear();
        vertices.Clear();
        tries.Clear();
        CreateMesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles=tries.ToArray();
        mesh.RecalculateNormals();
    }
}
