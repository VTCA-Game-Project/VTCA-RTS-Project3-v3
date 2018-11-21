using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnMouseDow : MonoBehaviour {

    // Use this for initialization
    private Image thisimage;
    private bool selected = false;
    private bool OnCowndow = false;
    Color anpha ;
    void Start ()
    {
        thisimage = GetComponent<Image>();
        anpha = thisimage.color;
        anpha.a = 0f;
        thisimage.color = anpha;
       

    }
    private void Update()
    {
        if (anpha.a <= 0)
        {
            OnCowndow = false;
            anpha.a = 0;
        }
        if (selected==true&& OnCowndow==true)
        {
           
           
            thisimage.color = anpha;

            anpha.a -= 10 * Time.deltaTime;
        }
        if(selected==true&& OnCowndow==false)
        {

        }

       
    }
    void OnUserclick()
    {
        if(OnCowndow==false)
        selected =!selected ;

        if (selected == true)
        {
            OnCowndow = true;
            anpha.a = 180f;
        }


    }
	
	// Update is called once per frame
	
}
