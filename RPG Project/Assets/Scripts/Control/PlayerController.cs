using Attributes;
using Combat;
using Core;
using Movement;
using UnityEngine;

namespace Control
{
    public class PlayerController : MonoBehaviour
    {
        private Camera _camera;
        private Mover _playerMover;
        private Fighter _fighter;
        private Health _health;

        private void Awake()
        {
            _playerMover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _camera = Camera.main;
        }

        private void Update()
        {
            if (_health.IsDead) return;
            Ray mouseRay = _camera.ScreenPointToRay(Input.mousePosition);
            if (InteractWithCombat(mouseRay)) return;
            if (InteractWithMovement(mouseRay)) return;
        }

        private bool InteractWithCombat(Ray mouseRay)
        {
            RaycastHit[] hits = Physics.RaycastAll(mouseRay);
            foreach (var hit in hits)
            {
                var target = hit.transform.GetComponent<CombatTarget>();
                if (target == null || !_fighter.CanAttack(target.gameObject)) continue;
                if (Input.GetMouseButton(0))
                {
                    _fighter.StartAttackAction(target.gameObject);
                }

                return true;
            }

            return false;
        }

        private bool InteractWithMovement(Ray mouseRay)
        {
            if (Physics.Raycast(mouseRay, out var hit))
            {
                if (Input.GetMouseButton(0))
                {
                    _playerMover.StartMoveAction(hit.point, 1f);
                }

                return true;
            }

            return false;
        }
    }
}