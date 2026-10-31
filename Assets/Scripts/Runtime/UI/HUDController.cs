using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LastShift.Matchstick;
using LastShift.Player;

namespace LastShift.UI
{
    /// <summary>
    /// Player HUD controller managing reticle, interaction prompts, document inspection modals,
    /// matchstick readiness indicators, and audio cue subtitles.
    /// Implements 09-ui-ux-accessibility and PLR-02..05.
    /// </summary>
    public class HUDController : MonoBehaviour
    {
        [Header("Interaction Prompt")]
        [SerializeField] private GameObject _interactionPromptRoot;
        [SerializeField] private TMP_Text _interactionPromptText;

        [Header("Document / Narrative Modal")]
        [SerializeField] private GameObject _documentModalRoot;
        [SerializeField] private TMP_Text _documentModalText;

        [Header("Matchstick Status")]
        [SerializeField] private TMP_Text _matchstickStatusText;

        [Header("Subtitles & Accessibility")]
        [SerializeField] private GameObject _subtitleRoot;
        [SerializeField] private TMP_Text _subtitleText;

        [Header("System References")]
        [SerializeField] private MatchstickController _matchstickController;

        private void Start()
        {
            if (_interactionPromptRoot != null) _interactionPromptRoot.SetActive(false);
            if (_documentModalRoot != null) _documentModalRoot.SetActive(false);
            if (_subtitleRoot != null) _subtitleRoot.SetActive(false);
        }

        private void OnEnable()
        {
            if (_matchstickController != null)
            {
                _matchstickController.OnStateChanged += HandleMatchstickStateChanged;
            }
        }

        private void OnDisable()
        {
            if (_matchstickController != null)
            {
                _matchstickController.OnStateChanged -= HandleMatchstickStateChanged;
            }
        }

        public void ShowInteractionPrompt(string text)
        {
            if (_interactionPromptRoot != null)
            {
                _interactionPromptRoot.SetActive(true);
            }
            if (_interactionPromptText != null)
            {
                _interactionPromptText.text = text;
            }
        }

        public void HideInteractionPrompt()
        {
            if (_interactionPromptRoot != null)
            {
                _interactionPromptRoot.SetActive(false);
            }
        }

        public void ShowDocumentModal(string content)
        {
            if (_documentModalRoot != null)
            {
                _documentModalRoot.SetActive(true);
            }
            if (_documentModalText != null)
            {
                _documentModalText.text = content;
            }
        }

        public void HideDocumentModal()
        {
            if (_documentModalRoot != null)
            {
                _documentModalRoot.SetActive(false);
            }
        }

        public void ShowSubtitle(string text, float duration = 3.0f)
        {
            if (_subtitleRoot != null)
            {
                _subtitleRoot.SetActive(true);
            }
            if (_subtitleText != null)
            {
                _subtitleText.text = text;
            }

            CancelInvoke(nameof(HideSubtitle));
            Invoke(nameof(HideSubtitle), duration);
        }

        private void HideSubtitle()
        {
            if (_subtitleRoot != null)
            {
                _subtitleRoot.SetActive(false);
            }
        }

        private void HandleMatchstickStateChanged(MatchstickState oldState, MatchstickState newState)
        {
            if (_matchstickStatusText == null) return;

            switch (newState)
            {
                case MatchstickState.Unlit:
                    _matchstickStatusText.text = "[LMB] Strike Match (Ready)";
                    break;
                case MatchstickState.Igniting:
                case MatchstickState.Burning:
                    _matchstickStatusText.text = "Match Burning...";
                    break;
                case MatchstickState.Extinguishing:
                    _matchstickStatusText.text = "Extinguished...";
                    break;
            }
        }
    }
}
