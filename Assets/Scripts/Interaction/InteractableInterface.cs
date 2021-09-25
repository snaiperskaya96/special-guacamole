using UnityEngine.EventSystems;

public interface IInteractable : IEventSystemHandler
{
    void OnInteraction(InteractorComponent Interactor);
}