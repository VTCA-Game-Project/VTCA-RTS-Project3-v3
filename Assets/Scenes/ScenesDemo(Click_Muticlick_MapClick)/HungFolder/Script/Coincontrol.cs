using Common.Building;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coincontrol : MonoBehaviour {

    public Refinery _refinery;
    public GameObject Coin;
    float x = 0;
	void Start () {
        Coin.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(_refinery.IsMax)
        {

            Coin.SetActive(true);
            x += 40 * Time.deltaTime;
            Coin.transform.localRotation =  Quaternion.Euler(0, x, 0);
        }
        else
        {
            x = 0;
            Coin.SetActive(false);
        }
	}
}
