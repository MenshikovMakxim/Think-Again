using UnityEngine;

public class BackButton : MonoBehaviour
{
    public void ClickBack()
    {
        UIManager.Instance.GoBack();
    }
}
