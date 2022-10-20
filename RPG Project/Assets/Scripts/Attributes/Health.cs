using Core;
using Saving;
using Stats;
using UnityEngine;

namespace Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float _healthPoints = 100f;

        private float _startingHealthPoints;

        public bool IsDead { get; private set; }
        private static readonly int DieTrigger = Animator.StringToHash("Die");

        private void Start()
        {
            _healthPoints = GetComponent<BaseStats>().GetHealth();
            _startingHealthPoints = _healthPoints;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            _healthPoints = Mathf.Max(_healthPoints - damage, 0);
            if (_healthPoints <= 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        public float GetPercentage()
        {
            return 100 * (_healthPoints / _startingHealthPoints);
        }

        private void Die()
        {
            if (IsDead) return;
            IsDead = true;
            GetComponent<Animator>().SetTrigger(DieTrigger);
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            var experience = instigator.GetComponent<Experience>();
            if (experience == null)
            {
                return;
            }

            experience.GainExperience(GetComponent<BaseStats>().GetExperienceReward());
        }

        public object CaptureState()
        {
            return _healthPoints;
        }

        public void RestoreState(object state)
        {
            _healthPoints = (float) state;
            if (_healthPoints <= 0)
            {
                Die();
            }
        }
    }
}