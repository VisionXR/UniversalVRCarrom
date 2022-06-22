using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleAudio : MonoBehaviour
{
    public AudioSource ass;
    public void PlayHoleAudio()
    {
        ass.Play();
    }
    public void OnCollisionEnter(Collision collision)
    {
        GameObject tmp = collision.gameObject;
        if (tmp.tag == "Black" || tmp.tag == "White" || tmp.tag == "Red" || tmp.tag == "Striker")
        {
            PlayHoleAudio();
        }
    }
}
