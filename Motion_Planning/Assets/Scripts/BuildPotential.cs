using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuildPotential : MonoBehaviour {
	public static int[,] bitmap= new int[129,129]; //建立一個128*128大小的二維陣列，儲存bitmap的資訊
	public Texture2D BitmapImage;
	bool ok = false;
	//bool draw_ok = false;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 129; i++)
			for (int j = 0; j < 129; j++)
				bitmap [i,j] = 254; //初始化所有值為254

		BitmapImage = new Texture2D (128, 128);
		//OnGUI ();
	}

	// Update is called once per frame
	void Update () {
		if (DrawRobot.robotIsReady && DrawObstacle.obstacleIsReady && !ok) //確定robot & obstacle已經讀檔完成，才開始畫Bitmap
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
						if(hit.collider.gameObject.name == "Obstacle") //如果射到的是障礙物才會填為255
							bitmap [i, j] = 255;
					}
				}
			//===========================================

			//========  NF1(goal設為0，並往外擴散)  ==========
			List<Vector2> goal_points = new List<Vector2>();
			for (int i = 0; i < DrawRobot.robots[0].control_points.Count; i++) //先算出機器人的第i個control point的位置，並存到list中
			{
				Vector2 temp_point = new Vector2(DrawRobot.robots[0].control_points[i].x, DrawRobot.robots[0].control_points[i].y);

				double angle = DrawRobot.robots[0].goal_configuration.z;

				float rotate_x = ((float)Math.Cos(angle * (Math.PI / 180.0))*temp_point.x) - ((float)Math.Sin(angle * (Math.PI / 180))*temp_point.y) + DrawRobot.robots[0].goal_configuration.x; //cosX - sinY +dx
				float rotate_y = ((float)Math.Sin(angle * (Math.PI / 180.0))*temp_point.x) + ((float)Math.Cos(angle * (Math.PI / 180))*temp_point.y) + DrawRobot.robots[0].goal_configuration.y; //sinX + cosY +dy
				//Debug.Log(k + " " +temp_x + " , " + temp_y);
				temp_point.x = rotate_x;
				temp_point.y = rotate_y;

				goal_points.Add (temp_point);
				bitmap [(int)temp_point.x, (int)temp_point.y] = 0;/////////////////////
			}

			List<Vector2> nf_l = new List<Vector2>();
			Vector2 temp_point2 = new Vector2((int)goal_points[0].x +1,(int)goal_points[0].y); //右
			nf_l.Add(temp_point2);
			temp_point2.x = (int)goal_points [0].x;
			temp_point2.y = (int)goal_points [0].y+1;//上
			nf_l.Add(temp_point2);
			temp_point2.x = (int)goal_points [0].x;
			temp_point2.y = (int)goal_points [0].y-1;//下
			nf_l.Add(temp_point2);
			temp_point2.x = (int)goal_points [0].x-1;
			temp_point2.y = (int)goal_points [0].y;//左
			nf_l.Add(temp_point2);

			//Debug.Log(nf_l[0]);
			//nf_l.RemoveAt (0);
			int curr_potential_count = 0;
			int potential_value = 0;
			int x = 0, y = 0;

			while (nf_l.Count != 0) 
			{
				curr_potential_count = nf_l.Count; //紀錄此輪有幾個點要檢查
				for (int i = 0; i < curr_potential_count; i++) 
				{
					//Debug.Log(bitmap [(int)nf_l[i].x, (int)nf_l[i].y]);
					x = (int)nf_l [i].x;
					y = (int)nf_l [i].y;
					if ((nf_l [i].x > 128) || (nf_l [i].x < 0) || (nf_l [i].y > 128) || (nf_l [i].y < 0) || (bitmap [(int)nf_l [i].x, (int)nf_l [i].y] != 254))
					{} 
					else 
					{
						bitmap [(int)nf_l [i].x, (int)nf_l [i].y] = potential_value + 1;

						if ((y + 1 < 128) && (bitmap [x, y + 1] == 254)) {
							temp_point2.x = x;
							temp_point2.y = y+1;//上
							nf_l.Add(temp_point2);
						}
							
						if ((y - 1 > 0) && (bitmap [x, y - 1] == 254)) {
							temp_point2.x = x;
							temp_point2.y = y-1;//下
							nf_l.Add(temp_point2);
						}
							
						if ((x + 1 < 128) && (bitmap [x + 1, y] == 254)) {
							temp_point2.x = x+1;
							temp_point2.y = y;//右
							nf_l.Add(temp_point2);
						}
							
						if ((x - 1 > 0) && (bitmap [x - 1, y] == 254)) {
							temp_point2.x = x-1;
							temp_point2.y = y;//左
							nf_l.Add(temp_point2);
						}
					}

				}

				for (int i = 0; i < curr_potential_count; i++) {
					nf_l.RemoveAt (0);
				}
				potential_value++;
			}

			//Debug.Log(" " +(int)goal_points[0].x + " , " + (int)goal_points[0].y);
			//NF_ONE ((int)goal_points[0].x, (int)goal_points[0].y+1, 0);
			//NF_ONE ((int)goal_points[0].x, (int)goal_points[0].y-1, 0);
			//Debug.Log(" " +bitmap [(int)goal_points[0].x ,(int)goal_points[0].y-1]);
			//NF_ONE ((int)goal_points[0].x+1, (int)goal_points[0].y, 0);
			//NF_ONE ((int)goal_points[0].x-1, (int)goal_points[0].y, 0);
			//Debug.Log("ok");
			//===========================================
		}
	}
		
	void OnGUI() //畫出Potential Field到螢幕上
	{
		//if (DrawRobot.robotIsReady && DrawObstacle.obstacleIsReady && !draw_ok) //確定robot & obstacle已經讀檔完成，才開始畫Bitmap
		if (DrawRobot.robotIsReady && DrawObstacle.obstacleIsReady) //確定robot & obstacle已經讀檔完成，才開始畫Bitmap
		{
			//BitmapImage = new Texture2D (128, 128);
			Rect rect = new Rect (0, 0, 128, 128);
			//rect.center = new Vector2 (Screen.width / 2, Screen.height / 2);
			rect.center = new Vector2 (100, 150);

			for (int i = 0; i < 128; i++) {
				for (int j = 0; j < 128; j++) {
					//if (bitmap [i, j] == 255)
					//	BitmapImage.SetPixel (i, j, Color.black);
					//if (bitmap [i, j] == 0)
					//	BitmapImage.SetPixel (i, j, Color.white);
					//if(i == 92)
					//	Debug.Log(i + " , " + j + " , " + bitmap [i, j]);
					BitmapImage.SetPixel (i, j, new Vector4(1.0F-0.005F*(float)bitmap [i, j], 1.0F-0.005F*(float)bitmap [i, j], 1.0F-0.005F*(float)bitmap [i, j], 1F));
				}
			}

			BitmapImage.Apply ();
			GUI.DrawTexture (rect, BitmapImage);
			//draw_ok = true;
		}	
	}

	bool NF_ONE(int x, int y, int front_bit)
	{
		if ((x > 128) || (x < 0) || (y > 128) || (y < 0) || (bitmap [x, y] != 254)) {
			return false;
		}
		//else
		//{
			//if (bitmap [x, y] == 254) { //254代表未拜訪過

				bitmap [x, y] = front_bit + 1;
				
				NF_ONE (x, y+1, front_bit + 1);
					//Debug.Log(x + " , " + y + " ,bit: " + bitmap [x, y]);
				NF_ONE (x+1, y, front_bit + 1);
				NF_ONE (x, y-1, front_bit + 1);
				
				NF_ONE (x-1, y, front_bit + 1);
				return true;
			//}
		//}

		/*if ((y+1 < 128) &&(bitmap [x, y+1] == 254))
			NF_ONE (x, y+1, front_bit + 1);

		if ((y-1 > 0) && (bitmap [x, y-1] == 254))
			NF_ONE (x, y-1, front_bit + 1);

		if ((x+1 < 128) &&(bitmap [x+1, y] == 254))
			NF_ONE (x+1, y, front_bit + 1);

		if ((x-1 > 0) && (bitmap [x-1, y] == 254))
			NF_ONE (x-1, y, front_bit + 1);*/

		//return false;
	}
		
}
