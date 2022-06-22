using UnityEngine;
public class CoinAudio : MonoBehaviour
{
    private AudioSource ass;
    private void Start()
    {
        ass = GetComponent<AudioSource>();
    }
    public void PlayCoinAudio(float volume)
    {
        ass.volume = volume;
        ass.Play();
    }
    public void OnCollisionExit(Collision collision)
    {
        GameObject tmp = collision.gameObject;
        if (tmp.tag == "Black" || tmp.tag == "White" || tmp.tag == "Red" || tmp.tag == "Striker")
        {
            Rigidbody Otherbody = tmp.GetComponent<Rigidbody>();
            Rigidbody thisbody = GetComponent<Rigidbody>();
            if (thisbody.velocity.magnitude > 0.3f && Otherbody.velocity.magnitude > 0.01f)
            {
                PlayCoinAudio(1);
            }
            else if (Otherbody.velocity.magnitude > 0.01f && thisbody.velocity.magnitude > 0.01 && thisbody.velocity.magnitude < 0.3f)
            {
                float volume = (1 / 0.29f) * (thisbody.velocity.magnitude - 0.01f);
                PlayCoinAudio(volume);
            }
        }
    }
}
