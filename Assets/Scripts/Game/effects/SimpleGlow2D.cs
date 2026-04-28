using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SimpleGlow2D : MonoBehaviour
{
    [Header("Glow Settings (Налаштуй тут)")]
    [SerializeField] private float paddingMultiplier = 1.2f;
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float pulseAmount = 0.1f;

    [Header("Debug Info (Не чіпати руками)")]
    [SerializeField] private GameObject _glowObject;
    [SerializeField] private Transform _glowTransform;
    [SerializeField] private SpriteRenderer _glowRenderer;
    [SerializeField] private SpriteRenderer _parentRenderer;
    [SerializeField] private float _targetWorldScale;
    
    private Sprite _glowSprite;
    void Start()
    {
        InitGlow();
    }

    private void InitGlow()
    {
        // if (glowSprite == null) 
        // {
        //     Debug.LogWarning($"[{gameObject.name}] Ей, ти забув призначити Glow Sprite в Інспекторі!");
        //     return;
        // }
        _glowSprite = GetComponent<SpriteRenderer>().sprite;
        
        // Якщо раптом світіння вже є — вбиваємо старе, щоб не плодити клонів
        if (_glowObject != null) Destroy(_glowObject);

        _parentRenderer = GetComponent<SpriteRenderer>();
        
        _glowObject = new GameObject("Glow_Generated");
        _glowTransform = _glowObject.transform;
        _glowTransform.SetParent(transform);
        _glowTransform.localPosition = Vector3.zero;

        _glowRenderer = _glowObject.AddComponent<SpriteRenderer>();
        _glowRenderer.sprite = _glowSprite;
        
        // Світло має бути під об'єктом, а не перекривати його
        _glowRenderer.sortingOrder = _parentRenderer.sortingOrder - 1;

        CalculateScale();
    }

    private void CalculateScale()
    {
         Vector2 parentWorldSize = _parentRenderer.bounds.size;
         Vector2 glowRawSize = _glowRenderer.bounds.size;
    
         float maxParentWorldDim = Mathf.Max(parentWorldSize.x, parentWorldSize.y);
         float maxGlowRawDim = Mathf.Max(glowRawSize.x, glowRawSize.y);
         
         _targetWorldScale = (maxParentWorldDim / maxGlowRawDim) * paddingMultiplier;
    }

    void Update()
    {
        // Не крутимо, якщо немає що крутити
        if (_glowTransform == null) return;

        // Обертання
        _glowTransform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
        
        // Пульсація
        float pulse = Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        float currentWorldScale = _targetWorldScale * (1f + pulse);
        
        // Захист від ділення на нуль (в Unity буває всяке)
        float lossyX = Mathf.Abs(transform.lossyScale.x);
        float lossyY = Mathf.Abs(transform.lossyScale.y);
        
        if (lossyX > 0.001f && lossyY > 0.001f)
        {
            _glowTransform.localScale = new Vector3(
                currentWorldScale / lossyX, 
                currentWorldScale / lossyY, 
                1f);
        }
    }
}