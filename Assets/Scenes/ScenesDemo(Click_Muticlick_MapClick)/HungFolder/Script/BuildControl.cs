using Common;
using Common.Building;
using DelegateCollection;
using EnumCollection;
using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildControl : MonoBehaviour
{

    public Camera rtsCamera;
    private Vector2 LatPoint;
    private RaycastHit hitInfo;
    private Ray ray;
    private List<CubeManager> ListInSelect = new List<CubeManager>();

    private List<CubeManager> ListBuildCube = new List<CubeManager>();

    public UIController uiControl;

    public bool OnselectTaget = false;

    private bool OnholdTaget = false;
    [HideInInspector]
    public GameObject BuildModel;
    [HideInInspector]
    public ConstructId BuildModelContructID;
    [HideInInspector]
    public Vector2 BuildSize = new Vector2();
    public GameObject Map;
    private MapControl ControlMap;
    private List<int> BuildCount;
    private List<CubeManager> ListCubeWasUse;
    private List<Vector2> ListBuildSize;
    public int BuildPrice;
    void Start()
    {
        Player _player = FindObjectOfType<MainPlayer>();
        _player.DestroyConstructUI = uiControl.GetACtiveBuild;
        if (Map != null)
        {
            ControlMap = Map.GetComponent<MapControl>();
        }
        BuildCount = new List<int>();
        ListCubeWasUse = new List<CubeManager>();
        ListBuildSize = new List<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {


        if (OnselectTaget == true)
        {
            foreach (CubeManager cube in ListInSelect)
            {

                if (cube != null)
                {
                    if (cube.CanBuild == false)
                    {


                        cube.SetState("Not");
                        cube.OnraycastIn();

                    }

                    else
                    {
                        cube.SetState("Can");
                        cube.OnraycastIn();


                        foreach (CubeManager _cube in ListBuildCube)
                        {
                            _cube.SetState("None");
                            _cube.OnraycastIn();
                        }

                    }
                }

            }

            ray = rtsCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray: ray,
                                hitInfo: out hitInfo,
                                maxDistance: Mathf.Infinity,
                                layerMask: LayerMask.GetMask("Floor")))
            {

                CubeManager cube = hitInfo.collider.gameObject.GetComponent<CubeManager>();


                if (cube != null)
                {

                    setBuildSize(BuildSize, cube);



                }


            }


            if (OnholdTaget)
            {
                if (Input.GetMouseButtonDown(1))
                { 
                    for (int k = 0; k < ListInSelect.Count; k++)
                    {
                        ListInSelect[k].SetState("None");
                        ListInSelect[k].OnraycastIn();
                    }
                    Player _player = FindObjectOfType<MainPlayer>();
                    _player.TakeGold(BuildPrice);
                    ResetTaget();
                    ListInSelect.Clear();
                    return;
                }
                if (Input.GetMouseButtonDown(0) && select() == false)
                {
                    return;
                }
                else
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (ListInSelect.Count < BuildSize.x * BuildSize.y)
                        { return; }
                        for (int k = 0; k < ListInSelect.Count; k++)
                        {

                            ListInSelect[k].CanBuild = false;
                            ListInSelect[k].SetState("None");
                            ListInSelect[k].OnraycastIn();
                            ListBuildCube.Add(ListInSelect[k]);
                        }

                      
                        GameObject NewGO = Instantiate(BuildModel, new Vector3(LatPoint.x+((int)BuildSize.x/2), 0, LatPoint.y+((int)BuildSize.y/2)), Quaternion.identity);
                        Construct construct = NewGO.GetComponentInChildren<Construct>();
                        construct.Group = EnumCollection.Group.Player;
                        SoundManager.instanece.PlayEffect(2);
                        NewGO.SetActive(true);
                        construct.Build();                                          
                        ListInSelect.Clear();
                        ResetTaget();
                        uiControl.GetACtiveBuild(construct.Player);
                       
                    }
                }



            }

            if (Input.GetMouseButtonUp(0))
            {
                OnholdTaget = true;
            }



        }
    }
    private void setBuildSize(Vector2 size, CubeManager currentpoint)
    {

        if (LatPoint != currentpoint.CodeLocal)
        {
            foreach (CubeManager minicube in ListInSelect)
            {
                minicube.SetState("None");
                minicube.OnraycastIn();
            }
            ListInSelect.Clear();

            if (!CheckBuildedCude(currentpoint))
            {
                LatPoint = currentpoint.CodeLocal;

            }

            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {

                    CubeManager sub = ControlMap.GetCubeBylocal(currentpoint.CodeLocal + new Vector2(i, j));
                    if (sub != null)

                    {
                        ListInSelect.Add(sub);

                    }



                }
            }

        }


    }



    private bool select()
    {
        for (int i = 0; i < ListInSelect.Count; i++)
        {
            if (ListInSelect[i].CanBuild == false)
            {

                return false;
            }
        }
        return true;
    }


    private bool CheckBuildedCude(CubeManager _cube)
    {

        return ListBuildCube.Contains(_cube);
    }

    public Transform getOrigin()
    {
        int taget = ListInSelect.Count / 2;
        return ListInSelect[taget].transform;
    }

    public void letOnDestroy(Vector3 taget,Vector2 buildsize)
    {
        Vector2 deadtaget = new Vector2(taget.x, taget.z);
        
        for (int i = 0; i < buildsize.x; i++)
        {
            for (int j = 0; j < buildsize.y; j++)
            {

                CubeManager sub = ControlMap.GetCubeBylocal(deadtaget + new Vector2(i, j));
                Debug.Log(sub.CodeLocal);
                if (sub != null)

                {
                  foreach( CubeManager cube in ListBuildCube)
                    {

                        if(cube.CodeLocal==sub.CodeLocal)
                        {
                            sub.CanBuild = true;
                            ListBuildCube.Remove(cube);
                        }
                    }

                }



            }
        }
    }

    public void ResetTaget()
    {
        OnselectTaget = false;
        OnholdTaget = false;
        BuildSize = new Vector2();
        BuildModel = null;
        BuildModelContructID = ConstructId.None;

    }
}
