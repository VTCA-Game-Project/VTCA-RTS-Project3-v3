using Common.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour {

    // Use this for initialization

    public Image imageHealthValue;

    public GameObject hpbot;
   
  
    private float counter;
    public AIAgent agent;

    void Start()
    {
        hpbot.SetActive(false);

    }
	
	// Update is called once per frame
	void Update ()

    {
        transform.eulerAngles = Camera.main.transform.eulerAngles;
        if (agent != null)
        {
            hpbot.SetActive(agent.IsSelected);
        }
        else
        {
            hpbot.SetActive(true);
        }
         }

    public void SetValue(float value)
    {
        imageHealthValue.fillAmount = value;

    }
}
