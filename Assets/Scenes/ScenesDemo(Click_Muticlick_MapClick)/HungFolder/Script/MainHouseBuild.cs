using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHouseBuild : MonoBehaviour {

     Vector2 BuildSize;
    public GameObject originGO;
    BuildControl build;
    private void Start()
    {
        build = originGO.GetComponent<BuildControl>();
        transform.position = build.getOrigin().position;
        transform.localScale=new Vector3(3, 3, 3);
    }


}
