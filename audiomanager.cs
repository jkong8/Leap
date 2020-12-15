/*AUTHOR: JEREMY TUCKER
 * NOTE: PLAYS ALL THE SOUNDS IN THE GAME*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audiomanager : MonoBehaviour
{
    public AudioClip jump;
    public AudioClip walk;
    public AudioClip dash;
    public AudioClip hit;
    public AudioClip music;

    public AudioSource Audio;
    // Start is called before the first frame update
    void Start()
    {
        Audio = GetComponent<AudioSource>();
        Audio.clip = music;
        Audio.Play();
    }

}
