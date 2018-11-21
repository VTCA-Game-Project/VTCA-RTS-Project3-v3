using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankElement : MonoBehaviour {

    // Use this for initialization
    [HideInInspector]
    public Image CurrentImage;
    public Sprite NewImage;
    void Start()
    {
        CurrentImage = GetComponent<Image>();

        CurrentImage.sprite = NewImage;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
