using System.Collections;
using UnityEngine;

public class BoardEffectHandler : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private GameObject _sprites;

    private Coroutine _splashesCoroutine;

    private void OnEnable()
    {
        if (_particleSystem == null || _sprites == null)
            throw new System.ArgumentNullException("Отсутствует обязательный объект. Проверьте редактор.");

        _sprites.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent<Character>(out Character character))
        {
            if (_splashesCoroutine != null)
                StopCoroutine(_splashesCoroutine);

            _splashesCoroutine = StartCoroutine(ShowSplashes());
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
