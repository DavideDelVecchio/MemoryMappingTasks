using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class BackgroundMusicController : MonoBehaviour
{
    public AudioMixerSnapshot justBackground, music1, music2, music3;
    public float bpm = 128;

    private float m_TransitionIn;
    private float m_QuaterNote;

    public static BackgroundMusicController singleton; 
    
    void Awake()
    {
        singleton = this;
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start()
    {
        m_QuaterNote = 60 / bpm;
        m_TransitionIn = m_QuaterNote*2;
    }

    public void onSwitchPression(int count)
    {
        switch(count)
        {
            case 0:
                justBackground.TransitionTo(m_TransitionIn);
                break;
            case 1:
                music1.TransitionTo(m_TransitionIn);
                break;
            case 2:
                music2.TransitionTo(m_TransitionIn);
                break;
            case 3:
                music3.TransitionTo(m_TransitionIn);
                break;
        }
    }


}
