using Attributes;
using Core;
using Saving;
using UnityEngine;
using UnityEngine.AI;

namespace Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private float _maxSpeed = 5.66f;

        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private Health _health;
        private ActionScheduler _actionScheduler;
        private static readonly int ForwardSpeed = Animator.StringToHash("forwardSpeed");

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _health = GetComponent<Health>();
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Update()
        {
            _navMeshAgent.enabled = !_health.IsDead;
            UpdateAnimation();
        }

        public void StartMoveAction(Vector3 destination, float maxSpeedFraction)
        {
            _actionScheduler.StartAction(this);
            MoveTo(destination, maxSpeedFraction);
        }

        public void MoveTo(Vector3 destination, float maxSpeedFraction)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.speed = _maxSpeed * Mathf.Clamp01(maxSpeedFraction);
            _navMeshAgent.destination = destination;
        }

        private void UpdateAnimation()
        {
            float speed = _navMeshAgent.velocity.magnitude;
            _animator.SetFloat(ForwardSpeed, speed);
        }

        public void Cancel()
        {
            _navMeshAgent.isStopped = true;
        }

        [System.Serializable]
        private struct MoverSaveData
        {
            public SerializableVector3 _position;
            public SerializableVector3 _rotation;
        }
        
        public object CaptureState()
        {
            var data = new MoverSaveData();
            var thisTransform = transform;
            data._position = new SerializableVector3(thisTransform.position);
            data._rotation = new SerializableVector3(thisTransform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            var data = (MoverSaveData) state;
            _navMeshAgent.Warp(data._position.ToVector());
            transform.eulerAngles = data._rotation.ToVector();
        }
    }
}