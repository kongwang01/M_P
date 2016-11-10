using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class DrawRobot : MonoBehaviour {
	List<Robot> robots = new List<Robot>(); //用來儲存障礙物

	// Use this for initialization
	void Start () {

		int n_of_robots = 0;
		int n_of_polygons = 0;
		//======  存讀檔   =======================================================
		string path = Application.dataPath + "/Resources/robot.dat";
		if (!File.Exists(path)){
			Debug.Log("Error Path: " + path);
			return;
		}
		StreamReader sr = File.OpenText(path);
		string input = ""; 

		List<string> input_string = new List<string>(); //把讀進來的檔案之數字部分存起來
		while (true) 
		{
			input = sr.ReadLine(); 

			if (input == null)
			{
				break;
			}
			if((input.Length > 0) && (input[0] != '#') && (input[0] != 'n'))
				input_string.Add(input);
		}
		sr.Close();
		//=======================================================================

		//========    把資料存進結構裡    ===================================
		n_of_robots = Convert.ToInt32( input_string[0] );
		int line = 1;

		Obstacle temp_o;
		Polygon temp_p;

		float temp_x = 0.0F;
		float temp_y = 0.0F;
		float temp_z = 0.0F;

		for(int i=0; i<n_of_robots; i++)
		{
			temp_o = new Obstacle();

			//input = sr.ReadLine(); //讀入number of polygons
			n_of_polygons = Convert.ToInt32( input_string[line++] );
			temp_o.n_of_polygons = n_of_polygons;
			for(int j=0; j<n_of_polygons; j++)
			{
				//input = sr.ReadLine(); //讀入number of vertices
				temp_p = new Polygon();
				temp_p.n_of_vertices = Convert.ToInt32( input_string[line++] );
				for(int k=0; k<temp_p.n_of_vertices; k++)
				{
					string[] sArray = input_string[line].Split(' ');
					temp_x = Convert.ToSingle( sArray[0] );
					temp_y = Convert.ToSingle( sArray[1] );
					Vector2 v2= new Vector2(temp_x, temp_y);

					temp_p.vertices.Add(v2);
					line++;
				}
				temp_o.polygons.Add(temp_p);
			}
			string[] temp_Array = input_string[line].Split(' ');
			temp_x = Convert.ToSingle( temp_Array[0] );
			temp_y = Convert.ToSingle( temp_Array[1] );
			temp_z = Convert.ToSingle( temp_Array[2] );
			Vector3 v3= new Vector3(temp_x, temp_y, temp_z);
			temp_o.init_configuration = v3;

			obstacles.Add(temp_o);		
			line++;
		}	
		//=======================================================================

		//==========  繪圖  =====================================================
		Vector2[] vertices2D;

		for (int i = 0; i < n_of_obstacles; i++) 
		{			
			for (int j = 0; j < obstacles[i].n_of_polygons; j++) 
			{
				vertices2D = obstacles[i].polygons[j].vertices.ToArray();
				DrawPolygon (vertices2D, i);
			}
		}

		//==========================================================================================
	}

	// Update is called once per frame
	void Update () {

	}

	public void DrawPolygon(Vector2[] vertices2D, int obstacle_n){
		// Use the triangulator to get indices for creating triangles
		Triangulator tr = new Triangulator(vertices2D);
		int[] indices = tr.Triangulate();

		// Create the Vector3 vertices
		Vector3[] vertices = new Vector3[vertices2D.Length];
		for (int i=0; i<vertices.Length; i++) {
			vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
		}

		// Create the mesh
		Mesh msh = new Mesh();
		msh.vertices = vertices;
		msh.triangles = indices;
		msh.RecalculateNormals();
		msh.RecalculateBounds();


		GameObject obj = new GameObject ("Obstacle");
		obj.AddComponent (typeof(MeshRenderer));
		MeshFilter filter = obj.AddComponent(typeof(MeshFilter)) as MeshFilter;
		filter.mesh = msh;

		obj.transform.Translate (new Vector3(obstacles [obstacle_n].init_configuration.x, obstacles [obstacle_n].init_configuration.x, 0));
		obj.transform.Rotate(new Vector3(0, 0, obstacles [obstacle_n].init_configuration.z));

		//把polygon畫成藍色
		//obj.GetComponent<Renderer> ().material.color = new Color (0.4f, 0.4f, 1.0f, 0.0f);
	}
}
