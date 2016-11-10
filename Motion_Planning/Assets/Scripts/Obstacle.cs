/*
using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
*/

using UnityEngine;
using System.Collections.Generic;
 
public class Obstacle
{
	public int n_of_polygons;
	//private List<int> n_of_vertices = new List<int>();
    //private List<Vector2> vertices = new List<Vector2>();
	public List<Polygon> polygons = new List<Polygon>();
 	public Vector3 init_configuration = new Vector3();
	
	public Obstacle () {
        
    }
	/*
    public Obstacle (Polygon[] ps) {
        //m_points = new List<Vector2>(points);
		polygons = new List<Polygon>(ps);
		n_of_polygons = polygons.Count;
    }
 
    public int[] Triangulate() {
        List<int> indices = new List<int>();
 
        int n = m_points.Count;
        if (n < 3)
            return indices.ToArray();
 
        int[] V = new int[n];
        
    }
 
    private float Area () {
        int n = m_points.Count;
        float A = 0.0f;
        for (int p = n - 1, q = 0; q < n; p = q++) {
            Vector2 pval = m_points[p];
            Vector2 qval = m_points[q];
            A += pval.x * qval.y - qval.x * pval.y;
        }
        return (A * 0.5f);
    }
	*/
 
 
}

public class Polygon
{
	public int n_of_vertices;
    public List<Vector2> vertices = new List<Vector2>();
	
	public Polygon () {
        
    }
	
	//public Polygon (Vector2[] points) {
    //    vertices = new List<Vector2>(points);
    //}
}