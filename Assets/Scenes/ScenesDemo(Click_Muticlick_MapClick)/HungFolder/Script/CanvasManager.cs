using DelegateCollection;
using EnumCollection;
using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour {

    public GameObject PlayCanvas;
    public GameObject EndCanvas;


    

	void Start ()
    {
        PlayCanvas.SetActive(true);
        EndCanvas.SetActive(false);

        Player[] buySoliderButtons = FindObjectsOfType<Player>();
        if (buySoliderButtons != null)
        {
            for (int i = 0; i < buySoliderButtons.Length; i++)
            {
                buySoliderButtons[i].LoseAction = Lose;
            }
        }
    }
	
	// Update is called once per frame

    public void Lose(object type)
    {

        if ((Group)type == Group.Player)
            QuitClick("Lost");
        if ((Group)type == Group.NPC)
            QuitClick("Win");

    }
    public void QuitClick(string values)
    {
        PlayCanvas.SetActive(false);
        EndCanvas.SetActive(true);
        End_CanvasScript end = EndCanvas.GetComponent<End_CanvasScript>();
        end.Command = values;
        Time.timeScale = 0f;
    }
    public void ReturnClick()
    {

        Destroy(SoundManager.instanece.gameObject);
        Time.timeScale = 1f;
        LoadSceneTagetButton.instanece.LoadSceneNum(0);
    }
}


