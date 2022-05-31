using System.Collections;
using UnityEngine;

public class PlatformEffectHandler : MonoBehaviour
{
    [SerializeField] private GameObject _sprites;
    [SerializeField] private ParticleSystem _particleSystem;

    private Coroutine _coroutine;

    private void OnEnable()
    {
        if (_sprites == null || _particleSystem == null)
            throw new System.ArgumentNullException("Отсутствует один из обязательных параметров. Проверьте редактор.");

        _sprites.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out Character character))
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(ShowSplashes());
        }
    }

    private IEnumerator ShowSplashes()
    {
        float timer;

        timer = 0;
        _sprites.SetActive(true);
        _particleSystem.Play();

        while (timer <= 0.3f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        _sprites.SetActive(false);
    }
}
