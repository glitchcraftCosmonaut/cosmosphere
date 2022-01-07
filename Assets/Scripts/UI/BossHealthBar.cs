using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : Statsbar_HUD
{
    [SerializeField] public Text bossName;
    protected override void SetPercentText()
    {
        // percentText.text = (targetFillAmount * 100f).ToString("f2") + "%";
        percentText.text = targetFillAmount.ToString("P");
    }
}
