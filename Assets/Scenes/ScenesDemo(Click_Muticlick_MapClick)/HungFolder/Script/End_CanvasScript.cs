using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End_CanvasScript : MonoBehaviour {

    public string Command;
    public GameObject WinOB;
    public GameObject LostOB;
	
	
	// Update is called once per frame
	void Update ()

    {

        if (Command == "Win")
        {
            WinOB.SetActive(true);
        }
        if (Command == "Lost")
        {
            LostOB.SetActive(true);
        }
    }



}
