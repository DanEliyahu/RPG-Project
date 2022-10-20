using UnityEngine;

namespace Attributes
{
    public class Experience : MonoBehaviour
    {
        [SerializeField] private float _experiencePoints;

        public void GainExperience(float experience)
        {
            _experiencePoints += experience;
        }
    }
}