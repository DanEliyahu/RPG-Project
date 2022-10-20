using System.Collections;
using Saving;
using UnityEngine;

namespace SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string DEFAULT_SAVE_FILE_NAME = "save";
        [SerializeField] private float _fadeInTime = 0.2f;
        private SavingSystem _savingSystem;

        private IEnumerator Start()
        {
            _savingSystem = GetComponent<SavingSystem>();
            var fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return _savingSystem.LoadLastScene(DEFAULT_SAVE_FILE_NAME);
            yield return fader.FadeIn(_fadeInTime);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        public void Load()
        {
            _savingSystem.Load(DEFAULT_SAVE_FILE_NAME);
        }

        public void Save()
        {
            _savingSystem.Save(DEFAULT_SAVE_FILE_NAME);
        }
    }
}