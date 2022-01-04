using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Statsbar : MonoBehaviour
{
    [SerializeField] Image fillImageBack;
    [SerializeField] Image fillImageFront;
    [SerializeField] bool delayFill = true;
    [SerializeField] float fillDelay = 0.5f;
    [SerializeField] float fillSpeed = 0.1f;
    float currentFillAmount;
    protected float targetFillAmount;
    float previousFillAmount;
    float t;

    WaitForSeconds waitForDelayFill;

    Coroutine bufferedFillCoroutine;
    Canvas canvas;

    private void Awake() 
    {
        if(TryGetComponent<Canvas>(out Canvas canvas))
        {
            canvas.worldCamera = Camera.main;
        }

        waitForDelayFill = new WaitForSeconds(fillDelay);
    }

    public virtual void Initialize(float currentValue, float maxValue)
    {
        currentFillAmount = currentValue / maxValue;
        targetFillAmount = currentFillAmount;
        fillImageBack.fillAmount = currentFillAmount;
        fillImageFront.fillAmount = currentFillAmount;
    }

    public void UpdateStates(float currentValue, float maxValue)
    {
        targetFillAmount = currentValue / maxValue;

        if(bufferedFillCoroutine != null)
        {
            StopCoroutine(bufferedFillCoroutine);
        }

        if(currentFillAmount > targetFillAmount)
        {
            fillImageFront.fillAmount = targetFillAmount;

            bufferedFillCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageBack));

            return;
        }

        if(currentFillAmount < targetFillAmount)
        {
            fillImageBack.fillAmount = targetFillAmount;

            bufferedFillCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageFront));
        }
    }

    protected virtual IEnumerator BufferedFillingCoroutine(Image image)
    {
        if(delayFill)
        {
            yield return waitForDelayFill;
        }
        previousFillAmount = currentFillAmount;
        t = 0f;
        while(t < 1f)
        {
            t += Time.deltaTime * fillSpeed;
            currentFillAmount = Mathf.Lerp(previousFillAmount, targetFillAmount, t);
            image.fillAmount = currentFillAmount;

            yield return null;
        }
    }
}
