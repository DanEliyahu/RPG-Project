using UnityEngine;

namespace Core
{
    public class PersistentObjectsSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _persistentObjectPrefab;

        private static bool _hasSpawned = false;

        private void Awake()
        {
            if (_hasSpawned)
            {
                return;
            }

            SpawnPersistentObjects();
            _hasSpawned = true;
        }

        private void SpawnPersistentObjects()
        {
            DontDestroyOnLoad(Instantiate(_persistentObjectPrefab));
        }
    }
}