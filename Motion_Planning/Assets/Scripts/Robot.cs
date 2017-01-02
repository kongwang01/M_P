using UnityEngine;
using System.Collections.Generic;

public class Robot
{
    //===  檔案讀來的資料  ===
	public int n_of_polygons;
	public List<Polygon> polygons = new List<Polygon>();
	public Vector3 init_configuration = new Vector3();	
	public Vector3 goal_configuration = new Vector3();	
	public int n_of_control_points;
	public List<Vector2> control_points = new List<Vector2>();

    //===  隨著運算會更動的資料  ===
    public Vector3 curr_configuration = new Vector3();
    public Vector3 goal_curr_configuration = new Vector3();
    public List<Vector2> control_points_pos = new List<Vector2>();

	public Robot () {

	}
}