using UnityEngine;
using UnityEngine.UI;

public class BladeDisplayUI : MonoBehaviour
{
    static Image cooldownImage;
    private void Awake()
    {
        cooldownImage = transform.Find("Cooldown Image").GetComponent<Image>();

    }
    public static void UpdateCooldownImage(float fillAmount) => cooldownImage.fillAmount = fillAmount;
}
