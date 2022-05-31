using UnityEngine;

public class DiamondsGenerator : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Diamond _diamondPrefab;
    [SerializeField] private Transform _diamondSlicesPool;

    private void OnEnable()
    {
        if (_spawnPoints == null || _diamondPrefab == null || _diamondSlicesPool == null)
            throw new System.ArgumentNullException("Отсутствует обязательный параметр. Проверьте редактор.");
    }

    private void Start()
    {
        GenerateDiamonds();
    }

    private void GenerateDiamonds()
    {
        Diamond tempDiamond;

        foreach (Transform spawnPoint in _spawnPoints)
        {
            tempDiamond = Instantiate(_diamondPrefab, spawnPoint);
            tempDiamond.SetPool(_diamondSlicesPool);
        }
    }
}
