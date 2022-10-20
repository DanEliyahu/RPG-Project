using UnityEngine;

namespace Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression")]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] _characterClasses;

        public float GetHealth(CharacterClass characterClass, int level)
        {
            foreach (var progressionCharacterClass in _characterClasses)
            {
                if (progressionCharacterClass._characterClass.Equals(characterClass))
                {
                    //return progressionCharacterClass._health[level - 1];
                }
            }

            return 0;
        }
        
        [System.Serializable]
        private class ProgressionCharacterClass
        {
            [SerializeField] public CharacterClass _characterClass;
            [SerializeField] public ProgressionStat[] _stats;
        }

        [System.Serializable]
        private class ProgressionStat
        {
            public Stat _stat;
            public float[] _levels;
        }
    }
}
