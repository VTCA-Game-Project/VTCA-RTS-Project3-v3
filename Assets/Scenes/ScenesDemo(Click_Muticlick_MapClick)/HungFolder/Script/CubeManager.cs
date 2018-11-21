using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour {
    enum CudeState {None,CanBuild,UnBuild,Wasbuild }
    [SerializeField]
    private Material m1;
    [SerializeField]
    private Material m2;
    [SerializeField]
    private Material m3;
    [SerializeField]
    private Material m4;
    private MeshRenderer CubeSkin;
    [HideInInspector]
    public Vector2 CodeLocal = new Vector2();
    [HideInInspector]
    public bool CanBuild = false;
    CudeState State = new CudeState();

    private void Awake()
    {
        CanBuild = false ;
    }
    void Start ()
    {

        State = CudeState.None;
        CubeSkin = GetComponent<MeshRenderer>();
       

    }

    public void OnraycastIn()
    {
        if (State == CudeState.CanBuild)
            CubeSkin.material = m1;
        if (State == CudeState.UnBuild)
            CubeSkin.material = m2;
        if(State == CudeState.None)
            CubeSkin.material = m3;
        if (State == CudeState.Wasbuild)
            CubeSkin.material = m4;

    }

    public string GetState()
    {
        return this.State.ToString();
    }

    public void SetState(string _nextstate)
    {


        switch(_nextstate)
        {
            case "None":
                this.State = CudeState.None;
                break;
            case "Can":
                this.State = CudeState.CanBuild;
                break;
            case "Not":
                this.State = CudeState.UnBuild;
                break;
            case "Was":
                this.State = CudeState.Wasbuild;
                break;
        }
        

    }

    public Vector2 GetLocalCube()
    {

        return this.CodeLocal;
    }

	
	


}
