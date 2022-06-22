using System.Collections;
using UnityEngine;

public class EyeBlinking : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(StartBlinking());
    }
    private IEnumerator StartBlinking()
    {
        while(true)
        {
            yield return new WaitForSeconds(3);
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.2f, 0.2f, 0.2f), 1);
            yield return new WaitForSeconds(1);
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 1, 1), 1);
        }
    }

}
