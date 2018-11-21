using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {

    public static SoundManager instanece { get; private set; }
    public AudioSource MainMusic;
    public List<AudioClip> Musiclist;
    public AudioSource SoundEffect;
    public List<AudioClip> SoundEffectList;


   
   
    void Awake ()
    {
        if (instanece == null)
            instanece = this;
        else
            Destroy(instanece.gameObject);


        DontDestroyOnLoad(this.gameObject);
	}
    private void Start()
    {
        MainMusic.clip = Musiclist[0];
        MainMusic.Play();
        
    }
    // Update is called once per frame
    void Update ()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                SoundEffect.clip = SoundEffectList[4];
                SoundEffect.Play();
               
            }
        }
    }
    public void PlayEffect(int num)
    {
        SoundEffect.clip = SoundEffectList[num];
        SoundEffect.Play();
    }
    public  void ChangeMusic(int num)
    {
        int index= num;
        if (num == 3)
            index = 2;
        MainMusic.Stop();
        MainMusic.clip = Musiclist[index];
        MainMusic.Play();
    }
    public void SetMusicVolum(float values)
    {
        if (values > 1 || values < 0)
        {
            SoundEffect.volume = 1;
            return;
        }

        MainMusic.volume = values;
    }
    public void SeteffectVolum(float values)
    {
        if (values > 1 || values < 0)
        {
            SoundEffect.volume = 1;
            return;
        }
        SoundEffect.volume = values;
    }
}
