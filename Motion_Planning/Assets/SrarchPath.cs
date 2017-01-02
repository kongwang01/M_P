using UnityEngine;
using System.Collections;

public class SrarchPath : MonoBehaviour {
	public GameObject player;
	public GameObject goal;

    int curr_x = 0;
	int curr_y = 0;
    int curr_potential_value = 0;

    int curr_x2 = 0;
    int curr_y2 = 0;
    int curr_potential_value2 = 0;

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
            Debug.Log(curr_potential_value2);

            //player.transform.Rotate(0, 0, 1);
            //player.transform.RotateAround(player.transform.FindChild("Control_Point1").transform.position, 1);
        }
	}


}
