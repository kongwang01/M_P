﻿using UnityEngine;
using System.Collections.Generic;

public class Robot
{
	public int n_of_polygons;
	public List<Polygon> polygons = new List<Polygon>();
	public Vector3 init_configuration = new Vector3();
	public Vector3 goal_configuration = new Vector3();
	public List<Vector2> control_points = new List<Vector2>();

	public Robot () {

	}
}