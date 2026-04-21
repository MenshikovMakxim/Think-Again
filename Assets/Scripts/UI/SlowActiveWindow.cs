using System.Collections;
using UnityEngine;

public class SlowActiveWindow : MonoBehaviour
{
    private GameObject _panel;
    public float delayInSeconds = 1.5f;
    
    public void OpenWindow(GameObject panel)
    {
        _panel = panel;
        StartCoroutine(WaitAndOpenCoroutine());
    }

    private IEnumerator WaitAndOpenCoroutine()
    {

        yield return new WaitForSecondsRealtime(delayInSeconds);
        
        if (_panel != null)
        {
            _panel.SetActive(true);
        }
    }
}