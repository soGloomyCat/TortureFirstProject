using System.Collections;
using UnityEngine;

public class SleepersCell : MonoBehaviour
{
    private const int _inviolableObjectsCount = 1;

    [SerializeField] private ParticleSystem _particleSystem;

    private BoxCollider _collider;
    private Coroutine _coroutine;

    public int SleepersCount => transform.childCount - _inviolableObjectsCount;
    public float SidePosition => transform.position.x;
    public float VerticalPosition => transform.position.y;
    public float HorizontalPosition => transform.position.z;

    private void OnEnable()
    {
        if (_particleSystem == null)
            throw new System.ArgumentNullException("Отсутствует обязательный параметр. Проверьте редактор.");

        _collider = GetComponent<BoxCollider>();
    }

    public void StartDestroy()
    {
        //if (_coroutine != null)
        //    StopCoroutine(_coroutine);

        //_coroutine = StartCoroutine(DestroyAvailableSleepers());
        _particleSystem.Play();

        for (int i = 1; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void SetColliderParameters()
    {
        float tempVerticalSize = (float)SleepersCount / 2;
        float tempVerticalCenter = SleepersCount * 0;

        _collider.size = new Vector3(_collider.size.x, tempVerticalSize, _collider.size.z);
        _collider.center = new Vector3(_collider.center.x, tempVerticalCenter, _collider.center.z);
    }

    private IEnumerator DestroyAvailableSleepers()
    {
        WaitForSeconds waiter;

        waiter = new WaitForSeconds(0.15f);
        

        for (int i = transform.childCount - 1; i >= 1; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
            yield return waiter;
        }
    }

}
