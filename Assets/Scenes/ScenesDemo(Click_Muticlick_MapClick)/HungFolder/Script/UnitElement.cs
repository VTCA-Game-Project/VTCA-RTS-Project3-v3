using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitElement : MonoBehaviour
{

   
    BuildElement Build;
    DefElement Def;
    SoilderElement Soild;
    TankElement Tank;
    private Image CurrentImage;
    public void Start()
    {

        CurrentImage = GetComponent<Image>();
        Build = GetComponent<BuildElement>();
      
        Def = GetComponent<DefElement>();
        Soild = GetComponent<SoilderElement>();
        Tank = GetComponent<TankElement>();


    }
    public void InBuldUnitClick(int i)
    {
        if (this.gameObject.name == "UnitBuild" + i )
        {
            string inputvalues = "";
            if (Input.GetMouseButtonDown(0))
                inputvalues = "LEFT";
            if (Input.GetMouseButtonDown(1))
                inputvalues = "RIGHT";
            Build.OnUnitClick(inputvalues);
            inputvalues = "";

        }
    }
    public void InSoilderUnitClick(int i)
    {

       
        if (this.gameObject.name == "UnitSoilder" + i )
        {

            string inputvalues = "";
            if (Input.GetMouseButtonDown(0))
                inputvalues = "LEFT";
            if (Input.GetMouseButtonDown(1))
                inputvalues = "RIGHT";
            Soild.OnUnitClick(inputvalues);
            inputvalues = "";

        }




    }
   
    private void Update()
    {
        //ActiveComponent();
    }
}
