using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private Fighter _fighter;
        private Text _text;

        private void Awake()
        {
            _fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            _text = GetComponent<Text>();
        }

        private void Update()
        {
            _text.text = _fighter.GetTarget() == null ? "N/A" : $"{_fighter.GetTarget().GetPercentage():N0}%";
        }
    }
}