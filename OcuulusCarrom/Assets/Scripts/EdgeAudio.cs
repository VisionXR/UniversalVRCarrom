using UnityEngine;

public class EdgeAudio : MonoBehaviour
{
    public AudioSource ass;
    public void PlayEdgeAudio(float volume)
    {
        ass.volume = volume;
        ass.Play();
    }

    public void OnCollisionEnter(Collision collision)
    {
        GameObject tmp = collision.gameObject;
        if (tmp.tag == "Black" || tmp.tag == "White" || tmp.tag == "Red" || tmp.tag == "Striker")
        {
            Rigidbody rb = tmp.GetComponent<Rigidbody>();
            if (rb.velocity.magnitude > 0.3f)
            {
                PlayEdgeAudio(1);
            }
            else if (rb.velocity.magnitude > 0.01f && rb.velocity.magnitude < 0.3f)
            {
                float volume = (1 / 0.29f) * (rb.velocity.magnitude - 0.01f);
                PlayEdgeAudio(volume);
            }
        }
    }


}
