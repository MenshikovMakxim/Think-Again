using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SimpleGlow2D : MonoBehaviour
{
    public GlowSettingsSO settings;

    private GameObject _glowObject;
    private Transform _glowTransform;
    private SpriteRenderer _glowRenderer;
    private SpriteRenderer _parentRenderer;
    private float _targetWorldScale;

    public void Setup(GlowSettingsSO newSettings)
    {
        settings = newSettings;
        InitGlow();
    }

    void Start()
    {
        if (settings != null) InitGlow();
    }

    private void InitGlow()
    {
        if (settings.glowSprite == null) return;
        
        if (_glowObject != null) Destroy(_glowObject);

        _parentRenderer = GetComponent<SpriteRenderer>();
        _glowObject = new GameObject("Glow_SO");
        _glowTransform = _glowObject.transform;
        _glowTransform.SetParent(transform);
        _glowTransform.localPosition = Vector3.zero;

        _glowRenderer = _glowObject.AddComponent<SpriteRenderer>();
        _glowRenderer.sprite = settings.glowSprite;
        _glowRenderer.sortingOrder = _parentRenderer.sortingOrder - 1;

        CalculateScale();
    }

    private void CalculateScale()
    {
        float maxParentDim = Mathf.Max(_parentRenderer.bounds.size.x, _parentRenderer.bounds.size.y);
        float maxGlowDim = Mathf.Max(settings.glowSprite.bounds.size.x, settings.glowSprite.bounds.size.y);
        _targetWorldScale = (maxParentDim / maxGlowDim) * settings.paddingMultiplier;
    }
    
    // private void CalculateScale()
    //  {
    //      Vector2 parentWorldSize = _parentRenderer.bounds.size;
    //      Vector2 glowRawSize = settings.glowSprite.bounds.size;
    //
    //      float maxParentWorldDim = Mathf.Max(parentWorldSize.x, parentWorldSize.y);
    //      float maxGlowRawDim = Mathf.Max(glowRawSize.x, glowRawSize.y);
    //      
    //      _targetWorldScale = (maxParentWorldDim / maxGlowRawDim) * settings.paddingMultiplier;
    //  }

    void Update()
    {
        if (_glowTransform == null || settings == null) return;

        _glowTransform.Rotate(Vector3.forward * settings.rotationSpeed * Time.deltaTime);
        float pulse = Mathf.Sin(Time.time * settings.pulseSpeed) * settings.pulseAmount;
        float currentWorldScale = _targetWorldScale * (1f + pulse);
        
        _glowTransform.localScale = new Vector3(
            currentWorldScale / Mathf.Abs(transform.lossyScale.x), 
            currentWorldScale / Mathf.Abs(transform.lossyScale.y), 1f);
    }
}