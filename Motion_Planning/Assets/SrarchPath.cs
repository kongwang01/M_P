using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SrarchPath : MonoBehaviour {
	public GameObject player;
	public GameObject goal;

    int curr_x = 0;
	int curr_y = 0;
    int curr_potential_value = 0;

    int curr_x2 = 0;
    int curr_y2 = 0;
    int curr_potential_value2 = 0;

    List<Vector2> robot_edges_point = new List<Vector2>(); //用來儲存robot邊上的所有點

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Robot");
        goal = GameObject.Find("Goal_of_Robot");

        curr_x = (int)player.transform.FindChild("Control_Point1").transform.position.x;
        curr_y = (int)player.transform.FindChild("Control_Point1").transform.position.y;
        curr_potential_value = BuildPotential.bitmap[curr_x, curr_y];

        curr_x2 = (int)player.transform.FindChild("Control_Point2").transform.position.x;
        curr_y2 = (int)player.transform.FindChild("Control_Point2").transform.position.y;
        curr_potential_value2 = BuildPotential.bitmap2[curr_x2, curr_y2];


		for (int j = 0; j < DrawRobot.robots[0].n_of_polygons; j++) //第i個障礙物由幾個polygon組成
			for (int k = 0; k < DrawRobot.robots[0].polygons[j].vertices.Count; k++) //polygon有幾個頂點
			{
                Vector2 tempV21 = new Vector2(0.0F, 0.0F);
                Vector2 tempV22 = new Vector2(0.0F, 0.0F);

                //float vertex2_x = 0.0F;
                //float vertex2_y = 0.0F;

                if (k == (DrawRobot.robots[0].polygons[j].vertices.Count - 1))
                {
                    tempV21.x = player.transform.FindChild("vertex_Point" + (j + 1) + "_" + (k + 1)).transform.position.x;
                    tempV21.y = player.transform.FindChild("vertex_Point" + (j + 1) + "_" + (k + 1)).transform.position.y;

                    tempV22.x = player.transform.FindChild("vertex_Point" + (j + 1) + "_1").transform.position.x;
                    tempV22.y = player.transform.FindChild("vertex_Point" + (j + 1) + "_1").transform.position.y;
                }
                else
                {
                    tempV21.x = player.transform.FindChild("vertex_Point" + (j + 1) + "_" + (k + 1)).transform.position.x;
                    tempV21.y = player.transform.FindChild("vertex_Point" + (j + 1) + "_" + (k + 1)).transform.position.y;

                    tempV22.x = player.transform.FindChild("vertex_Point" + (j + 1) + "_" + (k + 2)).transform.position.x;
                    tempV22.y = player.transform.FindChild("vertex_Point" + (j + 1) + "_" + (k + 2)).transform.position.y;
                }

                //Debug.Log(tempV21);
                Findline(tempV21, tempV22);
            }

	}
	
	// Update is called once per frame
	void Update () {
        if (BuildPotential.bitmap_ok && (BuildPotential.bitmap[curr_x, curr_y] != 0))
        {
			//player = GameObject.Find ("Robot");
			//goal = GameObject.Find ("Goal_of_Robot");

			//Debug.Log( goal.transform.position);
			//Debug.Log( goal.transform.FindChild("Control_Point1").transform.position);
			//Debug.Log( goal.transform.FindChild("Control_Point2").transform.position);

			//while (goal.transform.FindChild("Control_Point1").transform.position != player.transform.FindChild("Control_Point1").transform.position) {
				curr_x = (int)player.transform.FindChild ("Control_Point1").transform.position.x;
				curr_y = (int)player.transform.FindChild ("Control_Point1").transform.position.y;
				curr_potential_value = BuildPotential.bitmap [curr_x, curr_y];
                Debug.Log("curr_x:" + curr_x + " , curr_y:" + curr_y + " , p: " + curr_potential_value);
                Debug.Log("curr_x:" + curr_x + " , curr_y+1:" + (curr_y+1) + " , bit_p: " + BuildPotential.bitmap[curr_x, curr_y + 1]);
                Debug.Log("curr_x:" + curr_x + " , curr_y-1:" + (curr_y - 1) + " , bit_p: " + BuildPotential.bitmap[curr_x, curr_y - 1]);
                Debug.Log("curr_x+1:" + (curr_x + 1) + " , curr_y:" + curr_y + " , bit_p: " + BuildPotential.bitmap[curr_x + 1, curr_y]);
                Debug.Log("curr_x-1:" + (curr_x - 1) + " , curr_y:" + curr_y + " , bit_p: " + BuildPotential.bitmap[curr_x - 1, curr_y]);

				if ((curr_y + 1 < 128) && (BuildPotential.bitmap [curr_x, curr_y + 1] <= curr_potential_value) && CollitionDetect(0,1)) {
                    player.transform.Translate(0, 1, 0, Space.World);
				}

                if ((curr_y - 1 > 0) && (BuildPotential.bitmap[curr_x, curr_y - 1] <= curr_potential_value) && CollitionDetect(0, -1))
                {
                    player.transform.Translate(0, -1, 0, Space.World);
				}

                if ((curr_x + 1 < 128) && (BuildPotential.bitmap[curr_x + 1, curr_y] <= curr_potential_value) && CollitionDetect(1, 0))
                {
                    player.transform.Translate(1, 0, 0, Space.World);
				}

                if ((curr_x - 1 > 0) && (BuildPotential.bitmap[curr_x - 1, curr_y] <= curr_potential_value) && CollitionDetect(-1, 0))
                {
                    player.transform.Translate(-1, 0, 0, Space.World);
				}
				//break;
			//}

			/*int curr_x2 = (int)player.transform.FindChild ("Control_Point2").transform.position.x;
			int curr_y2 = (int)player.transform.FindChild ("Control_Point2").transform.position.y;
			int curr_potential_value2 = BuildPotential.bitmap [curr_x, curr_y];
			Debug.Log(curr_potential_value);

			if ((curr_y + 1 < 128) && (BuildPotential.bitmap [curr_x, curr_y + 1] < curr_potential_value)) {
				player.transform.Translate (1, 0, 0);
			}

			if ((curr_y - 1 > 0) && (BuildPotential.bitmap [curr_x, curr_y - 1] < curr_potential_value)) {
				player.transform.Translate (-1, 0, 0);
			}

			if ((curr_x + 1 < 128) && (BuildPotential.bitmap [curr_x+1, curr_y] < curr_potential_value)) {
				player.transform.Translate (0, -1, 0);
			}

			if ((curr_x - 1 > 0) && (BuildPotential.bitmap [curr_x-1, curr_y] < curr_potential_value)) {
				player.transform.Translate (0, 1, 0);
			}*/
		}

        if (BuildPotential.bitmap[curr_x, curr_y] == 0)
        {
            curr_x2 = (int)player.transform.FindChild("Control_Point2").transform.position.x;
            curr_y2 = (int)player.transform.FindChild("Control_Point2").transform.position.y;
            curr_potential_value2 = BuildPotential.bitmap2[curr_x2, curr_y2];
        }


        if ((BuildPotential.bitmap[curr_x, curr_y] == 0) && (BuildPotential.bitmap2[curr_x2, curr_y2] != 0))
        {
           // player = GameObject.Find("Robot");
            //goal = GameObject.Find("Goal_of_Robot");

            //Debug.Log( goal.transform.position);
            //Debug.Log( goal.transform.FindChild("Control_Point1").transform.position);
            //Debug.Log( goal.transform.FindChild("Control_Point2").transform.position);

            //while (goal.transform.FindChild("Control_Point1").transform.position != player.transform.FindChild("Control_Point1").transform.position) {
            //curr_x = (int)player.transform.FindChild("Control_Point2").transform.position.x;
            //curr_y = (int)player.transform.FindChild("Control_Point2").transform.position.y;
           // curr_potential_value = BuildPotential.bitmap2[curr_x, curr_y];
            //Debug.Log(curr_potential_value2);

            //player.transform.Rotate(0, 0, 1);
            //player.transform.RotateAround(player.transform.FindChild("Control_Point1").transform.position, 1);
        }
	}

    bool CollitionDetect(int plus_x, int plus_y)
    {
        bool IsCollition = false;
        float temp_x = 0.0F;
        float temp_y = 0.0F;

        for (int i = 0; i < robot_edges_point.Count; i++) //機器人邊界上的所有點, 檢查有沒有跟障礙物的格子重疊
        {
            temp_x = robot_edges_point[i].x + (float)plus_x;
            temp_y = robot_edges_point[i].y + (float)plus_y;

            if(BuildPotential.bitmap[(int)temp_x, (int) temp_y] == 255) //碰到障礙物
            {
                IsCollition = true;
                break;
            }
        }


        if (IsCollition)
        {
            return false;
        }
        else //沒有碰撞的話, 將所有點更新
        {
            //Debug.Log(plus_x + " , " + plus_y);
            int point_n = robot_edges_point.Count;
            //Debug.Log(point_n);
            for (int i = 0; i < point_n; i++) //機器人邊界上的所有點, 都更新
            {
                temp_x = robot_edges_point[i].x + (float)plus_x;
                temp_y = robot_edges_point[i].y + (float)plus_y;
                BuildPotential.bitmap2[(int)temp_x, (int)temp_y] = 255;
                //Debug.Log(temp_x + " , " + temp_y);
                Vector2 tempV2 = new Vector2(temp_x, temp_y);
                robot_edges_point.Add(tempV2);
                //robot_edges_point.RemoveAt(0);
            }
            for (int i = 0; i < point_n; i++) //機器人邊界上的所有點, 都更新
                robot_edges_point.RemoveAt(0);

            return true;
        }
    }

    //利用老師PPT的方法
	void Findline(Vector2 p1, Vector2 p2)
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
			//Debug.Log(x + " , " + y);
			//Debug.Log((int)x + " , " + (int)y);
            
            Vector2 tempV2 = new Vector2(x, y);
			robot_edges_point.Add(tempV2);
            //BuildPotential.bitmap[(int)x, (int) y] = 255;
		}
	}

}
