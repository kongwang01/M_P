using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class PolygonCreater : MonoBehaviour {
	List<Obstacle> obstacles = new List<Obstacle>(); //用來儲存障礙物

	// Use this for initialization
	void Start () {
		
		int n_of_obstacles = 0;
		int n_of_polygons = 0;
		//======  存讀檔   ===================================================
		string path = Application.dataPath + "/Resources/obstacle.dat";
        if (!File.Exists(path)){
			Debug.Log("Error Path: " + path);
			return;
		}
        StreamReader sr = File.OpenText(path);
        string input = ""; 
		//int mode = 0;
		
		List<string> input_string = new List<string>(); //把讀進來的檔案之數字部分存起來
		while (true) 
        {
            input = sr.ReadLine(); 
			//Debug.Log(input);

            if (input == null)
            {
                break;
            }
			if((input.Length > 0) && (input[0] != '#') && (input[0] != 'n'))
				input_string.Add(input);
				//Debug.Log(input);
		}
		sr.Close();
		
		//for(int i=0; i<input_string.Count; i++)
		//	Debug.Log(input_string[i]);
		
		//========    把資料存進結構裡    =============================
		//List<Obstacle> obstacles = new List<Obstacle>();
		n_of_obstacles = Convert.ToInt32( input_string[0] );
		int line = 1;
		
		Obstacle temp_o;
		Polygon temp_p;
		
		float temp_x = 0.0F;
		float temp_y = 0.0F;
		float temp_z = 0.0F;
		
		for(int i=0; i<n_of_obstacles; i++)
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
					//Debug.Log(temp_x);
					//Debug.Log(temp_y);
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
		//============================================================
		
       /* while (true) 
        {
            input = sr.ReadLine(); 
			//Debug.Log(input);

            if (input == null)
            {
                break;
            }
			if((input.Length > 0) && (input[0] != '#') && (input[0] != 'n'))
				//Debug.Log(input);
            //i = Int32.Parse( input );
				if(mode == 0)
				{
					n_of_obstacles = Convert.ToInt32( input );
					mode = 1;
					for(int i=0; i<n_of_obstacles; i++)
					{
						input = sr.ReadLine(); //讀入number of polygons
						n_of_polygons = Convert.ToInt32( input );
						for(int j=0; j<n_of_polygons; j++)
						{
							input = sr.ReadLine(); //讀入number of vertices
							Polygon temp_p;
							temp_p.n_of_vertices = Convert.ToInt32( input );
							for(int k=0; k<temp_p.n_of_vertices; k++)
							{
								string[] sArray = input.Split(' ');
								float temp_x = Convert.ToSingle( sArray[0] );
								float temp_y = Convert.ToSingle( sArray[1] );
								Vector2 v2= new Vector2(temp_x, temp_y);
								
							}
						}	
					}
				}
				else
				{
					//int nop = Int32.Parse( input );
					
				}
        }*/
		//======================================================================


		//==========  繪圖  =====================================================
		//for(int n=0; n<){
			// Create Vector2 vertices
			//Vector2[] vertices2D = new Vector2[] {
			//	new Vector2(0,0),
			// new Vector2(0,50),
				//new Vector2(50,50),
				//new Vector2(50,100),
			// new Vector2(0,100),
			//	new Vector2(0,150),
			//	new Vector2(150,150),
			// new Vector2(150,100),
			// new Vector2(100,100),
			// new Vector2(100,50),
			// new Vector2(150,50),
			//	new Vector2(150,0),
			//};

		Vector2[] vertices2D;

		for (int i = 0; i < n_of_obstacles; i++) 
		{			
			//Debug.Log(obstacles[i].n_of_polygons);
			for (int j = 0; j < obstacles[i].n_of_polygons; j++) 
			{
				vertices2D = obstacles[i].polygons[j].vertices.ToArray();
				//DrawPolygon (vertices2D);
				DrawPolygon (vertices2D, i);
				//Debug.Log(i+j);
			}
		}

		//Vector2[] vertices2D = obstacles [0].polygons [0].vertices.ToArray ();
		//DrawPolygon (vertices2D);

		//vertices2D = obstacles [1].polygons [0].vertices.ToArray ();
		//DrawPolygon (vertices2D);

		//}
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


		//==================  換顏色  ================================================
		/*Color colorT = new Color(UnityEngine.Random.Range (0.0f, 1.0f), UnityEngine.Random.Range (0.0f, 1.0f), UnityEngine.Random.Range (0.0f, 1.0f), 1.0f);
			
			// create new colors array where the colors will be created.
			Color[] colors = new Color[vertices.Length];
	
			for (int i = 0; i < vertices.Length; i++)
				colors[i] = colorT;
				//colors[i] = Color.Lerp(Color.red, Color.green, vertices[i].y);
			
			// assign the array of colors to the Mesh.
			msh.colors = colors;*/

		//==========================================================================



		// Set up game object with mesh;
		//gameObject.AddComponent(typeof(MeshRenderer));
		//MeshFilter filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
		//filter.mesh = msh;

		GameObject obj = new GameObject ("Obstacle");
		obj.AddComponent (typeof(MeshRenderer));
		MeshFilter filter = obj.AddComponent(typeof(MeshFilter)) as MeshFilter;
		filter.mesh = msh;

		obj.transform.Translate (new Vector3(obstacles [obstacle_n].init_configuration.x, obstacles [obstacle_n].init_configuration.x, 0));
		obj.transform.Rotate(new Vector3(0, 0, obstacles [obstacle_n].init_configuration.z));

		// Assigns a material named "Assets/Materials/Blue" to the object.
		//obj.AddComponent (typeof(MeshRenderer)); 
		//Material newMat = Resources.Load("Materials/Blue", typeof(Material)) as Material;
		//obj.GetComponent<MeshRenderer> ().material = newMat;

		obj.GetComponent<Renderer> ().material.color = new Color (0.4f, 0.4f, 1.0f, 0.0f);

		//obj.GetComponent<Renderer> ().material.color = Color.blue;

	}
}
	