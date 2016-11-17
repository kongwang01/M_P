using UnityEngine;
using System.Collections.Generic;
 
public class Obstacle
{
	public int n_of_polygons;
	//private List<int> n_of_vertices = new List<int>();
    //private List<Vector2> vertices = new List<Vector2>();
	public List<Polygon> polygons = new List<Polygon>();
 	public Vector3 init_configuration = new Vector3();
	public Vector3 curr_configuration = new Vector3();

	public Obstacle () {
        
    }

	/*
    public Obstacle (Polygon[] ps) {
        //m_points = new List<Vector2>(points);
		polygons = new List<Polygon>(ps);
		n_of_polygons = polygons.Count;
    }
	*/
}