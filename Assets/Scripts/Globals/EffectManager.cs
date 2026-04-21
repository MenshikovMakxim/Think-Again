using UnityEngine;
using UnityEngine.Serialization;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;
    public GlowSettingsSO winGlow;
    
    private void Awake() => Instance = this;

    public void ApplyWinGlow(GameObject target)
    {
        SimpleGlow2D glow = target.AddComponent<SimpleGlow2D>();
        glow.Setup(winGlow);
    }
}
