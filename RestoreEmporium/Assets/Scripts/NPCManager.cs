using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
        storedData = new();
        baseSprite.sprite = null;
        faceSprite.sprite = null;
        DialogueSystem.IsTalking(false);
        myCollider.enabled = false;
        myAnimator.SetBool("IsAtDesk",false);
    }

    public void LoadNPC(NPCData data)
    {
        storedData = data;
        baseSprite.sprite = storedData.Base;
        faceSprite.sprite = storedData.HappyFace;
        DialogueSystem.UpdateIconBox(storedData.Icon);
    }

    public void Spawn()
    {
        int chosenOne = Random.Range(0, GameManager._instance.Database.GetNPCRange());

        NPCData chosenNPCData = GameManager._instance.Database.GetNPCData(chosenOne).data;

        LoadNPC(chosenNPCData);

        GameManager._instance.gameDetails.NPCAtDesk = true;

        myCollider.enabled = true;
        myAnimator.SetBool("IsAtDesk", true);

        StartCoroutine(AnnoyedTimer(35));
    }

    public void Leave()
    {
        GameManager._instance.gameDetails.NPCAtDesk = false;
        myCollider.enabled = false;
        myAnimator.SetBool("IsAtDesk", false);
    }

    private IEnumerator AnnoyedTimer(int time)
    {
        if (time <= 1) { time = 2; }

        int timer = time;

        bool halfAnnoyed = false;
        bool reallyAnnoyed = false;

        while (timer > 0)
        {
            if (timer <= time / 2 && faceSprite.sprite != storedData.BlankFace && !halfAnnoyed)
            {
                halfAnnoyed = true;
                faceSprite.sprite = storedData.BlankFace;
                DialogueSystem.UpdateText("Hello? Could I have some assistance please?", 5, storedData.NPCtalkSound, this.gameObject);
            }

            if (timer <= time / 4 && faceSprite.sprite != storedData.AngryFace && !reallyAnnoyed)
            {
                reallyAnnoyed = true;
                faceSprite.sprite = storedData.AngryFace;
                DialogueSystem.UpdateText("I'm growing tired of waiting?...", 5, storedData.NPCtalkSound, this.gameObject);
            }

            timer--;
            yield return new WaitForSeconds(1);
        }

        DialogueSystem.UpdateText("Forget this, I'm out of here.",5, storedData.NPCtalkSound, this.gameObject);

        yield return new WaitForSeconds(5);

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
