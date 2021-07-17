using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    private static AudioController _instance;

    void Awake()
    {
        //Singleton for Audio - if there is already a Background Audio
        if (!_instance)
            _instance = this;
        else
            Destroy(this.gameObject);


        //Provides the Audio from being stopped on Scene change
        DontDestroyOnLoad(this.gameObject);
    }
}
