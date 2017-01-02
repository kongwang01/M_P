using UnityEngine;
using System.Collections;

public class main_GUI : MonoBehaviour {
    bool FirstPotential = true;

	// Use this for initialization
	void Start () {
        DrawObstacle.DrawObstacles();
        DrawRobot.DrawRobots();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DrawReadMap()
    {
        //DrawObstacle.DrawObstacles();
        //DrawRobot.DrawRobots();
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
    }

    public void SearchPath()
    {
        //GameObject scr = GameObject.Find("SrarchPath");
        //BuildPotential.DrawPoten();
        this.gameObject.AddComponent<SrarchPath>();
    }
}
