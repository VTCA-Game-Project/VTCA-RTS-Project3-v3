using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumCollection;
using RTS_ScriptableObject;
using Manager;

public class BuildElement : MonoBehaviour {

    // Use this for initialization

   
    public ConstructId Id;
     int Price;
    public ConstructPrice offset;
    public Image CurrentImage;
    public Sprite NewImage;
    public GameObject Mouse;
    public Image CountDownIMG;
    public GameObject BuildModel;
    public Vector2 info;
    private BuildControl ClickEvent;
    private float totaltime=0f;
    private float delaytime =5f;
    private float TimeperUpdate = 1f;
    private float totaltimeImage = 0f;
    private float delaytimeImag = 0.5f;
    Color newcolo;
    private bool stateCowdown=false;
    private bool CowDownComplete;
    void Start()
    {
     switch(Id)
        {
            case ConstructId.Barrack:
                Price = offset.Barrack;
                break;
            case ConstructId.Yard:
                Price = offset.Yard;
                break;

            case ConstructId.Refinery:
                Price = offset.Refinery;
                break;
            case ConstructId.Defender:
                Price = offset.Defender;
                break;


        }
        ClickEvent = Mouse.GetComponent<BuildControl>();

        CurrentImage = GetComponent<Image>();
       

        CurrentImage.sprite = NewImage;
        newcolo = CountDownIMG.color;
        newcolo.a = 0;
        CountDownIMG.color = newcolo;
    }

    // Update is called once per frame
    void Update ()
    {
        if(stateCowdown)
        {
            totaltime += TimeperUpdate * Time.deltaTime;

            CountDownIMG.fillAmount -=(TimeperUpdate/delaytime) * Time.deltaTime;

        }

        if(totaltime>delaytime)
        {
            CountDownIMG.fillAmount = 1;
             totaltime = 0f;
            stateCowdown = false;
            CowDownComplete = true;
          
           
        }

        if(CowDownComplete)
        {
            if(totaltimeImage> delaytimeImag)

            {
              
                newcolo.a *= -1;
                CountDownIMG.color = newcolo;

                totaltimeImage = 0;
            }
            else
            {
               
                totaltimeImage += 0.5f * Time.deltaTime;
            }

        }

        
		
	}

    public void OnUnitClick(string inputvalues)
    {
        Player _player = FindObjectOfType<MainPlayer>();
        if (inputvalues == "RIGHT" )
        {
            if (stateCowdown == true|| CowDownComplete==true)
                _player.TakeGold(Price);
            CowDownComplete = false;
            stateCowdown = false;
            newcolo.a = 0;
            totaltime = 0;
            CountDownIMG.fillAmount = 1;
          
            CountDownIMG.color = newcolo;
            return;
        }
        if (!stateCowdown&&!CowDownComplete)
        {
            
            if (_player.GetGold() >= Price)
            {

                _player.TakeGold(-1 * Price);
                stateCowdown = true;
                newcolo.a = 180;
                CountDownIMG.color = newcolo;
            }
        }
        if (CowDownComplete)
        {
            if (inputvalues == "LEFT")
            {
               
                ClickEvent.ResetTaget();               
                ClickEvent.OnselectTaget = true;
                ClickEvent.BuildModel = BuildModel;
                ClickEvent.BuildModelContructID = Id;
                ClickEvent.BuildSize = info;
                ClickEvent.BuildPrice = Price;
                CowDownComplete = false;
                newcolo.a = 0;
                CountDownIMG.color = newcolo;
            }
        }
       

    }


    public void letOnDestroy(Vector3 buildpos)
    {
        Vector3 mypos = new Vector3(transform.position.x - info.x / 2, 0, transform.position.z - info.y / 2);
        ClickEvent.letOnDestroy(mypos, info);
    }




}
