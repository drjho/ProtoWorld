/*
 * 
 * GAPSLABS EXTENDED EDITOR
 * Aram Azhari
 * 
 * Reviewed by Miguel Ramos Carretero
 * Note: This is not used anymore. Look at OSMReaderSQL.cs instead.
 * 
 */

using UnityEngine;
using UnityEditor;

public class LineEditorFilledTriangles : Editor
{
    //[MenuItem("Gapslabs GIS Package/Line Filled Triangles")]
    static void Create()
	{
		GameObject gameObject = new GameObject("Line");

		Polygon p = gameObject.AddComponent<Polygon>();

		if (gameObject.GetComponent<MeshFilter>() == null)
		{
			MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
			meshFilter.mesh = new Mesh();
		}
		else
			gameObject.GetComponent<MeshFilter>().mesh = new Mesh();
		//////////////////////////////////////////////////////////////////////////
		// Example vertices
		Vector3[] nodes= new Vector3[]{
			new Vector3(0,0,0),
			new Vector3(1,0,0),
			new Vector3(2,0,1),
			new Vector3(2.5f,0,2),
			new Vector3(3,0,2),
			new Vector3(3,0,3),
		};
		
		
		Vector3[] NewVertices=new Vector3[nodes.Length-1];
		
		float distanceFromLine=0.5f;
		// if y1 == y2 ==> Slope becomes zero, and the normal slope is infinity. Therefore, 
		// The new vertex on the normal is (x1, y1 + distanceFromLine)
		
		for (int i=1; i<nodes.Length;i++)
		{
		
			if (nodes[i-1].z==nodes[i].z)
				
				NewVertices[i-1]=new Vector3(nodes[i-1].x,0,nodes[i-1].z + distanceFromLine);
			// if x1 == x2 ==> Slope becomes infinity, and the normal slope is zero. Therefore,
			// The new vertex on the normal is (x1 + distanceFromLine, y1)
			else
			if (nodes[i-1].x==nodes[i-1].x)
				NewVertices[i-1]=new Vector3(nodes[i-1].x+distanceFromLine,0,nodes[i-1].z);
			// Calculate slope, offset and the resulting point.
			else		
			{
				float slope=-1f/CalculateSlope(nodes[i-1],nodes[i]);
				float b=CalculateB(slope,nodes[i-1]);
				NewVertices[i-1]=new Vector3(nodes[i-1].x+distanceFromLine,0, CalculateY(slope,b,nodes[i-1].x+distanceFromLine));
			}
			Debug.Log("New vertex: " + NewVertices[i-1]);
		}
		
		// for every two nodes, we create an extra node. Therefore, 
		// The total of 3 vertices for every pair and we have N-1 number of pairs. ( N= original number of nodes)
		// the second part refers to filled triangles.
		Vector3[] verts = new Vector3[(nodes.Length-1)*3 + (nodes.Length-2)*3]; 
		for (int i=0;i<nodes.Length-1;i++)
		{
			verts[i*5]=nodes[i];
			verts[i*5+1]=nodes[i+1];
			verts[i*5+2]=NewVertices[i]; 
			
			if (i!=nodes.Length-2)
			{
				Debug.LogWarning("nv"+i + " with node"+(i+1)+ " with nv"+(i+1));
				verts[i*5+3]=NewVertices[i];
				verts[i*5+4]=nodes[i+1];
				verts[i*5+5]=NewVertices[i+1]; 
			}
		}
		
		//for (int i=0; i<verts.Length; i++)
		//	Debug.Log(
		int[] Tris = new int[verts.Length];
		for (int i=0; i<verts.Length; i++)
			Tris[i]=i;
		
		
		Vector2 uv0 = new Vector2(0, 0);
		Vector2 uv1 = new Vector2(1, 0);
		Vector2 uv2 = new Vector2(0.5f, 1);
		Vector2[] uvTemp = new Vector2[] {uv0,uv1,uv2};
		Vector2[] UVs = new Vector2[verts.Length];
		for (int i=0; i<verts.Length; i++)
			UVs[i]= uvTemp[i%3];
		
		p.Rebuild(verts, Tris, UVs);
		
		p.GetComponent<Renderer>().sharedMaterial = new Material(Shader.Find("Particles/Additive"));
		//p.renderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
	}
    // Note that we don't invole y component into calculations as our roads are in 2D.
    public static float CalculateSlope(Vector3 p1, Vector3 p2)
    {
        return (p2.z - p1.z) / (p2.x - p1.x);
    }
    // Note that we don't invole y component into calculations as our roads are in 2D.
    public static float CalculateB(float Slope, Vector3 p)
    {
        // y= a*x + b
        // b= y-a*x
        // a= Slope
        return p.z - p.x * Slope;
    }
    public static float CalculateY(float Slope,float b,float X)
	{
			return Slope*X+b;
		}
}