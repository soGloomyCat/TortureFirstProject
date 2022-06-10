using System.Collections;
using UnityEngine;

public class BlinkHandler : MonoBehaviour
{
    private Coroutine _coroutine;

    public void PrepairToBlink(Material material, SpriteRenderer eyes, SpriteRenderer mouth)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Blink(material, eyes, mouth));
    }

    private IEnumerator Blink(Material material, SpriteRenderer eyes, SpriteRenderer mouth)
    {
        float minAlphaValue;
        float maxAlphaValue;
        float totalBlinkCount;
        float currentBlink;
        float tempAlphaValue;
        float changeMultiplier;
        WaitForSeconds mainTimer;
        WaitForSeconds secondTimer;


        minAlphaValue = 0.51f;
        maxAlphaValue = 1f;
        totalBlinkCount = 3;
        currentBlink = 0;
        changeMultiplier = 2f;
        mainTimer = new WaitForSeconds(0.05f);
        secondTimer = new WaitForSeconds(0.01f);

        while (currentBlink < totalBlinkCount)
        {
            while (material.color.a > minAlphaValue)
            {
                tempAlphaValue = material.color.a - changeMultiplier * Time.deltaTime;
                eyes.color = new Color(eyes.color.r, eyes.color.g, eyes.color.b, tempAlphaValue);
                mouth.color = new Color(mouth.color.r, mouth.color.g, mouth.color.b, tempAlphaValue);
                material.color = new Color(material.color.r, material.color.g, material.color.b, tempAlphaValue);
                yield return secondTimer;
            }

            yield return mainTimer;

            while (material.color.a < maxAlphaValue)
            {
                tempAlphaValue = material.color.a + changeMultiplier * Time.deltaTime;
                eyes.color = new Color(eyes.color.r, eyes.color.g, eyes.color.b, tempAlphaValue);
                mouth.color = new Color(mouth.color.r, mouth.color.g, mouth.color.b, tempAlphaValue);
                material.color = new Color(material.color.r, material.color.g, material.color.b, tempAlphaValue);
                yield return secondTimer;
            }

            yield return mainTimer;

            currentBlink++;
        }
    }
}
