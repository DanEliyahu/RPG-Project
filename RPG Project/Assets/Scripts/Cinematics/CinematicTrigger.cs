using UnityEngine;
using UnityEngine.Playables;

namespace Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool _alreadyTriggered;

        private void OnTriggerEnter(Collider other)
        {
            if (!_alreadyTriggered && other.gameObject.CompareTag("Player"))
            {
                _alreadyTriggered = true;
                GetComponent<PlayableDirector>().Play();
            }
        }
    }
}