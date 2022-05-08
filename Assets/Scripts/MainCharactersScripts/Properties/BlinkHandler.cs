using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class BlinkHandler : MonoBehaviour
{
    [SerializeField] private Mover _mover;
    [SerializeField] private SpriteRenderer _eyes;
    [SerializeField] private SpriteRenderer _mouth;
    [SerializeField] private Transform _pool;

    private Coroutine _coroutine;
    private Material _material;

    private void Start()
    {
        _material = GetComponent<Renderer>().material;
    }

    public void PrepairToBlink()
    {
        if (_coroutine != null)
            StopCoroutine(Blink());

        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        float minAlphaValue;
        float maxAlphaValue;
        float totalBlinkCount;
        float currentBlink;
        WaitForSeconds timer;

        minAlphaValue = 0;
        maxAlphaValue = 255;
        totalBlinkCount = 4;
        currentBlink = 0;
        timer = new WaitForSeconds(0.15f);

        while (currentBlink < totalBlinkCount)
        {
            _eyes.color = new Color(_eyes.color.r, _eyes.color.g, _eyes.color.b, minAlphaValue);
            _mouth.color = new Color(_mouth.color.r, _mouth.color.g, _mouth.color.b, minAlphaValue);
            Evaporate();
            _material.color = new Color(_material.color.r, _material.color.g, _material.color.b, minAlphaValue);
            yield return timer;

            _eyes.color = new Color(_eyes.color.r, _eyes.color.g, _eyes.color.b, maxAlphaValue);
            _mouth.color = new Color(_mouth.color.r, _mouth.color.g, _mouth.color.b, maxAlphaValue);
            Recover();
            _material.color = new Color(_material.color.r, _material.color.g, _material.color.b, maxAlphaValue);
            yield return timer;

            currentBlink++;
            yield return null;
        }
    }

    private void Evaporate()
    {
        int invisibleLayerIndex;

        invisibleLayerIndex = 3;

        for (int i = 0; i < _pool.childCount; i++)
        {
            _pool.GetChild(i).gameObject.layer = invisibleLayerIndex;
        }
    }

    private void Recover()
    {
        int standartLayerIndex;

        standartLayerIndex = 0;

        for (int i = 0; i < _pool.childCount; i++)
        {
            _pool.GetChild(i).gameObject.layer = standartLayerIndex;
        }
    }
}
