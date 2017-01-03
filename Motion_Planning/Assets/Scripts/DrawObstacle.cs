using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class DrawObstacle : MonoBehaviour {
	public static List<Obstacle> obstacles = new List<Obstacle>(); //用來儲存障礙物
	public static bool obstacleIsReady = false;
	public static string obstacle_path = Application.dataPath + "/Resources/obstacle.dat";

	public static void DrawObstacles () {
		int n_of_obstacles = 0;
		int n_of_polygons = 0;
		//======  存讀檔   =======================================================
		//string path = Application.dataPath + "/Resources/obstacle.dat";
		string path = obstacle_path;
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
			temp_o.curr_configuration = v3; //讀檔時的位置為初始位置，如果使用者移動or旋轉物件，資訊會存在curr_configuration

			obstacles.Add(temp_o);		
			line++;
		}	
		//=======================================================================

		//==========  把configuration完的點存起來  =====================================================
		for(int i=0; i<obstacles.Count; i++)
		{
			for(int j=0; j<obstacles[i].n_of_polygons; j++)
			{
				for(int k=0; k<obstacles[i].polygons[j].n_of_vertices; k++)
				{
					double angle = obstacles [i].curr_configuration.z;
					if (angle > 180.0)
						angle -= 180.0;
					//Debug.Log (angle);
					temp_x = obstacles [i].polygons [j].vertices [k].x;
					temp_y = obstacles [i].polygons [j].vertices [k].y;
					//temp_x = temp_x + obstacles [i].curr_configuration.x; //未加上旋轉
					//temp_y = temp_y + obstacles [i].curr_configuration.y;
					float rotate_x = ((float)Math.Cos(angle * (Math.PI / 180.0))*temp_x) - ((float)Math.Sin(angle * (Math.PI / 180))*temp_y) + obstacles [i].curr_configuration.x; //cosX - sinY +dx
					float rotate_y = ((float)Math.Sin(angle * (Math.PI / 180.0))*temp_x) + ((float)Math.Cos(angle * (Math.PI / 180))*temp_y) + obstacles [i].curr_configuration.y; //sinX + cosY +dy
					//Debug.Log(k + " " +temp_x + " , " + temp_y);
					Vector2 v2= new Vector2(rotate_x, rotate_y);

					obstacles[i].polygons[j].config_vertices.Add(v2);
				}
			}
		}	
		//=======================================================================

        //==========  畫背景  =====================================================
        Vector2[] vertices2D_backGround = new Vector2[] {
            new Vector2(0,0),
            new Vector2(0,128),
            new Vector2(128,128),
            new Vector2(128,0),
        };

        GameObject backGroundObj = Polygon.DrawPolygon(vertices2D_backGround);
        //把BackGround畫成白色
		backGroundObj.GetComponent<Renderer>().material.color = new Color(0.3f, 0.3f, 0.3f, 0.0f);
		backGroundObj.GetComponent<Renderer> ().material.shader = Shader.Find ("Unlit/Color");
        backGroundObj.name = "BackGround";
        backGroundObj.transform.Translate(0.0f, 0.0f, 1.0f);

		//==========  繪圖  =====================================================
		Vector2[] vertices2D;

		for (int i = 0; i < n_of_obstacles; i++) 
		{
            //GameObject parentObj;
            if (obstacles[i].n_of_polygons > 1)
            {
                GameObject parentObj = new GameObject("Obstacle");
                for (int j = 0; j < obstacles[i].n_of_polygons; j++)
                {
                    vertices2D = obstacles[i].polygons[j].vertices.ToArray();
                    GameObject childObj = Polygon.DrawPolygon(vertices2D);
                    childObj.transform.parent = parentObj.transform;

                    PolygonCollider2D collider = parentObj.AddComponent(typeof(PolygonCollider2D)) as PolygonCollider2D;
                    collider.points = vertices2D;
                }
                //parentObj.AddComponent(Type.GetType("TransAndRotateForPolygon"));
                parentObj.transform.Translate(new Vector3(obstacles[i].init_configuration.x, obstacles[i].init_configuration.y, 0));
                parentObj.transform.Rotate(new Vector3(0, 0, obstacles[i].init_configuration.z));
            }
            else
            {
                GameObject parentObj;
                //for (int j = 0; j < obstacles[i].n_of_polygons; j++)
                //{
                    vertices2D = obstacles[i].polygons[0].vertices.ToArray();
                    parentObj = Polygon.DrawPolygon(vertices2D);

                    PolygonCollider2D collider = parentObj.AddComponent(typeof(PolygonCollider2D)) as PolygonCollider2D;
                    collider.points = vertices2D;
                //}
               //parentObj.AddComponent(Type.GetType("TransAndRotateForPolygon"));
                parentObj.transform.Translate(new Vector3(obstacles[i].init_configuration.x, obstacles[i].init_configuration.y, 0));
                parentObj.transform.Rotate(new Vector3(0, 0, obstacles[i].init_configuration.z));
                parentObj.name = "Obstacle";          
            }
		}
			
		//==========================================================================================
		obstacleIsReady = true;
	}

	// Update is called once per frame
	void Update () {

	}

}
