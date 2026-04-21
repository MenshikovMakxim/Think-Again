using UnityEngine;

[CreateAssetMenu(fileName = "NewGlowSettings", menuName = "ContentManager/Glow Settings")]
public class GlowSettingsSO : ScriptableObject
{
    public Sprite glowSprite;
    public float paddingMultiplier = 1.3f;
    public float pulseSpeed = 3f;
    public float pulseAmount = 0.1f;
    public float rotationSpeed = 45f;
}
