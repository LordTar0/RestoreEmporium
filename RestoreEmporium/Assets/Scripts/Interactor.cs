using UnityEngine;
using UnityEngine.UIElements;

public class Interactor : MonoBehaviour
{
    public LayerMask interactionLayer;
    public float interactionRadias = 1f;
    public bool isInteracting {  get; private set; }

    IInteractable currentInteractable;
    Vector3 lastPos = Vector3.zero;

    public IInteractable GetInteractable(Vector3 position)
    {
        var colliders = Physics.OverlapSphere(position, interactionRadias, interactionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            var interactable = colliders[i].GetComponent<IInteractable>();

            lastPos = position;

            if (interactable != null) { StartInteraction(interactable); currentInteractable = interactable; return interactable; }
        }

        currentInteractable = null;
        return null;
    }

    public void StartInteraction(IInteractable interactable)
    {
        interactable.Interact(this, out bool interactSuccessful);
        isInteracting = true;
    }

    public void EndInteraction()
    {
        currentInteractable.EndInteraction();
        currentInteractable = null;
        isInteracting = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(lastPos, interactionRadias);
    }
}