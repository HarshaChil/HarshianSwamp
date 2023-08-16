using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    AudioSource source;
    // Start is called before the first frame update
    void Awake()
    {
        source = GetComponent<AudioSource>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        if (instance == this) return;
        Destroy(gameObject);


    }



    // Update is called once per frame
    public void PlaySound(AudioClip _sound)
    {
        source.PlayOneShot(_sound);
    }

    public void StopMusic()
    {
        source.Stop();
    }

}
