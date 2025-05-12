using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSignManager : MonoBehaviour, IInteractable
{
    bool isOpen;
    Animator myAnimator;
    Collider myCollider;
    Material opencloseMat;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        isOpen = false;

        MeshRenderer meshrender = GetComponentInChildren<MeshRenderer>();

        Material[] materials = meshrender.materials;

        opencloseMat = materials[1];

        UpdateSignVisuals();
    }

    public void ChangeSign()
    {
        isOpen = !isOpen;

        GameManager._instance.gameDetails.IsShopOpen = isOpen;
        GameManager._instance.gameDetailVisuals.UpdateShopOpenStatus(isOpen);
        GameManager._instance.ShopStatusChange();

        if (isOpen) { EnableSignChange(false);}
        else {GameManager._instance.CloseUpShop(); EnableSignChange(false); }

        UpdateSignVisuals();
    }

    public void UpdateSignVisuals()
    {
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