using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    
    public float panSpeed ;
    [HideInInspector]
    public float panBroader = -10f;

    public Vector2 panlimit;
	// Update is called once per frame
	void Update ()
    {
        Vector3 pos = transform.position;
        if (Input.mousePosition.y >= Screen.height - panBroader)
            pos.z += panSpeed * Time.deltaTime;
        if (Input.mousePosition.y <= panBroader)
            pos.z -= panSpeed * Time.deltaTime;
        if (Input.mousePosition.x >= Screen.width - panBroader)
            pos.x += panSpeed * Time.deltaTime;
        if (Input.mousePosition.x <= panBroader)
            pos.x -= panSpeed * Time.deltaTime;

      
        pos.x = Mathf.Clamp(pos.x, 10, 84);
        pos.z = Mathf.Clamp(pos.z, 3, 84);
        transform.position = pos;
	}
}
