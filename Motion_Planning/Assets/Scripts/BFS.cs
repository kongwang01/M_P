using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BFS : MonoBehaviour {
    public static Vector3[,,] BFS_tree = new Vector3[129, 129, 360]; //建立一個128*128*360大小的三維陣列，儲存(x, y,角度)是否有拜訪過
    bool success = false;
    List<Vector3> finded_path;
	Vector3 x_init;

    List<Vector2> robot_edges_point = new List<Vector2>(); //用來儲存robot邊上的所有點


    GameObject player;
    //Vector3 x_init = DrawRobot.robots[0].curr_configuration;
    //Vector3 goal_config = DrawRobot.robots[0].goal_curr_configuration;
   // List<Vector3> OPEN_list = new List<Vector3>(); //用來儲存OPEN


	// Use this for initialization
	void Start () {

        for (int i = 0; i < 129; i++)
            for (int j = 0; j < 129; j++)
                for (int k = 0; k < 360; k++)
                {
                    BFS_tree[i, j, k] = new Vector3(-1, -1, -1); //未拜訪過時, 值皆為-1; 若已被拜訪, 值為parent的configuration
                }

        //GameObject player = GameObject.Find("Robot");
        player = GameObject.Find("Robot");
        GameObject goal = GameObject.Find("Goal_of_Robot");

        DrawRobot.robots[0].curr_configuration.x = player.transform.position.x;
        DrawRobot.robots[0].curr_configuration.y = player.transform.position.y;
        DrawRobot.robots[0].curr_configuration.z = player.transform.rotation.eulerAngles.z;

        DrawRobot.robots[0].goal_curr_configuration.x = goal.transform.position.x;
        DrawRobot.robots[0].goal_curr_configuration.y = goal.transform.position.y;
        DrawRobot.robots[0].goal_curr_configuration.z = goal.transform.rotation.eulerAngles.z;


        for (int j = 0; j < DrawRobot.robots[0].n_of_polygons; j++) //第j個障礙物由幾個polygon組成
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

        BFS_Search();
        //OPEN_list.Add(x_init);

        //BFS_tree[(int)x_init.x, (int)x_init.y, (int)x_init.z] = new Vector3(0.0F, 0.0F, 0.0F); //parent為(0,0,0) ，代表為起始點
	}

    // Update is called once per frame
    /*void Update()
    {
        if ((OPEN_list.Count != 0) && (!success)) //OPEN_list中還有可拜訪的, 或是還沒成功到達終點, 持續迴圈
        {
            Vector3 search_p = OPEN_list[0]; //取最小的
            OPEN_list.RemoveAt(0);


            if (((int)search_p.y + 1 < 128) && (BFS_tree[(int)search_p.x, (int)search_p.y + 1, (int)search_p.z].x == -1)
                && CollitionDetect((int)x_init.x - (int)search_p.x, (int)x_init.y - (int)search_p.y + 1, (int)x_init.z - (int)search_p.z)) //檢查y+1合不合法
            {
                if ((int)search_p.x == 80)
                    Debug.Log("y+1 :" + (int)search_p.x + " , " + ((int)search_p.y + 1) + " , " + (int)search_p.z);
                BFS_tree[(int)search_p.x, (int)search_p.y + 1, (int)search_p.z] = search_p; //install x' in T & mark x' visited
                //temp_potential_value = Arbitration_U();

                OPEN_list.Add(new Vector3(search_p.x, search_p.y + 1.0F, search_p.z)); //insert x' in OPEN
                player.transform.position = new Vector3(search_p.x, search_p.y + 1.0F, 0.0F);

                if (((int)search_p.x == (int)goal_config.x) && (((int)search_p.y + 1) == (int)goal_config.y) && ((int)search_p.z == (int)goal_config.z)) //x' == x_goal
                {
                    success = true;
                }
            }

            if (((int)search_p.y - 1 > 0) && (BFS_tree[(int)search_p.x, (int)search_p.y - 1, (int)search_p.z].x == -1)
                && CollitionDetect((int)x_init.x - (int)search_p.x, (int)x_init.y - (int)search_p.y - 1, (int)x_init.z - (int)search_p.z)) //檢查y-1合不合法
            {
                if ((int)search_p.x == 80)
                Debug.Log((int)search_p.x + " , " + ((int)search_p.y - 1) + " , " + (int)search_p.z);
                BFS_tree[(int)search_p.x, (int)search_p.y - 1, (int)search_p.z] = search_p; //install x' in T & mark x' visited
                //temp_potential_value = Arbitration_U();

                OPEN_list.Add(new Vector3(search_p.x, search_p.y - 1.0F, search_p.z)); //insert x' in OPEN
                player.transform.position = new Vector3(search_p.x, search_p.y - 1.0F, 0.0F);

                if (((int)search_p.x == (int)goal_config.x) && (((int)search_p.y - 1) == (int)goal_config.y) && ((int)search_p.z == (int)goal_config.z)) //x' == x_goal
                {
                    success = true;
                }
            }

            if (((int)search_p.x + 1 < 128) && (BFS_tree[(int)search_p.x + 1, (int)search_p.y, (int)search_p.z].x == -1)
                && CollitionDetect((int)x_init.x - (int)search_p.x + 1, (int)x_init.y - (int)search_p.y, (int)x_init.z - (int)search_p.z)) //檢查x+1合不合法
            {
                if ((int)search_p.x + 1 == 80)
                Debug.Log(((int)search_p.x + 1) + " , " + (int)search_p.y + " , " + (int)search_p.z);
                BFS_tree[(int)search_p.x + 1, (int)search_p.y, (int)search_p.z] = search_p; //install x' in T & mark x' visited
                //temp_potential_value = Arbitration_U();

                OPEN_list.Add(new Vector3(search_p.x + 1.0F, search_p.y, search_p.z)); //insert x' in OPEN
                player.transform.position = new Vector3(search_p.x + 1.0F, search_p.y, 0.0F);

                if ((((int)search_p.x + 1) == (int)goal_config.x) && ((int)search_p.y == (int)goal_config.y) && ((int)search_p.z == (int)goal_config.z)) //x' == x_goal
                {
                    success = true;
                }
            }

            if (((int)search_p.x - 1 > 0) && (BFS_tree[(int)search_p.x - 1, (int)search_p.y, (int)search_p.z].x == -1)
                && CollitionDetect((int)x_init.x - (int)search_p.x - 1, (int)x_init.y - (int)search_p.y, (int)x_init.z - (int)search_p.z)) //檢查x-1合不合法
            {
                if ((int)search_p.x - 1 == 80)
                Debug.Log(((int)search_p.x - 1) + " , " + (int)search_p.y + " , " + (int)search_p.z);
                BFS_tree[(int)search_p.x - 1, (int)search_p.y, (int)search_p.z] = search_p; //install x' in T & mark x' visited
                //temp_potential_value = Arbitration_U();

                OPEN_list.Add(new Vector3(search_p.x - 1.0F, search_p.y, search_p.z)); //insert x' in OPEN
                player.transform.position = new Vector3(search_p.x - 1.0F, search_p.y, 0.0F);

                if ((((int)search_p.x - 1) == (int)goal_config.x) && ((int)search_p.y == (int)goal_config.y) && ((int)search_p.z == (int)goal_config.z)) //x' == x_goal
                {
                    success = true;
                }
            }

        }

    }*/

    void BFS_Search()
    {
        int count = 0;

        Vector3[] control_p = new Vector3[2];

		x_init = DrawRobot.robots[0].curr_configuration;
		Vector3 goal_config = DrawRobot.robots[0].goal_curr_configuration;

        //Debug.Log(x_init);
        //Debug.Log(goal_config);

        int temp_potential_value = 0;
		int smallest_index = 0;

        List<Vector3> OPEN_list = new List<Vector3>(); //用來儲存OPEN
		List<int> OPEN_list_potential_value = new List<int>(); //用來儲存OPEN中每個configuration對應的potential值

        OPEN_list.Add(x_init);
		temp_potential_value = Arbitration_U(x_init);
		OPEN_list_potential_value.Add (temp_potential_value);

        BFS_tree[(int)x_init.x, (int)x_init.y, (int)x_init.z] = new Vector3(0.0F, 0.0F, 0.0F); //parent為(0,0,0) ，代表為起始點

        while ((OPEN_list.Count != 0) && (!success)) //OPEN_list中還有可拜訪的, 或是還沒成功到達終點, 持續迴圈
        {
			smallest_index = 0;
			for (int i = 0; i < OPEN_list.Count; i++) { //找出potential最小的
				if (OPEN_list_potential_value [smallest_index] > OPEN_list_potential_value [i]) { 
					smallest_index = i;
				}
			}

			Vector3 search_p = OPEN_list[smallest_index]; //取最小的
			//Debug.Log(OPEN_list_potential_value [smallest_index]);
			//Debug.Log(search_p);
			OPEN_list.RemoveAt(smallest_index);
			OPEN_list_potential_value.RemoveAt(smallest_index);


            if (((int)search_p.y + 1 < 128) && (BFS_tree[(int)search_p.x, (int)search_p.y +1, (int)search_p.z].x == -1)
                && CollitionDetect((int)search_p.x - (int)x_init.x, (int)search_p.y - (int)x_init.y + 1, (int)search_p.z - (int)x_init.z)) //檢查y+1合不合法
            {
                //if ((int)search_p.x == 95)
                //    Debug.Log("y+1 :" + (int)search_p.x + " , " + ((int)search_p.y + 1) + " , " + (int)search_p.z);
                BFS_tree[(int)search_p.x, (int)search_p.y + 1, (int)search_p.z] = search_p; //install x' in T & mark x' visited
				temp_potential_value = Arbitration_U(new Vector3(search_p.x, search_p.y + 1.0F, search_p.z));

				OPEN_list.Add (new Vector3 (search_p.x, search_p.y + 1.0F, search_p.z)); //insert x' in OPEN
				OPEN_list_potential_value.Add(temp_potential_value);

				//for (int i = 0; i < OPEN_list.Count; i++) 
				//{
					//if (OPEN_list_potential_value [i] < temp_potential_value) // 讓
					//{
						//OPEN_list.Insert (i, new Vector3 (search_p.x, search_p.y + 1.0F, search_p.z)); //insert x' in OPEN
						//OPEN_list_potential_value.Insert(i, temp_potential_value);
						//break;
					//}
				//}

                if (((int)search_p.x == (int)goal_config.x) && (((int)search_p.y + 1) == (int)goal_config.y) && ((int)search_p.z == (int)goal_config.z)) //x' == x_goal
                {
                    //Debug.Log("y+1 :" + (int)search_p.x + " , " + ((int)search_p.y + 1) + " , " + (int)search_p.z);
                    success = true;
                }
            }

			 if (((int)search_p.y - 1 > 0) && (BFS_tree[(int)search_p.x, (int)search_p.y - 1, (int)search_p.z].x == -1)
                && CollitionDetect((int)search_p.x - (int)x_init.x, (int)search_p.y - (int)x_init.y - 1, (int)search_p.z - (int)x_init.z)) //檢查y-1合不合法
            {
               // if ((int)search_p.x == 95)
               //     Debug.Log("y+1 :" + (int)search_p.x + " , " + ((int)search_p.y - 1) + " , " + (int)search_p.z);
                BFS_tree[(int)search_p.x, (int)search_p.y - 1, (int)search_p.z] = search_p; //install x' in T & mark x' visited
				temp_potential_value = Arbitration_U(new Vector3(search_p.x, search_p.y - 1.0F, search_p.z));

				OPEN_list.Add (new Vector3 (search_p.x, search_p.y - 1.0F, search_p.z)); //insert x' in OPEN
				OPEN_list_potential_value.Add(temp_potential_value);

                if (((int)search_p.x == (int)goal_config.x) && (((int)search_p.y - 1) == (int)goal_config.y) && ((int)search_p.z == (int)goal_config.z)) //x' == x_goal
                {
                    //Debug.Log("y-1 :" + (int)search_p.x + " , " + ((int)search_p.y - 1) + " , " + (int)search_p.z);
                    success = true;
                }
            }

            if (((int)search_p.x + 1 < 128) && (BFS_tree[(int)search_p.x + 1, (int)search_p.y, (int)search_p.z].x == -1)
                && CollitionDetect((int)search_p.x - (int)x_init.x + 1, (int)search_p.y - (int)x_init.y, (int)search_p.z - (int)x_init.z)) //檢查x+1合不合法
            {
               // if ((int)search_p.x + 1 == 95)
               //     Debug.Log(((int)search_p.x + 1) + " , " + (int)search_p.y + " , " + (int)search_p.z);
                BFS_tree[(int)search_p.x + 1, (int)search_p.y, (int)search_p.z] = search_p; //install x' in T & mark x' visited
				temp_potential_value = Arbitration_U(new Vector3(search_p.x + 1.0F, search_p.y, search_p.z));

                OPEN_list.Add(new Vector3(search_p.x + 1.0F, search_p.y, search_p.z)); //insert x' in OPEN
				OPEN_list_potential_value.Add(temp_potential_value);

                if ((((int)search_p.x + 1) == (int)goal_config.x) && ((int)search_p.y == (int)goal_config.y) && ((int)search_p.z == (int)goal_config.z)) //x' == x_goal
                {
                    //Debug.Log("x+1 :" + ((int)search_p.x + 1) + " , " + (int)search_p.y + " , " + (int)search_p.z);
                    success = true;
                }
            }

            if (((int)search_p.x - 1 > 0) && (BFS_tree[(int)search_p.x - 1, (int)search_p.y, (int)search_p.z].x == -1)
                && CollitionDetect((int)search_p.x - (int)x_init.x - 1, (int)search_p.y - (int)x_init.y, (int)search_p.z - (int)x_init.z)) //檢查x-1合不合法
            {
               // if ((int)search_p.x - 1 == 95)
                //    Debug.Log(((int)search_p.x - 1) + " , " + (int)search_p.y + " , " + (int)search_p.z);
                BFS_tree[(int)search_p.x - 1, (int)search_p.y, (int)search_p.z] = search_p; //install x' in T & mark x' visited
				temp_potential_value = Arbitration_U(new Vector3(search_p.x - 1.0F, search_p.y, search_p.z));

				OPEN_list.Add(new Vector3(search_p.x - 1.0F, search_p.y, search_p.z)); //insert x' in OPEN
				OPEN_list_potential_value.Add(temp_potential_value);

                if ((((int)search_p.x - 1) == (int)goal_config.x) && ((int)search_p.y == (int)goal_config.y) && ((int)search_p.z == (int)goal_config.z)) //x' == x_goal
                {
                    //Debug.Log("x-1 :" + ((int)search_p.x - 1) + " , " + (int)search_p.y + " , " + (int)search_p.z);
                    success = true;
                }
            }



			if (((int)search_p.z + 1 < 360) && (BFS_tree[(int)search_p.x, (int)search_p.y, (int)search_p.z + 1].x == -1)
                && CollitionDetect((int)search_p.x - (int)x_init.x, (int)search_p.y - (int)x_init.y, (int)search_p.z - (int)x_init.z + 1)) //檢查angle + 1合不合法
            {
                //Debug.Log((int)search_p.x + " , " + ((int)search_p.y) + " , " + ((int)search_p.z + 1));
                BFS_tree[(int)search_p.x, (int)search_p.y, (int)search_p.z + 1] = search_p; //install x' in T & mark x' visited
				temp_potential_value = Arbitration_U(new Vector3(search_p.x, search_p.y, search_p.z + 1.0F));

                OPEN_list.Add(new Vector3(search_p.x, search_p.y, search_p.z + 1.0F)); //insert x' in OPEN
				OPEN_list_potential_value.Add(temp_potential_value);

                if (((int)search_p.x == (int)goal_config.x) && ((int)search_p.y == (int)goal_config.y) && (((int)search_p.z + 1) == (int)goal_config.z)) //x' == x_goal
                {
                    success = true;
                }
            }

            if (((int)search_p.z - 1 >= 0) && (BFS_tree[(int)search_p.x, (int)search_p.y, (int)search_p.z - 1].x == -1)
                && CollitionDetect((int)search_p.x - (int)x_init.x, (int)search_p.y - (int)x_init.y, (int)search_p.z - (int)x_init.z - 1)) //檢查angle - 1合不合法
            {
                //Debug.Log((int)search_p.x + " , " + ((int)search_p.y) + " , " + (int)search_p.z + 1);
                BFS_tree[(int)search_p.x, (int)search_p.y, (int)search_p.z - 1] = search_p; //install x' in T & mark x' visited
				temp_potential_value = Arbitration_U(new Vector3(search_p.x, search_p.y, search_p.z - 1.0F));

				OPEN_list.Add(new Vector3(search_p.x, search_p.y, search_p.z - 1.0F)); //insert x' in OPEN
				OPEN_list_potential_value.Add(temp_potential_value);

                if (((int)search_p.x == (int)goal_config.x) && ((int)search_p.y == (int)goal_config.y) && (((int)search_p.z - 1) == (int)goal_config.z)) //x' == x_goal
                {
                    success = true;
                }
            }

            /*
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
            }*/

            if (success)
                break;

            count++;
            if (count > 5000000) //防止無窮迴圈
                break;
        }
        Debug.Log(count);

        if (success)
        {
            Debug.Log("success!!!!!");
            finded_path = new List<Vector3>(); //用來儲存找到的path

            Vector3 tracing_path = goal_config;

            while (tracing_path.x != 0)
            {
                finded_path.Insert(0, tracing_path);
                Vector3 tempV3 = BFS_tree[(int)tracing_path.x, (int)tracing_path.y, (int)tracing_path.z];

                BuildPotential.bitmap[(int)tracing_path.x, (int)tracing_path.y] = 255;//Debug用

                tracing_path = tempV3;
    
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (success && main_GUI.showPathOrNot)
        {
            if(finded_path.Count != 0)
            {
                player.transform.position = new Vector3(finded_path[0].x, finded_path[0].y, 0.0F);
				//Debug.Log (finded_path [0].z);
                player.transform.rotation = Quaternion.Euler(0.0F, 0.0F, finded_path[0].z);
				//Debug.Log (player.transform.rotation.eulerAngles);
                finded_path.RemoveAt(0);
            }
        }
    }

	int Arbitration_U(Vector3 config)
    {
		Vector2 control_p1 = new Vector2 (0, 0);
		Vector2 control_p2 = new Vector2 (0, 0);

		double angle = config.z;
		if (angle > 180.0)
			angle -= 180.0;
		//Debug.Log (angle);
		float temp_x = DrawRobot.robots[0].control_points[0].x;
		float temp_y = DrawRobot.robots[0].control_points[0].y;
		//temp_x = temp_x + obstacles [i].curr_configuration.x; //未加上旋轉
		//temp_y = temp_y + obstacles [i].curr_configuration.y;
		float rotate_x = ((float)Math.Cos(angle * (Math.PI / 180.0))*temp_x) - ((float)Math.Sin(angle * (Math.PI / 180))*temp_y) + config.x; //cosX - sinY +dx
		float rotate_y = ((float)Math.Sin(angle * (Math.PI / 180.0))*temp_x) + ((float)Math.Cos(angle * (Math.PI / 180))*temp_y) + config.y; //sinX + cosY +dy
		//Debug.Log(k + " " +temp_x + " , " + temp_y);
		//Vector2 v2= new Vector2(rotate_x, rotate_y);

		if ((int)rotate_x > 127) //因為旋轉有可能超出範圍, 因此先判斷是否在1~127的範圍
			rotate_x = 127.0F;
		if ((int)rotate_x <= 0)
			rotate_x = 1.0F;
		if ((int)rotate_y > 127)
			rotate_y = 127.0F;
		if ((int)rotate_y <= 0)
			rotate_y = 1.0F;

		int potential_value = BuildPotential.bitmap[(int)rotate_x, (int)rotate_y];


		float temp_x2 = DrawRobot.robots[0].control_points[1].x;
		float temp_y2 = DrawRobot.robots[0].control_points[1].y;
		//temp_x = temp_x + obstacles [i].curr_configuration.x; //未加上旋轉
		//temp_y = temp_y + obstacles [i].curr_configuration.y;
		float rotate_x2 = ((float)Math.Cos(angle * (Math.PI / 180.0))*temp_x2) - ((float)Math.Sin(angle * (Math.PI / 180))*temp_y2) + config.x; //cosX - sinY +dx
		float rotate_y2 = ((float)Math.Sin(angle * (Math.PI / 180.0))*temp_x2) + ((float)Math.Cos(angle * (Math.PI / 180))*temp_y2) + config.y; //sinX + cosY +dy
		//Debug.Log(k + " " +temp_x + " , " + temp_y);
		//Vector2 v2= new Vector2(rotate_x, rotate_y);

		if ((int)rotate_x2 > 127) //因為旋轉有可能超出範圍, 因此先判斷是否在1~127的範圍
			rotate_x2 = 127.0F;
		if ((int)rotate_x2 <= 0)
			rotate_x2 = 1.0F;
		if ((int)rotate_y2 > 127)
			rotate_y2 = 127.0F;
		if ((int)rotate_y2 <= 0)
			rotate_y2 = 1.0F;

		int potential_value2 = BuildPotential.bitmap2[(int)rotate_x2, (int)rotate_y2];

		//control_p1.x = player.transform.FindChild("Control_Point1").transform.position.x;

		//int potential_value = BuildPotential.bitmap[curr_x, curr_y];

		//int curr_x2 = (int)player.transform.FindChild("Control_Point2").transform.position.x;
		//int curr_y2 = (int)player.transform.FindChild("Control_Point2").transform.position.y;
		//int potential_value2 = BuildPotential.bitmap2[curr_x2, curr_y2];

		return (potential_value + potential_value2);
    }


    bool CollitionDetect(int plus_x, int plus_y, int plus_angle)
    {
        bool IsCollition = false;
        float temp_x = 0.0F;
        float temp_y = 0.0F;
        //Debug.Log(plus_x + " " + plus_y + " , " + plus_angle);
        for (int i = 0; i < robot_edges_point.Count; i++) //機器人邊界上的所有點, 檢查有沒有跟障礙物的格子重疊
        {
            //temp_x = robot_edges_point[i].x + (float)plus_x;
            //temp_y = robot_edges_point[i].y + (float)plus_y;

            //===  加上旋轉  ===
            double angle = (double)plus_angle;
			temp_x = robot_edges_point[i].x - x_init.x;
			temp_y = robot_edges_point[i].y - x_init.y;

            float rotate_x = ((float)Math.Cos(angle * (Math.PI / 180.0)) * temp_x) - ((float)Math.Sin(angle * (Math.PI / 180)) * temp_y) + (float)plus_x; //cosX - sinY +dx
            float rotate_y = ((float)Math.Sin(angle * (Math.PI / 180.0)) * temp_x) + ((float)Math.Cos(angle * (Math.PI / 180)) * temp_y) + (float)plus_y; //sinX + cosY +dy
			//Debug.Log(" " +rotate_x + " , " + rotate_y);

			temp_x = rotate_x + x_init.x;
			temp_y = rotate_y + x_init.y;
            //==================

            if (((int)temp_x < 129) && ((int)temp_x >= 0) && ((int)temp_y < 129) && ((int)temp_y >= 0)) //因為旋轉有可能超出範圍, 因此先判斷是否在0~128的範圍
                if (BuildPotential.bitmap[(int)temp_x, (int)temp_y] == 255) //碰到障礙物
                {
                    //Debug.Log("Collition " + temp_x + " , " + temp_y);
                    IsCollition = true;
                    break;
                }
        }


        if (IsCollition)
        {
            return false;
        }
        else //沒有碰撞
        {
            //Debug.Log(plus_x + " , " + plus_y);
            int point_n = robot_edges_point.Count;
            //Debug.Log(point_n);
            for (int i = 0; i < point_n; i++) //機器人邊界上的所有點, 都更新
            {
                //temp_x = robot_edges_point[i].x + (float)plus_x;
                //temp_y = robot_edges_point[i].y + (float)plus_y;

                //===  加上旋轉  ===
                double angle = (double)plus_angle;
				temp_x = robot_edges_point[i].x - x_init.x;
				temp_y = robot_edges_point[i].y - x_init.y;

                float rotate_x = ((float)Math.Cos(angle * (Math.PI / 180.0)) * temp_x) - ((float)Math.Sin(angle * (Math.PI / 180)) * temp_y) + (float)plus_x; //cosX - sinY +dx
                float rotate_y = ((float)Math.Sin(angle * (Math.PI / 180.0)) * temp_x) + ((float)Math.Cos(angle * (Math.PI / 180)) * temp_y) + (float)plus_y; //sinX + cosY +dy
                //Debug.Log(k + " " +temp_x + " , " + temp_y);

				temp_x = rotate_x + x_init.x;
				temp_y = rotate_y + x_init.y;
                //==================

				if (((int)temp_x < 129) && ((int)temp_x >= 0) && ((int)temp_y < 129) && ((int)temp_y >= 0)) //因為旋轉有可能超出範圍, 因此先判斷是否在0~128的範圍
					BuildPotential.SearchBitmap[(int)temp_x, (int)temp_y] = 250;
                //Debug.Log(temp_x + " , " + temp_y);
                //Vector2 tempV2 = new Vector2(temp_x, temp_y);
                //robot_edges_point.Add(tempV2);
                //robot_edges_point.RemoveAt(0);
            }
            //for (int i = 0; i < point_n; i++) //機器人邊界上的所有點, 都更新
                //robot_edges_point.RemoveAt(0);

            return true;
        }
    }

    //利用老師PPT的方法
    void Findline(Vector2 p1, Vector2 p2)
    {
        int d = 0;
        float dx = 0.0F, dy = 0.0F;

        if (Mathf.Abs(p2.x - p1.x) > Mathf.Abs(p2.y - p1.y))
            d = (int)Mathf.Abs(p2.x - p1.x);
        else
            d = (int)Mathf.Abs(p2.y - p1.y);

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
