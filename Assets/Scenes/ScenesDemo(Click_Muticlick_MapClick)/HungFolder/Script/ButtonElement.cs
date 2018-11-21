using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonElement : MonoBehaviour {
    
    private CellManager cell;
    private bool OnCownDown=false ;
   [HideInInspector]
    public bool Selected = false;
    [HideInInspector]
    public string Buttonname;
    private Color _color;
    private ButtonDown downfunction;
    
    private Image _image;
	void Start ()
    {
        
        Buttonname = this.gameObject.name;
        cell = GameObject.FindObjectOfType<CellManager>();
        _image = this.GetComponent<Image>();

        _color = _image.color;
        _color.a *= -1;
        _image.color = _color;
        downfunction = GetComponentInParent<ButtonDown>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        


    }
    public void Showon(string name)
    {

        if(!downfunction.returnactivebuton()|| downfunction.CheckActiveButton(name))
        {
            this.Selected = !Selected;
            ButtonElement _but = this.GetComponent<ButtonElement>();
            downfunction.ChangeStatusButton(_but);

        }      
    }



   
    public void ChangeButtonColor(ButtonElement _bu)
    {
       
        _color = _bu._image.color;
        if (_bu.Selected == true)
        {
            _color.a *=-1;
        }
        else
        {
            _color.a *= -1;
        }

        _bu._image.color = _color;
    }
}
