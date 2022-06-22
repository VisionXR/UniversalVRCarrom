using UnityEngine;

public class CoinRegister : MonoBehaviour
{
    private void OnEnable()
    {
        CoinManager.instance.AddCoin(gameObject);
    }

    private void OnDisable()
    {
        CoinManager.instance.RemoveCoin(gameObject);
    }
}
