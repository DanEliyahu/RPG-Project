using Attributes;
using Core;
using Movement;
using Saving;
using UnityEngine;

namespace Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private float _timeBetweenAttacks = 1f;
        [SerializeField] private Transform _rightHandTransform;
        [SerializeField] private Transform _leftHandTransform;
        [SerializeField] private Weapon _defaultWeapon;

        private Health _target;
        private Mover _mover;
        private Animator _animator;
        private ActionScheduler _actionScheduler;
        private Weapon _currentWeapon;

        private float _timeSinceLastAttack = Mathf.Infinity;
        private static readonly int AttackTrigger = Animator.StringToHash("Attack");
        private static readonly int StopAttacking = Animator.StringToHash("StopAttacking");

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Start()
        {
            if (_currentWeapon == null)
            {
                EquipWeapon(_defaultWeapon);
            }
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            if (_target == null) return;
            if (_target.IsDead) return;

            var targetPosition = _target.transform.position;
            bool isInRange = Vector3.Distance(transform.position, targetPosition) < _currentWeapon.WeaponRange;
            if (!isInRange)
            {
                _mover.MoveTo(targetPosition, 1f);
            }
            else
            {
                _mover.Cancel();
                AttackBehaviour();
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            _currentWeapon = weapon;
            _currentWeapon.Spawn(_rightHandTransform, _leftHandTransform, _animator);
        }

        private void AttackBehaviour()
        {
            transform.LookAt(_target.transform);
            if (_timeSinceLastAttack > _timeBetweenAttacks)
            {
                _animator.ResetTrigger(StopAttacking);
                // This will trigger the Hit() event.
                _animator.SetTrigger(AttackTrigger);
                _timeSinceLastAttack = 0;
            }
        }

        // Animation Event
        private void Hit()
        {
            if (_target)
            {
                _target.TakeDamage(gameObject, _currentWeapon.WeaponDamage);
            }
        }

        private void Shoot()
        {
            if (_target && _currentWeapon.HasProjectile)
            {
                _currentWeapon.LaunchProjectile(_rightHandTransform, _leftHandTransform, _target, gameObject);
            }
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead;
        }

        public void StartAttackAction(GameObject target)
        {
            _actionScheduler.StartAction(this);
            _target = target.GetComponent<Health>();
        }

        public void Cancel()
        {
            _animator.ResetTrigger(AttackTrigger);
            _animator.SetTrigger(StopAttacking);
            _target = null;
            _mover.Cancel();
        }

        public Health GetTarget()
        {
            return _target;
        }

        public object CaptureState()
        {
            return _currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            var weaponName = (string) state;
            var weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}