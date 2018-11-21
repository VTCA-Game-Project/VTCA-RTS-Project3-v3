using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Pattern;

public class ButtonDown : MonoBehaviour, IPointerDownHandler
{
     string GameClass=Singleton.classname;
    public GameObject[] ButtonList;
    public GameObject[] UnitBuildList;

    public GameObject[] UnitSoilderList;
    
    public GameObject[] UnitManagerList;
    private ButtonElement element;
    private UnitElement UnitElement;
    delegate void tranfrom();


   
    private void Awake()
    {

        if(GameClass=="Human")
        {
            UnitSoilderList[2].SetActive(false);
        }
        if(GameClass == "Orc")
        {
            UnitSoilderList[1].SetActive(false);
        }
    }
    
   
    public void OnPointerDown(PointerEventData eventData)
    {
        string even = eventData.pointerEnter.name.ToString();
        switch (even)
        {
            case "BuildButton":
                element = ButtonList[0].GetComponent<ButtonElement>();
                element.Showon("BuildButton");
                break;

            case "SoilderButton":
                element = ButtonList[1].GetComponent<ButtonElement>();
                element.Showon("SoilderButton");
                break;
         
            case "UnitBuild1":
                UnitElement = UnitBuildList[0].GetComponent<UnitElement>();
                UnitElement.InBuldUnitClick(1);
                break;
            case "UnitBuild2":
                UnitElement = UnitBuildList[1].GetComponent<UnitElement>();
                UnitElement.InBuldUnitClick(2);
                break;
            case "UnitBuild3":
                UnitElement = UnitBuildList[2].GetComponent<UnitElement>();
                UnitElement.InBuldUnitClick(3);
                break;
            case "UnitBuild4":
                UnitElement = UnitBuildList[3].GetComponent<UnitElement>();
                UnitElement.InBuldUnitClick(4);
                break;
            case "UnitBuild5":
                UnitElement = UnitBuildList[4].GetComponent<UnitElement>();
                UnitElement.InBuldUnitClick(5);
                break;
            case "UnitBuild6":
                UnitElement = UnitBuildList[5].GetComponent<UnitElement>();
                UnitElement.InBuldUnitClick(6);
                break;
            case "UnitBuild7":
                UnitElement = UnitBuildList[6].GetComponent<UnitElement>();
                UnitElement.InBuldUnitClick(7);
                break;
            case "UnitBuild8":
                UnitElement = UnitBuildList[7].GetComponent<UnitElement>();
                UnitElement.InBuldUnitClick(8);
                break;



            case "UnitSoilder1":
                UnitElement = UnitSoilderList[0].GetComponent<UnitElement>();
                UnitElement.InSoilderUnitClick(1);
                break;
            case "UnitSoilder2":
                UnitElement = UnitSoilderList[1].GetComponent<UnitElement>();
                UnitElement.InSoilderUnitClick(2);
                break;
            case "UnitSoilder3":
                UnitElement = UnitSoilderList[2].GetComponent<UnitElement>();
                UnitElement.InSoilderUnitClick(3);
                break;
            case "UnitSoilder4":
                UnitElement = UnitSoilderList[3].GetComponent<UnitElement>();
                UnitElement.InSoilderUnitClick(4);
                break;
            case "UnitSoilder5":
                UnitElement = UnitSoilderList[4].GetComponent<UnitElement>();
                UnitElement.InSoilderUnitClick(5);
                break;
            case "UnitSoilder6":
                UnitElement = UnitSoilderList[5].GetComponent<UnitElement>();
                UnitElement.InSoilderUnitClick(6);
                break;
            case "UnitSoilder7":
                UnitElement = UnitSoilderList[6].GetComponent<UnitElement>();
                UnitElement.InSoilderUnitClick(7);
                break;
            case "UnitSoilder8":
                UnitElement = UnitSoilderList[7].GetComponent<UnitElement>();
                UnitElement.InSoilderUnitClick(8);
                break;


        }
       
    }
    

    // Use this for initialization
    void Start()
    {
        DisableUnitBar();
    }

    // Update is called once per frame
    void Update()
    {

        ClearUnactiveButton();
    }
    public void ChangeStatusButton(ButtonElement _but)
    {

                switch(_but.Buttonname)
                {
                    case "BuildButton":
                        EnableUnitBar("UnitBuyBuild");
                      
                        break;

                    case "SoilderButton":
                        EnableUnitBar("UnitBuySoilder");
                      
                        break;
                   
                }


        _but.ChangeButtonColor(_but);





            
           

        

    }
    public void EnableUnitBar(string _name)
    {
        foreach (GameObject manager in UnitManagerList)
        {
            if (manager.name == _name)
                manager.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
    public void DisableUnitBar()
    {
        foreach (GameObject manager in UnitManagerList)
        {
            manager.transform.eulerAngles = new Vector3(0, 90, 0);
        }
    }
    private void ClearUnactiveButton()
    {


        if (Input.GetMouseButtonDown(0))
        {
            int index = 0;
            for (int j = 0; j < ButtonList.Length; j++)
            {
                ButtonElement _butel = ButtonList[j].GetComponent<ButtonElement>();

                if (_butel.Selected == false)
                    index++;
            }
            if (index > 1)
            {
                DisableUnitBar();
              

            }
        }
    }
    public void UnitElementactive(string _name)
    {

          foreach(GameObject manager in UnitManagerList)
        {
            if (manager.name == _name)
                 manager.transform.eulerAngles= new Vector3(0,90,0);
            else
                manager.transform.eulerAngles = new Vector3(0, 0, 0);
        }

    }



    public bool returnactivebuton()
    {
        foreach (GameObject GO in ButtonList)
        {
            ButtonElement _element = GO.GetComponent<ButtonElement>();
            if (_element.Selected == true)
            {
                return true;
            }
        }
        return false;
    }


    public bool CheckActiveButton(string name)
    {


        foreach (GameObject GO in ButtonList)
        {
            ButtonElement _element = GO.GetComponent<ButtonElement>();
            if (_element.Selected == true&&_element.Buttonname==name)
            {
                return true;
            }
        }
        return false;
    }

  

}
