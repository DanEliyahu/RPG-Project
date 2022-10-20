using Attributes;
using Core;
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private AnimatorOverrideController _animatorOverride;
        [SerializeField] private GameObject _equippedPrefab;
        [SerializeField] private float _weaponDamage;
        [SerializeField] private float _weaponRange;
        [SerializeField] private bool _isRightHanded = true;
        [SerializeField] private Projectile _projectile;

        private const string WEAPON_NAME = "Weapon";

        public float WeaponDamage => _weaponDamage;
        public float WeaponRange => _weaponRange;
        public bool HasProjectile => _projectile != null;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);
            if (_equippedPrefab != null)
            {
                GameObject weapon = Instantiate(_equippedPrefab, _isRightHanded ? rightHand : leftHand);
                weapon.name = WEAPON_NAME;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (_animatorOverride != null)
            {
                animator.runtimeAnimatorController = _animatorOverride;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(WEAPON_NAME);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(WEAPON_NAME);
            }

            if (oldWeapon == null)
            {
                return;
            }

            oldWeapon.name = "Destroying";
            Destroy(oldWeapon.gameObject);
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator)
        {
            Projectile projectile = Instantiate(_projectile, _isRightHanded ? rightHand.position : leftHand.position,
                Quaternion.identity);
            projectile.SetTarget(target, instigator, _weaponDamage);
        }
    }
}