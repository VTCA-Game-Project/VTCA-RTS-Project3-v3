using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Click : MonoBehaviour
{
    public static Click Instance;
    public LayerMask Clicklayer;
    public Camera cameraRaycaster;

    private Vector3 startLeftMouse;
    private Vector3 endLeftMouse;
    private RaycastHit hitInfo;

    private List<ClickOn> selectedObjects;
    private List<ClickOn> selectableObjects;


    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != null) Destroy(Instance.gameObject);

        selectedObjects         = new List<ClickOn>();
        selectableObjects       = new List<ClickOn>();

    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RighMouseDown();
        }


       
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            LeftMouseDown();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            LeftMouseUp();
        }
    }
    void SelectObs()
    {
        Rect selectrect = new Rect((Vector2)startLeftMouse,(Vector2)(endLeftMouse - startLeftMouse));
        foreach (ClickOn selectobj in selectableObjects)
        {
            if (selectobj != null)
            {
                if (selectobj.PlayerGroup == EnumCollection.Group.Player)
                {
                    if (selectrect.Contains(cameraRaycaster.WorldToViewportPoint(selectobj.transform.position), true))
                    {
                        selectedObjects.Add(selectobj);
                        selectobj.Select();
                    }
                }
            }
        }
    }
    void ClearSelection()
    {
        int count = selectableObjects.Count;
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                selectableObjects[i].UnSelect();
            }
            selectedObjects.Clear();
        }
    }

    public void Add(ClickOn obj)
    {
        if (selectableObjects.Contains(obj)) return;
        selectableObjects.Add(obj);
       
    }

    // mouse events
    private void LeftMouseDown()
    {
        startLeftMouse = cameraRaycaster.ScreenToViewportPoint(Input.mousePosition);
    }
    private void LeftMouseUp()
    {
        Vector3 mousePosition = Input.mousePosition;
        endLeftMouse = cameraRaycaster.ScreenToViewportPoint(mousePosition);

        if ((startLeftMouse - endLeftMouse).sqrMagnitude > 0.005f)
        {
            ClearSelection();
            SelectObs();
        }
        else
        {
            bool hitted = Physics.Raycast
                   (ray: cameraRaycaster.ScreenPointToRay(mousePosition),
                    hitInfo: out hitInfo,
                    maxDistance: float.MaxValue,
                    layerMask: Clicklayer);
            if (hitted)
            {
                ClickOn obj = hitInfo.collider.GetComponent<ClickOn>();
                if(obj != null)
                {
                    ClearSelection();
                    obj.Select();
                    selectedObjects.Add(obj);
                }
            }
            else
            {
                Pointer.Instance.PutPointer();
                SelectedActionCallback();
            }
        }

        ResetMousePosition();
    }

    private void RighMouseDown()
    {
        ClearSelection();
    }

    private void SelectedActionCallback()
    {
        int count = selectedObjects.Count;
        for (int i = 0; i < count; i++)
        {
            selectedObjects[i].Action();
        }
    }

    private void ResetMousePosition()
    {
        startLeftMouse = Vector3.zero;
        endLeftMouse = Vector3.zero;
    }
}

