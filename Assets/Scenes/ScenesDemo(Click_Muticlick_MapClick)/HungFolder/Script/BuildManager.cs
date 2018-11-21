using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour {

    // Use this for initialization

    private List<GameObject> BuildList = new List<GameObject>();
    public List<GameObject> ButtonList;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
       
      

	}

    private bool setToBuild(string buildName)
    {
        int count = 0;
        if (BuildList.Count < 0)
            return false;
        foreach (GameObject GO in BuildList)
        {
            if (GO.name == buildName)
                count++;


        }
        if (count < 0)
            return false;

        return true;
    }
    public  void addToBuildManager(GameObject newGO)
        {
        BuildList.Add(newGO);
        }

    public void RemoveToBuildManager(GameObject newGO)
    {
        if(BuildList.Contains(newGO))
        {
            BuildList.RemoveAt(BuildList.IndexOf(newGO));
        }

    }




}
