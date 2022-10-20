using System.Collections;
using UnityEngine;

namespace Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] private Weapon _weapon;
        [SerializeField] private float _respawnTime = 5f;

        private Collider _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }
            other.GetComponent<Fighter>().EquipWeapon(_weapon);
            StartCoroutine(HideForSeconds(_respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            SetPickupVisibility(false);
            yield return new WaitForSeconds(seconds);
            SetPickupVisibility(true);
        }

        private void SetPickupVisibility(bool visibility)
        {
            _collider.enabled = visibility;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(visibility);
            }
        }
    }
}
