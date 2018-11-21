using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour {

    private Material[] cellmaterial;
    private MeshRenderer render;
    [SerializeField] private GameObject CELL;
    [SerializeField] private GameObject orthercell;

	void Start () {
        render = GetComponent<MeshRenderer>();
        
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
        {
            orthercell.transform.position = hit.point;
        }
       
	}
    public void makecellboard(int wight,int height)
    {
       
        for (int i = 0; i < wight; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Instantiate(CELL, orthercell.transform.position + new Vector3(i * 2, 0, j * 2), Quaternion.identity, orthercell.transform);
            }
        }
    }
}
