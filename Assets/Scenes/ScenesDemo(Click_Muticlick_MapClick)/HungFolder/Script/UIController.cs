using Common.Building;
using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    // Use this for initialization

   
    public ButtonDown build;
    public GameObject solidetbutton;
	void Start ()

    {
        for (int i = 1; i < 8; i++)
        {
            build.UnitBuildList[i].SetActive(false);

        }

        build.UnitManagerList[1].SetActive(false);
        solidetbutton.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void GetACtiveBuild(Player _tempPlayer)
    {
      
            build.UnitBuildList[1].SetActive(_tempPlayer.IsCanBuild(EnumCollection.ConstructId.Barrack));
            build.UnitBuildList[2].SetActive(_tempPlayer.IsCanBuild(EnumCollection.ConstructId.Refinery));
            build.UnitBuildList[4].SetActive(_tempPlayer.IsCanBuild(EnumCollection.ConstructId.Defender));

        bool res = false;
        for (int i = 0; i<_tempPlayer.Constructs.Count;i++)
        {
            if(_tempPlayer.Constructs[i] is Barrack )
            {
                UnlockUI(true);
                res = true;
            }
        }
        if(res==false)
        {
            UnlockUI(false);
        }
    }

    public void UnlockUI(bool values)
    {

        build.UnitManagerList[1].SetActive(values);
        solidetbutton.SetActive(values);
    }
}
