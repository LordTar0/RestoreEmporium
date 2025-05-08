using UnityEngine;

public class Interactor : MonoBehaviour
{
    public LayerMask interactionLayer;
    public float interactionRadias = 1f;
    public bool isInteracting {  get; private set; }

    public IInteractable GetColliders(Vector3 position)
    {
        var colliders = Physics.OverlapSphere(position, interactionRadias, interactionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            var interactable = colliders[i].GetComponent<IInteractable>();

            if (interactable != null) { StartInteraction(interactable); return interactable; }
        }

        return null;
    }

    public void StartInteraction(IInteractable interactable)
    {
        interactable.Interact(this, out bool interactSuccessful);
        isInteracting = true;
    }

    public void EndInteraction()
    {
        isInteracting = false;
    }
}