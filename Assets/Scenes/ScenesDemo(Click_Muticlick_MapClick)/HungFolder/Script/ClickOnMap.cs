using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickOnMap : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    private Camera raycam;

    public GameObject effect;

    public GameObject Minimap;

    int MapWidth=100;
    int MapHeight=100;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
               
            }
            else
            {
                ClickMap(GetObjectPosition());
            }
        }
    }
    public void ClickMap(Vector3 taget)
    {
       
            var minieffect = Instantiate(effect, taget, Quaternion.identity);


            Destroy(minieffect, 0.3f);
        
    }
    private Vector3 GetObjectPosition()
    {
        Vector2 pos = Input.mousePosition;
        Ray ray = raycam.ScreenPointToRay(new Vector3(pos.x, pos.y, 0.0f));
        RaycastHit hit;
        Physics.Raycast(ray, out hit,Mathf.Infinity,LayerMask.GetMask("Place","Floor"));
        return hit.point;
    }


    public void MinimapClick()
    {
        var miniMapRect = Minimap.GetComponent<RectTransform>().rect;
       
        var screenRect = new Rect(
            Minimap.transform.position.x,
            Minimap.transform.position.y,
            miniMapRect.width, miniMapRect.height);

        var mousePos = Input.mousePosition;
        mousePos.y -= screenRect.y;
        mousePos.x -= screenRect.x;

        var camPos = new Vector3(
            mousePos.x * (MapWidth / screenRect.width), Camera.main.transform.position.y,
            mousePos.y * (MapHeight / screenRect.height)
           );
        Camera.main.transform.position = camPos;    
    }


}
