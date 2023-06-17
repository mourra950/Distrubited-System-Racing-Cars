using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class music : MonoBehaviour
{

    public AudioSource m_MyAudioSource;

    //Play the music
    bool m_Play;
    //Detect when you use the toggle, ensures music isn’t played multiple times
    bool m_ToggleChange;

    void Start()
    {
        //Fetch the AudioSource from the GameObject
        //Ensure the toggle is set to true for the music to play at start-up
        m_Play = true;
        m_MyAudioSource.loop = true;


    }

    void Update()
    {

    }
}
