using Common.Entity;
using EnumCollection;
using InterfaceCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOn : MonoBehaviour
{
    private ISelectable selectableObject;
    public Group PlayerGroup { get; private set; }
    private void Awake()
    {
        AIAgent agent = GetComponent<AIAgent>();
        selectableObject = agent;
        PlayerGroup = agent.PlayerGroup;
    }
    void Start()
    {
        Click.Instance.Add(this);
        UnSelect();
    }

    public void Select()
    {
        selectableObject.Select();           
    }
    public void UnSelect()
    {
        selectableObject.UnSelect();
    }
    public void Action()
    {
        selectableObject.Action();
    }
}
