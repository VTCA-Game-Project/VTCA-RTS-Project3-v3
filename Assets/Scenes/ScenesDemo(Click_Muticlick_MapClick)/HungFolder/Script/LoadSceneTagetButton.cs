using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneTagetButton : MonoBehaviour {

    public static LoadSceneTagetButton instanece { get; private set; }

    private void Awake()
    {
        if (instanece == null)
            instanece = this;
        else
            Destroy(instanece.gameObject);

        DontDestroyOnLoad(instanece.gameObject);
    }
    public void LoadSceneNum(int num)
    {
      
        if (num<0||num>=SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning("Can't load SCence num" + num + ", SceneManager only has" + SceneManager.sceneCountInBuildSettings + "scenes in BuildSettings!");
            return;
        }

        LoadingScreenManager.LoadScene(num);

    }

	
}
