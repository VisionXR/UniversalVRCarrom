using UnityEngine;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour
{
    public Text WinText;
    void Start()
    {
        GameManager.instance.GameCompleted += OnGameCompleted;
    }

    private void OnGameCompleted(string obj)
    {
        WinText.text = obj;
    }
}
