namespace LastShift.Player
{
    /// <summary>
    /// Interaction interface for raycast interactable world objects.
    /// Owned by SSOT 03-player-and-core-loop.md (PLR-03).
    /// </summary>
    public interface IInteractable
    {
        string InteractionPrompt { get; }
        void Interact(PlayerController player);
    }
}
