using UnityEngine;
using System.Collections;

public class SrarchPath : MonoBehaviour {
	public GameObject player;
	public GameObject goal;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (BuildPotential.bitmap_ok) {
			player = GameObject.Find ("Robot");
			goal = GameObject.Find ("Goal_of_Robot");

			//Debug.Log( goal.transform.position);
			//Debug.Log( goal.transform.FindChild("Control_Point1").transform.position);
			//Debug.Log( goal.transform.FindChild("Control_Point2").transform.position);

			//while (goal.transform.FindChild("Control_Point1").transform.position != player.transform.FindChild("Control_Point1").transform.position) {
				int curr_x = (int)player.transform.FindChild ("Control_Point1").transform.position.x;
				int curr_y = (int)player.transform.FindChild ("Control_Point1").transform.position.y;
				int curr_potential_value = BuildPotential.bitmap [curr_x, curr_y];
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
	}


}
