using Attributes;
using Core;
using UnityEngine;

namespace Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 2f;
        [SerializeField] private bool _isHoming = true;
        [SerializeField] private GameObject _hitEffect;
        [SerializeField] private float _maxLifeTime = 10f;

        private Health _target;
        private GameObject _instigator;
        private float _damage;

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            _target = target;
            _damage = damage;
            _instigator = instigator;
            Destroy(gameObject, _maxLifeTime);
        }

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        private void Update()
        {
            if (!_target)
            {
                return;
            }

            if (_isHoming && !_target.IsDead)
            {
                transform.LookAt(GetAimLocation());
            }

            transform.Translate(Vector3.forward * (_speed * Time.deltaTime));
        }

        private Vector3 GetAimLocation()
        {
            var targetCollider = _target.GetComponent<Collider>();
            return targetCollider ? targetCollider.bounds.center : _target.transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _target.gameObject)
            {
                if (_target.IsDead)
                {
                    return;
                }

                _target.TakeDamage(_instigator, _damage);
                if (_hitEffect != null)
                {
                    Instantiate(_hitEffect, transform.position, transform.rotation);
                }

                Destroy(gameObject);
            }
        }
    }
}