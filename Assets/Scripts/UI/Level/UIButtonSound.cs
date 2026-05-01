using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonSound : MonoBehaviour
{
    private void Awake()
    {
        // Беремо компонент кнопки і через код прив'язуємо нашу подію до її OnClick
        Button btn = GetComponent<Button>();
        
        btn.onClick.AddListener(() => 
        {
            EventBus.RaiseUIButtonClicked();
        });
    }
}