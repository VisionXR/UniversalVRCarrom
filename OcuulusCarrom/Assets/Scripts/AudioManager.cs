using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioSource> ass;
    public static AudioManager instance;

    private void Awake()
    {
        instance = this;
    }
    public void PlayFoulSound()
    {
        ass[0].Play();
    }
    public void PlayRedCoveredSound()
    {
        ass[1].Play();
    }
}
