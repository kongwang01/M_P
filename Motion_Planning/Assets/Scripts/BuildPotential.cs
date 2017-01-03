using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuildPotential : MonoBehaviour {
	public static int[,] bitmap= new int[129,129]; //建立一個128*128大小的二維陣列，儲存bitmap的資訊
	public static int[,] bitmap2= new int[129,129]; //建立一個128*128大小的二維陣列，儲存bitmap的資訊
	public static int[,] SearchBitmap= new int[129,129]; //建立一個128*128大小的二維陣列，儲存bitmap的資訊
	public Texture2D BitmapImage;
	public Texture2D BitmapImage2;
	public Texture2D SearchImage;
	public bool ok = false;

	public bool IsDebug = true;

	//public GameObject player;
	//public GameObject goal;
	public static bool bitmap_ok = false;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 129; i++)
			for (int j = 0; j < 129; j++)
			{
                if ((i == 0) || (i == 128) || (j == 0) || (j == 128))
                {
                    bitmap[i, j] = 255; //邊框?
                    bitmap2[i, j] = 255; //邊框?
					SearchBitmap[i, j] = 255; //邊框?
                }
                else
                {
                    bitmap[i, j] = 254; //初始化所有值為254
                    bitmap2[i, j] = 254; //初始化所有值為254
					SearchBitmap[i, j] = 0; //初始化所有值為254
                }
			}

		BitmapImage = new Texture2D (129, 129);
		BitmapImage2 = new Texture2D (129, 129);
		SearchImage = new Texture2D (129, 129);
		//OnGUI ();
	}

	// Update is called once per frame
	//void Update () {

    //}

    void Update () 
    //public static void DrawPoten ()
    {
		if (DrawRobot.robotIsReady && DrawObstacle.obstacleIsReady && !ok) //確定robot & obstacle已經讀檔完成，才開始畫Bitmap
        //if (DrawRobot.robotIsReady && DrawObstacle.obstacleIsReady) //確定robot & obstacle已經讀檔完成，才開始畫Bitmap
		{
			ok = true;
			int layerMask = -1;
			float maxDistance = 10;

			//========  將障礙物的部分填為255  ==========
			for (int i = 0; i < 129; i++)
				for (int j = 0; j < 129; j++) {
					Vector2 ray_point = new Vector2((float)i, (float)j);
					RaycastHit2D hit = Physics2D.Raycast (ray_point, new Vector2(0.0F,0.0F), maxDistance, layerMask);

					if (hit.collider)
					{
						if (hit.collider.gameObject.name == "Obstacle") { //如果射到的是障礙物才會填為255
							bitmap [i, j] = 255;
							bitmap2 [i, j] = 255;
							SearchBitmap [i, j] = 255;
						}
					}
				}
			//===========================================

			//========  NF1(goal設為0，並往外擴散)  ==========
            GameObject goal = GameObject.Find("Goal_of_Robot");
			List<Vector2> goal_points = new List<Vector2>();
			for (int i = 0; i < DrawRobot.robots[0].control_points.Count; i++) //先算出機器人的第i個control point的位置，並存到list中
			{
				//Vector2 temp_point = new Vector2(DrawRobot.robots[0].control_points[i].x, DrawRobot.robots[0].control_points[i].y);

				//double angle = DrawRobot.robots[0].goal_configuration.z;

				//float rotate_x = ((float)Math.Cos(angle * (Math.PI / 180.0))*temp_point.x) - ((float)Math.Sin(angle * (Math.PI / 180))*temp_point.y) + DrawRobot.robots[0].goal_configuration.x; //cosX - sinY +dx
				//float rotate_y = ((float)Math.Sin(angle * (Math.PI / 180.0))*temp_point.x) + ((float)Math.Cos(angle * (Math.PI / 180))*temp_point.y) + DrawRobot.robots[0].goal_configuration.y; //sinX + cosY +dy
				//Debug.Log(k + " " +temp_x + " , " + temp_y);
				//temp_point.x = rotate_x;
				//temp_point.y = rotate_y;
				//Debug.Log(temp_point);

                Vector2 temp_point = new Vector2(0.0F, 0.0F);

                temp_point.x = goal.transform.FindChild("Control_Point" + (i+1)).transform.position.x;
                temp_point.y = goal.transform.FindChild("Control_Point" + (i+1)).transform.position.y;

				goal_points.Add (temp_point);
				if(i == 0)
					bitmap [(int)temp_point.x, (int)temp_point.y] = 0;
				else if(i == 1)
					bitmap2 [(int)temp_point.x, (int)temp_point.y] = 0;
			}

			SetMap (goal_points [0],1);
			SetMap (goal_points [1],2);
            //for (int i = 0; i < 129; i++)
            //    Debug.Log(i + " , 1 : " +bitmap[i, 1]);
			bitmap_ok = true;
            //ShowOnScreen();
			//===========================================
		}
	}
		
	void OnGUI() //畫出Potential Field到螢幕上
    //static void ShowOnScreen()
	{
		//if (DrawRobot.robotIsReady && DrawObstacle.obstacleIsReady && !draw_ok) //確定robot & obstacle已經讀檔完成，才開始畫Bitmap
		if (DrawRobot.robotIsReady && DrawObstacle.obstacleIsReady) //確定robot & obstacle已經讀檔完成，才開始畫Bitmap
		{
            //Texture2D BitmapImage = new Texture2D(128, 128);
            //Texture2D BitmapImage2 = new Texture2D(128, 128);

			//BitmapImage = new Texture2D (128, 128);
			Rect rect = new Rect (0, 0, 129, 129);
			//rect.center = new Vector2 (Screen.width / 2, Screen.height / 2);
			rect.center = new Vector2 (100, 80);

			for (int i = 0; i < 129; i++) {
				for (int j = 0; j < 129; j++) {
					BitmapImage.SetPixel (i, j, new Vector4(1.0F-0.005F*(float)bitmap [i, j], 1.0F-0.005F*(float)bitmap [i, j], 1.0F-0.005F*(float)bitmap [i, j], 1F));
				}
			}
            //Debug.Log("ok");
			BitmapImage.Apply ();
            //GameObject poten1 = GameObject.Find("Potential1");
            //poten1.AddComponent(typeof(MeshRenderer));
            //poten1.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            //poten1.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Color");
            //poten1.GetComponent<Renderer>().material.mainTexture = BitmapImage;
			GUI.DrawTexture (rect, BitmapImage);
			//draw_ok = true;



			Rect rect2 = new Rect (0, 0, 129, 129);
			//rect.center = new Vector2 (Screen.width / 2, Screen.height / 2);
			rect2.center = new Vector2 (100, 250);

			for (int i = 0; i < 129; i++) {
				for (int j = 0; j < 129; j++) {
					BitmapImage2.SetPixel (i, j, new Vector4(1.0F-0.005F*(float)bitmap2 [i, j], 1.0F-0.005F*(float)bitmap2 [i, j], 1.0F-0.005F*(float)bitmap2 [i, j], 1F));
				}
			}

			BitmapImage2.Apply ();
			GUI.DrawTexture (rect2, BitmapImage2);

			if (IsDebug) 
			{
				Rect rect3 = new Rect (0, 0, 129, 129);
				//rect.center = new Vector2 (Screen.width / 2, Screen.height / 2);
				rect3.center = new Vector2 (100, 450);

				for (int i = 0; i < 129; i++) {
					for (int j = 0; j < 129; j++) {
						if(SearchBitmap [i, j] == 250) //路徑設為藍色
							SearchImage.SetPixel (i, j, Color.blue);
						else
							SearchImage.SetPixel (i, j, new Vector4(1.0F-0.005F*(float)SearchBitmap [i, j], 1.0F-0.005F*(float)SearchBitmap [i, j], 1.0F-0.005F*(float)SearchBitmap [i, j], 1F));
					}
				}

				SearchImage.Apply ();
				GUI.DrawTexture (rect3, SearchImage);
			}
		}	
	}

    static void SetMap(Vector2 goal_point, int map_count)
	{
		List<Vector2> nf_l = new List<Vector2>();
		Vector2 temp_point = new Vector2((int)goal_point.x +1,(int)goal_point.y); //右
		nf_l.Add(temp_point);
		temp_point.x = (int)goal_point.x;
		temp_point.y = (int)goal_point.y+1;//上
		nf_l.Add(temp_point);
		temp_point.x = (int)goal_point.x;
		temp_point.y = (int)goal_point.y-1;//下
		nf_l.Add(temp_point);
		temp_point.x = (int)goal_point.x-1;
		temp_point.y = (int)goal_point.y;//左
		nf_l.Add(temp_point);


		int curr_potential_count = 0;
		int potential_value = 0;
		int x = 0, y = 0;

		while (nf_l.Count != 0) 
		{
			curr_potential_count = nf_l.Count; //紀錄此輪有幾個點要檢查
			for (int i = 0; i < curr_potential_count; i++) 
			{
				x = (int)nf_l [i].x;
				y = (int)nf_l [i].y;
				if ((nf_l [i].x > 128) || (nf_l [i].x < 0) || (nf_l [i].y > 128) || (nf_l [i].y < 0)) 
				{
				} 
				else if ((map_count==1) && (bitmap [(int)nf_l [i].x, (int)nf_l [i].y] != 254))
				{
				}
				else if ((map_count==2) && (bitmap2 [(int)nf_l [i].x, (int)nf_l [i].y] != 254))
				{
				}
				else 
				{
					if (map_count == 1) 
					{
						bitmap [(int)nf_l [i].x, (int)nf_l [i].y] = potential_value + 1;

						if ((y + 1 < 128) && (bitmap [x, y + 1] == 254)) {
							temp_point.x = x;
							temp_point.y = y+1;//上
							nf_l.Add(temp_point);
						}

						if ((y - 1 >= 0) && (bitmap [x, y - 1] == 254)) {
							temp_point.x = x;
							temp_point.y = y-1;//下
							nf_l.Add(temp_point);
						}

						if ((x + 1 < 128) && (bitmap [x + 1, y] == 254)) {
							temp_point.x = x+1;
							temp_point.y = y;//右
							nf_l.Add(temp_point);
						}

						if ((x - 1 >= 0) && (bitmap [x - 1, y] == 254)) {
							temp_point.x = x-1;
							temp_point.y = y;//左
							nf_l.Add(temp_point);
						}
					}
					else if(map_count == 2)
					{
						bitmap2 [(int)nf_l [i].x, (int)nf_l [i].y] = potential_value + 1;

						if ((y + 1 < 128) && (bitmap2 [x, y + 1] == 254)) {
							temp_point.x = x;
							temp_point.y = y+1;//上
							nf_l.Add(temp_point);
						}

						if ((y - 1 >= 0) && (bitmap2 [x, y - 1] == 254)) {
							temp_point.x = x;
							temp_point.y = y-1;//下
							nf_l.Add(temp_point);
						}

						if ((x + 1 < 128) && (bitmap2 [x + 1, y] == 254)) {
							temp_point.x = x+1;
							temp_point.y = y;//右
							nf_l.Add(temp_point);
						}

						if ((x - 1 >= 0) && (bitmap2 [x - 1, y] == 254)) {
							temp_point.x = x-1;
							temp_point.y = y;//左
							nf_l.Add(temp_point);
						}
					}


				}

			}

			for (int i = 0; i < curr_potential_count; i++) {
				nf_l.RemoveAt (0);
			}
			potential_value++;
		}
	}
}



/*void SetMap(Vector2 goal_point)
{
	List<Vector2> nf_l = new List<Vector2>();
	Vector2 temp_point = new Vector2((int)goal_point.x +1,(int)goal_point.y); //右
	nf_l.Add(temp_point);
	temp_point.x = (int)goal_point.x;
	temp_point.y = (int)goal_point.y+1;//上
	nf_l.Add(temp_point);
	temp_point.x = (int)goal_point.x;
	temp_point.y = (int)goal_point.y-1;//下
	nf_l.Add(temp_point);
	temp_point.x = (int)goal_point.x-1;
	temp_point.y = (int)goal_point.y;//左
	nf_l.Add(temp_point);


	int curr_potential_count = 0;
	int potential_value = 0;
	int x = 0, y = 0;

	while (nf_l.Count != 0) 
	{
		curr_potential_count = nf_l.Count; //紀錄此輪有幾個點要檢查
		for (int i = 0; i < curr_potential_count; i++) 
		{
			x = (int)nf_l [i].x;
			y = (int)nf_l [i].y;
			if ((nf_l [i].x > 128) || (nf_l [i].x < 0) || (nf_l [i].y > 128) || (nf_l [i].y < 0) || (bitmap [(int)nf_l [i].x, (int)nf_l [i].y] != 254))
			{} 
			else 
			{
				bitmap [(int)nf_l [i].x, (int)nf_l [i].y] = potential_value + 1;

				if ((y + 1 < 128) && (bitmap [x, y + 1] == 254)) {
					temp_point.x = x;
					temp_point.y = y+1;//上
					nf_l.Add(temp_point);
				}

				if ((y - 1 > 0) && (bitmap [x, y - 1] == 254)) {
					temp_point.x = x;
					temp_point.y = y-1;//下
					nf_l.Add(temp_point);
				}

				if ((x + 1 < 128) && (bitmap [x + 1, y] == 254)) {
					temp_point.x = x+1;
					temp_point.y = y;//右
					nf_l.Add(temp_point);
				}

				if ((x - 1 > 0) && (bitmap [x - 1, y] == 254)) {
					temp_point.x = x-1;
					temp_point.y = y;//左
					nf_l.Add(temp_point);
				}
			}

		}

		for (int i = 0; i < curr_potential_count; i++) {
			nf_l.RemoveAt (0);
		}
		potential_value++;
	}
}*/
