using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSignManager : MonoBehaviour, IInteractable
{
    bool isOpen;
    Animator myAnimator;
    Collider myCollider;
    Material opencloseMat;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<Collider>();
        isOpen = true;

        MeshRenderer meshrender = GetComponentInChildren<MeshRenderer>();

        Material[] materials = meshrender.materials;

        opencloseMat = materials[1];

        ChangeSign();
    }

    public void ChangeSign()
    {
        isOpen = !isOpen;

        GameManager._instance.gameDetails.IsShopOpen = isOpen;
        GameManager._instance.gameDetailVisuals.UpdateShopOpenStatus(isOpen);

        if (isOpen) { EnableSignChange(false); GameManager._instance.ShopStatusChange(); }

        myAnimator.ResetTrigger("MoveSign");
        myAnimator.SetTrigger("MoveSign");

        if (isOpen) { opencloseMat.SetInt("_isOpen", 1); }
        else { opencloseMat.SetInt("_isOpen", 0); }
    }

    public void EnableSignChange(bool enable)
    {
        myCollider.enabled = enable;
    }

    public void EndInteraction()
    {
        throw new System.NotImplementedException();
    }

    public void Interact(Interactor interactor, out bool isSuccessful)
    {
        ChangeSign();
        isSuccessful = true;
    }
}