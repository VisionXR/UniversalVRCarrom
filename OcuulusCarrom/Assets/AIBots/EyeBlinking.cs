using System.Collections;
using UnityEngine;

public class EyeBlinking : MonoBehaviour
{
    private Vector3 initScale;
    void Start()
    {
        StartCoroutine(StartBlinking());
        initScale = transform.localScale;
    }
    private IEnumerator StartBlinking()
    {
        while(true)
        {
            yield return new WaitForSeconds(3);
            transform.localScale = Vector3.Lerp(initScale, new Vector3(0.2f, 0.2f, 0.2f), 1);
            yield return new WaitForSeconds(1);
            transform.localScale = Vector3.Lerp(transform.localScale, initScale, 1);
        }
    }

}
