using Manager;
using Pattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectTeam : MonoBehaviour {

	// Use this for initialization
    public void OnHumanclick()
    {
        Singleton.classname = "Human";
        // LoadSceneTagetButton.instanece.LoadSceneNum(3);
        Progressbar.Instance.LoadScene(2);

    }
    public void OnOrcClick()
    {
        Singleton.classname = "Orc";
        // LoadSceneTagetButton.instanece.LoadSceneNum(3);
    }
}

