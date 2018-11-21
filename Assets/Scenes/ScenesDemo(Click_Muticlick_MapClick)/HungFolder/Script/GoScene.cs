using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoScene : MonoBehaviour {

    public Slider MusicSlider;
    public Slider EffectSlider;

  

    public GameObject fond;

    private void Start()
    {
        fond.SetActive(false);
    }
    public void Go_select_Scene()
    {
         LoadSceneTagetButton.instanece.LoadSceneNum(1);
       
    }
    private void Update()
    {
        SoundManager.instanece.SeteffectVolum(EffectSlider.value);


        SoundManager.instanece.SetMusicVolum(MusicSlider.value);

    }

    public void Close()
    {
        fond.SetActive(false);

    }
    public void Open()
    {
        fond.SetActive(true);

    }
    public void OnQuit()
    {
        Application.Quit();
    }

  

}
