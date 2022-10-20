using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    public class Portal : MonoBehaviour
    {
        private enum DestinationIdentifier
        {
            A,
            B,
            C,
            D
        }

        [SerializeField] private int _sceneToLoad = -1;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private DestinationIdentifier _destination;
        [SerializeField] private float _fadeOutTime = 1f;
        [SerializeField] private float _fadeInTime = 2f;
        [SerializeField] private float _fadeWaitTime = 0.5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (_sceneToLoad < 0)
            {
                Debug.LogError("Scene to load is not set on " + name);
                yield break;
            }
            
            DontDestroyOnLoad(gameObject);
            
            var fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(_fadeOutTime);

            var savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();
            
            yield return SceneManager.LoadSceneAsync(_sceneToLoad);
            
            savingWrapper.Load();

            var otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            
            savingWrapper.Save();

            yield return new WaitForSeconds(_fadeWaitTime);
            yield return fader.FadeIn(_fadeInTime);

            Destroy(gameObject);
        }

        private Portal GetOtherPortal()
        {
            var portals = FindObjectsOfType<Portal>();
            return portals.FirstOrDefault(portal =>
                portal.gameObject != gameObject && portal._destination == _destination);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            var player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal._spawnPoint.position);
            player.transform.rotation = otherPortal._spawnPoint.rotation;
        }
    }
}