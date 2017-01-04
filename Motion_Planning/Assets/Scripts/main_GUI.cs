using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class main_GUI : MonoBehaviour {
	public Text MessageText;
    public static Text ProcessText;
	bool drawMapOrNot = false;
    bool FirstPotential = true;
    bool FirstBFS = true;
    public static bool showPathOrNot = false;

    public float User_angle_diff = 10.0F;
    public static float For_BFS_angle_diff;  //BFS的角度要分多少格

	// Use this for initialization
	void Start () {
        //DrawObstacle.DrawObstacles();
        //DrawRobot.DrawRobots();
        ProcessText = GameObject.Find("P_Text").GetComponent<Text>();
        For_BFS_angle_diff = User_angle_diff;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeData(int index)
	{
		if (index == 0) {
			DrawRobot.robot_path = Application.dataPath + "/Resources/robot.dat";
			DrawObstacle.obstacle_path = Application.dataPath + "/Resources/obstacle.dat";
		}
		else if (index == 1) {
			DrawRobot.robot_path = Application.dataPath + "/Resources/robot02.dat";
			DrawObstacle.obstacle_path = Application.dataPath + "/Resources/map02.dat";
		}
		else if (index == 2) {
			DrawRobot.robot_path = Application.dataPath + "/Resources/robot03.dat";
			DrawObstacle.obstacle_path = Application.dataPath + "/Resources/map03.dat";
		}
		else if (index == 3) {
			DrawRobot.robot_path = Application.dataPath + "/Resources/robot04.dat";
			DrawObstacle.obstacle_path = Application.dataPath + "/Resources/map04.dat";
		}
		else if (index == 4) {
			DrawRobot.robot_path = Application.dataPath + "/Resources/robot05.dat";
			DrawObstacle.obstacle_path = Application.dataPath + "/Resources/map05.dat";
		}
		else if (index == 5) {
			DrawRobot.robot_path = Application.dataPath + "/Resources/robot06.dat";
			DrawObstacle.obstacle_path = Application.dataPath + "/Resources/map06.dat";
		}
	}

    public void DrawReadMap()
    {
		if (!drawMapOrNot) //第一次畫時直接畫
		{
			MessageText.text = "";
			DrawObstacle.DrawObstacles ();
			DrawRobot.DrawRobots ();
			drawMapOrNot = true;
		}
		else //第二次以後, 先刪掉原本儲存的資料再畫
		{
			DrawObstacle.obstacles.Clear();
			DrawRobot.robots.Clear();

			GameObject[] old_obj;
			old_obj = GameObject.FindGameObjectsWithTag ("Motion_Planning");

			for (int i = 0; i < old_obj.Length; i++)
				Destroy (old_obj [i]);

			DrawObstacle.DrawObstacles ();
			DrawRobot.DrawRobots ();
		}
        ProcessText.text = "DrawPotentialField -> BFS -> ShowPath";
    }

    public void DrawPotential()
    {
        //BuildPotential.DrawPoten();
        if (FirstPotential)
        {
            this.gameObject.AddComponent<BuildPotential>();
            FirstPotential = false;
        }
        else
        {
            Destroy(this.gameObject.GetComponent<BuildPotential>());
            this.gameObject.AddComponent<BuildPotential>();
        }
        ProcessText.text = "DrawPotentialField -> BFS -> ShowPath";
    }

	public void Angle_Changed(string str)
	{
		int temp_i = int.Parse (str);
		if ((temp_i >= 1) && (temp_i <= 30))
			User_angle_diff = (float)temp_i;
	}

    public void SearchPath()
    {
        //GameObject scr = GameObject.Find("SrarchPath");
        //BuildPotential.DrawPoten();
        this.gameObject.AddComponent<SrarchPath>();
    }

    public void BFSearch()
    {
        ProcessText.text = "Path Searching...";
        //GameObject scr = GameObject.Find("SrarchPath");
        //BuildPotential.DrawPoten();
        //this.gameObject.AddComponent<BFS>();
		if (!FirstPotential) //確定有畫完potential才能做BFS
		{
            For_BFS_angle_diff = User_angle_diff;
            ProcessText.text = "Path Searching...";
			if (FirstBFS)
			{
				this.gameObject.AddComponent<BFS> ();
				FirstBFS = false;
			} 
			else
			{
				Destroy (this.gameObject.GetComponent<BFS> ());
				this.gameObject.AddComponent<BFS> ();
			}
		}
    }

    public void ShowPath()
    {
		if(showPathOrNot)
        	showPathOrNot = false;
		else
			showPathOrNot = true;
    }
}
