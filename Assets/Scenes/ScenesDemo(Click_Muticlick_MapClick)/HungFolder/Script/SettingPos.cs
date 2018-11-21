using Common.Building;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPos : MonoBehaviour {

    public Material m1;
    public Material m2;

    public GameObject mytaget;
    public GameObject Mychild;
    MeshRenderer muren;

    Refinery recode;
	void Start ()
    {
       

        muren = Mychild.GetComponent<MeshRenderer>();
		if(mytaget.gameObject.layer==12)
        {
            muren.material = m1;
            this.gameObject.layer = 12;
        }
        if (mytaget.gameObject.layer == 13||mytaget.gameObject.layer==11)
        {
            muren.material = m2;
            this.gameObject.layer = 13;
        }


    }


	
	// Update is called once per frame
	
}
