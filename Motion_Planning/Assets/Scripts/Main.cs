using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {
	public static int[,] bitmap= new int[129,129]; //建立一個128*128大小的二維陣列，儲存bitmap的資訊
	public Texture2D BitmapImage;
	bool ok = false;

	// Use this for initialization
	void Start () {
		//bitmap = new int[128][128];
		//for (int i = 0; i < 128; i++)
		//	bitmap [i][] = new int*[128];

		for (int i = 0; i < 129; i++)
			for (int j = 0; j < 129; j++)
				bitmap [i,j] = 254; //初始化所有值為254

		/*GameObject obj = new GameObject("BITMAP");
		obj.AddComponent(typeof(Renderer));

		Texture2D texture = new Texture2D(128, 128);
		obj.GetComponent<Renderer>().material.mainTexture = texture;

		for (int y = 0; y < texture.height; y++) {
			for (int x = 0; x < texture.width; x++) {
				Color color = ((x & y) != 0 ? Color.white : Color.gray);
				texture.SetPixel(x, y, color);
			}
		}
		texture.Apply();*/
		BitmapImage = new Texture2D (128, 128);
		OnGUI ();
	}

	// Update is called once per frame
	void Update () {
		if (DrawRobot.robotIsReady && DrawObstacle.obstacleIsReady && !ok) //確定robot & obstacle已經讀檔完成，才開始畫Bitmap
		{
			ok = true;
			Vector2 xmin = new Vector2(0.0f,0.0f); //用來紀錄該polygon的頂點中，x最小的頂點之中y最小的頂點
			Vector2 xmax = new Vector2(0.0f,0.0f);
			for (int i = 0; i < DrawObstacle.obstacles.Count; i++) //第i個障礙物
			{
				for (int j = 0; j < DrawObstacle.obstacles [i].n_of_polygons; j++) //第i個障礙物由幾個polygon組成
				{
					for (int k = 0; k < DrawObstacle.obstacles [i].polygons [j].config_vertices.Count; k++) //polygon有幾個頂點
					{
						//先畫出polygon的外框
						if(k == (DrawObstacle.obstacles [i].polygons [j].config_vertices.Count -1))
							Bitmap_Drawline (DrawObstacle.obstacles [i].polygons [j].config_vertices [k], DrawObstacle.obstacles [i].polygons [j].config_vertices [0]);
						else
							Bitmap_Drawline (DrawObstacle.obstacles [i].polygons [j].config_vertices [k], DrawObstacle.obstacles [i].polygons [j].config_vertices [k+1]);
						//int x = (int)DrawObstacle.obstacles [i].polygons [j].config_vertices [k].x;
						//x = x + (int)DrawObstacle.obstacles[i].init_configuration.x;
						//int y = (int)DrawObstacle.obstacles [i].polygons [j].config_vertices [k].y;
						//y = y + (int)DrawObstacle.obstacles[i].init_configuration.y;

						//bitmap [x, y] = 255;
						//BitmapImage.SetPixel(x,y,Color.white);

						if (k == 0) 
						{
							xmin.x = DrawObstacle.obstacles [i].polygons [j].config_vertices [k].x;
							xmin.y = DrawObstacle.obstacles [i].polygons [j].config_vertices [k].y;

							xmax.x = DrawObstacle.obstacles [i].polygons [j].config_vertices [k].x;
							xmax.y = DrawObstacle.obstacles [i].polygons [j].config_vertices [k].y;
						} 
						else 
						{
							Debug.Log("xmin " + (int)xmin.x + " , " + (int)xmin.y);
							Debug.Log("curr " + (int)DrawObstacle.obstacles [i].polygons [j].config_vertices [k].x + " , " + (int)DrawObstacle.obstacles [i].polygons [j].config_vertices [k].y);
							if ((int)DrawObstacle.obstacles [i].polygons [j].config_vertices [k].x < (int)xmin.x) { //更新xmin
								xmin.x = DrawObstacle.obstacles [i].polygons [j].config_vertices [k].x;
								xmin.y = DrawObstacle.obstacles [i].polygons [j].config_vertices [k].y;
								Debug.Log(1);
							}

							if ((int)DrawObstacle.obstacles [i].polygons [j].config_vertices [k].x == (int)xmin.x) { //更新xmin
								if ((int)DrawObstacle.obstacles [i].polygons [j].config_vertices [k].y < (int)xmin.y) {
									xmin.x = DrawObstacle.obstacles [i].polygons [j].config_vertices [k].x;
									xmin.y = DrawObstacle.obstacles [i].polygons [j].config_vertices [k].y;
									Debug.Log(2);
								}
							}

							if ((int)DrawObstacle.obstacles [i].polygons [j].config_vertices [k].x > (int)xmax.x) { //更新xmax
								xmax.x = DrawObstacle.obstacles [i].polygons [j].config_vertices [k].x;
								xmax.y = DrawObstacle.obstacles [i].polygons [j].config_vertices [k].y;
								Debug.Log(3);
							}
						}

						Debug.Log(k);
					}

					Debug.Log("final xmin " + (int)xmin.x + " , " + (int)xmin.y);
					Debug.Log("final xmax " + (int)xmax.x + " , " + (int)xmax.y);
					//將polygon填滿
					int searchX = (int)xmin.x;
					int searchY = (int)xmin.y;
					while (searchX < (int)xmax.x) //從xmin掃到xmax
					{
						Debug.Log("Search " + searchX + " , " + searchY + " , "+ bitmap [searchX + 1, searchY]);
						Debug.Log(bitmap [searchX + 1, searchY + 1]);
						Debug.Log(bitmap [searchX + 1, searchY - 1]);
						if (bitmap [searchX + 1, searchY] == 255) {
							searchX = searchX + 1;
						}
						else if(bitmap [searchX + 1, searchY - 1] == 255) {
							searchX = searchX + 1;
							searchY = searchY - 1;
						}
						else if(bitmap [searchX + 1, searchY + 1] == 255) {
							searchX = searchX + 1;
							searchY = searchY + 1;
						}

						Debug.Log("final xmin " + searchX + " , " + searchY);
						searchX++;
						while (bitmap [searchX, searchY] != 255) //每個x由下往上掃，直到碰到邊框
						{ 
							bitmap [searchX, searchY] = 255;

							searchY++;
						}
						//searchX++;
						//break;
					}
						

					//Debug.Log (DrawObstacle.obstacles [i].polygons [j].config_vertices.Count);
					/*if (DrawObstacle.obstacles [i].polygons [j].config_vertices.Count == 4) //暴力法填邊框
					{
						Bitmap_Drawline (DrawObstacle.obstacles [i].polygons [j].config_vertices [0], DrawObstacle.obstacles [i].polygons [j].config_vertices [1]);
						Bitmap_Drawline (DrawObstacle.obstacles [i].polygons [j].config_vertices [1], DrawObstacle.obstacles [i].polygons [j].config_vertices [2]);
						Bitmap_Drawline (DrawObstacle.obstacles [i].polygons [j].config_vertices [2], DrawObstacle.obstacles [i].polygons [j].config_vertices [3]);
						Bitmap_Drawline (DrawObstacle.obstacles [i].polygons [j].config_vertices [3], DrawObstacle.obstacles [i].polygons [j].config_vertices [0]);
					}

					if (DrawObstacle.obstacles [i].polygons [j].config_vertices.Count == 6) 
					{
						Bitmap_Drawline (DrawObstacle.obstacles [i].polygons [j].config_vertices [0], DrawObstacle.obstacles [i].polygons [j].config_vertices [1]);
						Bitmap_Drawline (DrawObstacle.obstacles [i].polygons [j].config_vertices [1], DrawObstacle.obstacles [i].polygons [j].config_vertices [2]);
						Bitmap_Drawline (DrawObstacle.obstacles [i].polygons [j].config_vertices [2], DrawObstacle.obstacles [i].polygons [j].config_vertices [3]);
						Bitmap_Drawline (DrawObstacle.obstacles [i].polygons [j].config_vertices [3], DrawObstacle.obstacles [i].polygons [j].config_vertices [4]);
						Bitmap_Drawline (DrawObstacle.obstacles [i].polygons [j].config_vertices [4], DrawObstacle.obstacles [i].polygons [j].config_vertices [5]);
						Bitmap_Drawline (DrawObstacle.obstacles [i].polygons [j].config_vertices [5], DrawObstacle.obstacles [i].polygons [j].config_vertices [0]);
					}*/
				}
			}
			
			//BitmapImage.Apply ();
			//ok = true;
		}
	}


	void OnGUI() //畫出Potential Field到螢幕上
	{
		if (DrawRobot.robotIsReady && DrawObstacle.obstacleIsReady) //確定robot & obstacle已經讀檔完成，才開始畫Bitmap
		{
			//BitmapImage = new Texture2D (128, 128);
			Rect rect = new Rect (0, 0, 128, 128);
			//rect.center = new Vector2 (Screen.width / 2, Screen.height / 2);
			rect.center = new Vector2 (100, 150);

			for (int i = 0; i < 128; i++)
					for (int j = 0; j < 128; j++)
						if(bitmap[i,j] == 255)
							BitmapImage.SetPixel(i,j,Color.black);

			BitmapImage.Apply ();
			GUI.DrawTexture (rect, BitmapImage);
		}	
	}

	//利用CG畫線的構想
	/*void Bitmap_Drawline(Vector2 p1, Vector2 p2) //p1, p2為configuration過後的點
	{
		for (int i = 0; i <= 100; i++)
		{
			float a = 0.01F * i;
			float x = a * p1.x + (1 - a) * p2.x;
			float y = a * p1.y + (1 - a) * p2.y;

			//Debug.Log((int)x + " , " + (int)y);

			bitmap [(int)x,(int)y] = 255;
		}
	}*/

	//利用老師PPT的方法
	void Bitmap_Drawline(Vector2 p1, Vector2 p2) //p1, p2為configuration過後的點
	{
		int d = 0;
		float dx = 0.0F, dy = 0.0F;

		if (Mathf.Abs (p2.x - p1.x) > Mathf.Abs (p2.y - p1.y))
			d = (int)Mathf.Abs (p2.x - p1.x);
		else
			d = (int)Mathf.Abs (p2.y - p1.y);

		dx = (p2.x - p1.x) / (float)d;
		dy = (p2.y - p1.y) / (float)d;

		for (int i = 0; i < d; i++)
		{
			//float a = 0.02F * i;
			float x = p1.x + i * dx;
			float y = p1.y + i * dy;
			Debug.Log(x + " , " + y);
			Debug.Log((int)x + " , " + (int)y);

			//if (x > 128.0F)
			//	x = 128.0F;
			//if (y > 128.0F)
			//	y = 128.0F;

			bitmap [(int)x,(int)y] = 255;
		}
	}

}
