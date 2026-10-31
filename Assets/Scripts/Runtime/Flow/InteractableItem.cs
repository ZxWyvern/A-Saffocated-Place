using System;
using UnityEngine;
using LastShift.Player;

namespace LastShift.Flow
{
    /// <summary>
    /// Interactable scene object providing prompts and narrative text inspects.
    /// Implements NAR-ITEM-01..03, NAR-04, and LVL-01..05 item triggers.
    /// </summary>
    public class InteractableItem : MonoBehaviour, IInteractable
    {
        [Header("Interaction Display")]
        [SerializeField] private string _promptText = "Press [E] to Interact";
        [TextArea(3, 8)]
        [SerializeField] private string _narrativeText = "";

        [Header("Behavior")]
        [SerializeField] private bool _isReusable = true;
        [SerializeField] private bool _disableGOClippedOnUse = false;
        [SerializeField] private AudioSource _interactionAudio;

        private bool _hasBeenUsed = false;

        public string InteractionPrompt => _promptText;
        public string PromptText => _promptText;
        public string NarrativeText => _narrativeText;
        public bool HasBeenUsed => _hasBeenUsed;

        public event Action<InteractableItem> OnInteracted;

        public void Interact(PlayerController player)
        {
            if (!_isReusable && _hasBeenUsed) return;

            _hasBeenUsed = true;

            if (_interactionAudio != null)
            {
                _interactionAudio.Play();
            }

            OnInteracted?.Invoke(this);

            if (_disableGOClippedOnUse)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
