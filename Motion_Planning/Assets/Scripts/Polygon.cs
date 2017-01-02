using UnityEngine;
using System.Collections.Generic;

public class Polygon
{
	public int n_of_vertices;
	public List<Vector2> vertices = new List<Vector2>();
	public List<Vector2> config_vertices = new List<Vector2>();

	public Polygon () {

	}
	//public Polygon (Vector2[] points) {
	//    vertices = new List<Vector2>(points);
	//}

    public static GameObject DrawPolygon(Vector2[] vertices2D)
    {
        // Use the triangulator to get indices for creating triangles
        Triangulator tr = new Triangulator(vertices2D);
        int[] indices = tr.Triangulate();

        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[vertices2D.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
        }

        // Create the mesh
        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = indices;
        msh.RecalculateNormals();
        msh.RecalculateBounds();

        GameObject obj = new GameObject("Polygon");
        obj.AddComponent(typeof(MeshRenderer));
        MeshFilter filter = obj.AddComponent(typeof(MeshFilter)) as MeshFilter;
        filter.mesh = msh;

        //PolygonCollider2D collider = obj.AddComponent(typeof(PolygonCollider2D)) as PolygonCollider2D;
        //collider.points = vertices2D;

        //GameObject objState = GameObject.Find("TransAndRotateForPolygon");
        //obj.AddComponent(Type.GetType("TransAndRotateForPolygon"));


        //obj.transform.Translate (new Vector3(obstacles [obstacle_n].init_configuration.x, obstacles [obstacle_n].init_configuration.y, 0));
        //obj.transform.Rotate(new Vector3(0, 0, obstacles [obstacle_n].init_configuration.z));

        //把polygon畫成藍色
        //obj.GetComponent<Renderer> ().material.color = new Color (0.4f, 0.4f, 1.0f, 0.0f);

        return obj;
    }
}