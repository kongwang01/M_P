using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;


public class Reader : MonoBehaviour {


	// Use this for initialization
	void Start () {
		//Debug.Log(Application.dataPath);
		
		//======  存讀檔   ===================================================
		/*string path = Application.dataPath + "/obstacle.dat";
        if (!File.Exists(path)) return;
        StreamReader sr = File.OpenText(path);
        string input = ""; 
        //int i = 0;
        while (true) 
        {
            input = sr.ReadLine(); 
			//Debug.Log(input);

            if (input == null)
            {
                break;
            }
			if((input.Length > 0) && (input[0] != '#'))
				Debug.Log(input);
            //i = Int32.Parse( input );
        }
        sr.Close();*/
		
		//======================================================================
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
