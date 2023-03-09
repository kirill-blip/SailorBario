public interface IInteractionAware
{
    bool CanInteract { get; set; }
    void Interact();
}