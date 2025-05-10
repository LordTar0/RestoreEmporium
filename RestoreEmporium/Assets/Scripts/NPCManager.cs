using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class NPCManager : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueSystem DialogueSystem;
    [SerializeField] private SpriteRenderer baseSprite;
    [SerializeField] private SpriteRenderer faceSprite;
    [SerializeField] private BoxCollider myCollider;
    [SerializeField] private Animator myAnimator;

    [SerializeField] private bool isTalking;

    NPCData storedData;

    private void Start()
    {
        NPCSetup();
    }

    public void NPCSetup()
    {
        myCollider = GetComponent<BoxCollider>();
        myAnimator = GetComponent<Animator>();

        baseSprite.sprite = null;
        faceSprite.sprite = null;
        DialogueSystem.IsTalking(false);
        myCollider.enabled = false;
        myAnimator.SetBool("IsAtDesk",false);
    }

    public void LoadNPC(NPCData data)
    {
        storedData = data;
    }

    public void Spawn()
    {
        myCollider.enabled = true;
        myAnimator.SetBool("IsAtDesk", true);

        StartCoroutine(AnnoyedTimer(15));
    }

    public void Leave()
    {
        myCollider.enabled = true;
        myAnimator.SetBool("IsAtDesk", false);
    }

    private IEnumerator AnnoyedTimer(int time)
    {
        if (time <= 1) { time = 2; }

        int timer = time;

        while (timer > 0)
        {
            if (timer <= time / 2 && faceSprite.sprite != storedData.AngryFace)
            {
                faceSprite.sprite = storedData.AngryFace;
            }

            timer--;
            yield return new WaitForSeconds(1);
        }

        Leave();
    }

    public void Interact(Interactor interactor, out bool isSuccessful)
    {
        throw new System.NotImplementedException();
    }

    public void EndInteraction()
    {

    }
}
