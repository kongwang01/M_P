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

		Robot temp_r;
		Polygon temp_p;

		float temp_x = 0.0F;
		float temp_y = 0.0F;
		float temp_z = 0.0F;

		for(int i=0; i<n_of_robots; i++)
		{
			temp_r = new Robot();

			//input = sr.ReadLine(); //讀入number of polygons
			n_of_polygons = Convert.ToInt32( input_string[line++] );
			temp_r.n_of_polygons = n_of_polygons;
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
				temp_r.polygons.Add(temp_p);
			}
			string[] temp_Array = input_string[line++].Split(' '); //拆分initial configuration
			temp_x = Convert.ToSingle( temp_Array[0] );
			temp_y = Convert.ToSingle( temp_Array[1] );
			temp_z = Convert.ToSingle( temp_Array[2] );
			Vector3 v3= new Vector3(temp_x, temp_y, temp_z);
			temp_r.init_configuration = v3;

			temp_Array = input_string[line++].Split(' ');  //拆分goal configuration
			temp_x = Convert.ToSingle( temp_Array[0] );
			temp_y = Convert.ToSingle( temp_Array[1] );
			temp_z = Convert.ToSingle( temp_Array[2] );
			v3= new Vector3(temp_x, temp_y, temp_z);
			temp_r.goal_configuration = v3;

			temp_r.n_of_control_points = Convert.ToInt32( input_string[line++] ); //儲存control point
			for(int k=0; k<temp_r.n_of_control_points; k++)
			{
				string[] sArray = input_string[line].Split(' ');
				temp_x = Convert.ToSingle( sArray[0] );
				temp_y = Convert.ToSingle( sArray[1] );
				Vector2 v2= new Vector2(temp_x, temp_y);

				temp_r.control_points.Add(v2);
				line++;
			}

			robots.Add(temp_r);		
			//line++;
		}	
		//=======================================================================

		//==========  繪圖  =====================================================
		Vector2[] vertices2D;

		for (int i = 0; i < n_of_robots; i++) 
		{			
			for (int j = 0; j < robots[i].n_of_polygons; j++) 
			{
				vertices2D = robots[i].polygons[j].vertices.ToArray();
				DrawPolygon (vertices2D, i);
			}
		}

		//==========================================================================================
	}

	// Update is called once per frame
	void Update () {

	}

	public void DrawPolygon(Vector2[] vertices2D, int robot_n){
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


		//====  畫initial robot ============
		GameObject obj = new GameObject ("Robot");
		obj.AddComponent (typeof(MeshRenderer));
		MeshFilter filter = obj.AddComponent(typeof(MeshFilter)) as MeshFilter;
		filter.mesh = msh;

		obj.transform.Translate (new Vector3(robots [robot_n].init_configuration.x, robots [robot_n].init_configuration.y, 0));
		obj.transform.Rotate(new Vector3(0, 0, robots [robot_n].init_configuration.z));

		//把polygon畫成藍色
		obj.GetComponent<Renderer> ().material.color = new Color (0.4f, 0.4f, 1.0f, 0.0f);
		//======================================

		//====  畫goal robot ============
		GameObject obj2 = new GameObject ("Goal");
		obj2.AddComponent (typeof(MeshRenderer));
		MeshFilter filter2 = obj2.AddComponent(typeof(MeshFilter)) as MeshFilter;
		filter2.mesh = msh;

		obj2.transform.Translate (new Vector3(robots [robot_n].goal_configuration.x, robots [robot_n].goal_configuration.y, 0));
		obj2.transform.Rotate(new Vector3(0, 0, robots [robot_n].goal_configuration.z));

		//把polygon畫成綠色
		obj2.GetComponent<Renderer> ().material.color = new Color (0.4f, 1.0f, 0.4f, 0.0f);
		//======================================
	}
}
