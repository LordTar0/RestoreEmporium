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

    [SerializeField] private string[] firstStageAnnoyed;
    [SerializeField] private string[] secondStageAnnoyed;
    [SerializeField] private string[] leavingAnnoyed;

    NPCData storedData;

    Coroutine annoyedTimer;

    Item iteminMind = new();
    bool isFromPlayer;

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
        iteminMind = new();
    }

    public void LoadNPC(NPCData data)
    {
        storedData = data;
        baseSprite.sprite = storedData.Base;
        faceSprite.sprite = storedData.HappyFace;
        DialogueSystem.UpdateIconBox(storedData.Icon);


        if (PlayerManager._instance.inventory.Slots.Count > 0 && Random.Range(0, 100) >= 50)
        {
            int ItemToBuy = Random.Range(0, PlayerManager._instance.inventory.Slots.Count - 1);
            iteminMind.GetItemData(GameManager._instance.Database, ItemToBuy);
            isFromPlayer = true;
        }
        else 
        {
            int Item = Random.Range(0, GameManager._instance.Database.GetItemRange());
            iteminMind.GetItemData(GameManager._instance.Database, Item);
        }
    }

    public void Spawn()
    {
        int chosenOne = Random.Range(0, GameManager._instance.Database.GetNPCRange());

        NPCData chosenNPCData = GameManager._instance.Database.GetNPCData(chosenOne).data;

        LoadNPC(chosenNPCData);

        GameManager._instance.gameDetails.NPCAtDesk = true;

        myCollider.enabled = true;
        myAnimator.SetBool("IsAtDesk", true);

        annoyedTimer = StartCoroutine(AnnoyedTimer(35));
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
        int choice;

        while (timer > 0)
        {
            if (timer <= time / 2 && faceSprite.sprite != storedData.BlankFace && !halfAnnoyed)
            {
                halfAnnoyed = true;
                faceSprite.sprite = storedData.BlankFace;
                choice = Random.Range(0, firstStageAnnoyed.Length);
                DialogueSystem.UpdateText(firstStageAnnoyed[choice], 5, storedData.NPCtalkSound, this.gameObject, SpeechType.Default);
            }

            if (timer <= time / 4 && faceSprite.sprite != storedData.AngryFace && !reallyAnnoyed)
            {
                reallyAnnoyed = true;
                faceSprite.sprite = storedData.AngryFace;
                choice = Random.Range(0, secondStageAnnoyed.Length);
                DialogueSystem.UpdateText(secondStageAnnoyed[choice], 5, storedData.NPCtalkSound, this.gameObject, SpeechType.Default);
            }

            timer--;
            yield return new WaitForSeconds(1);
        }

        choice = Random.Range(0, leavingAnnoyed.Length);
        DialogueSystem.UpdateText(leavingAnnoyed[choice], 5, storedData.NPCtalkSound, this.gameObject, SpeechType.Default);

        yield return new WaitForSeconds(5);

        Leave();
    }

    public void Interact(Interactor interactor, out bool isSuccessful)
    {
        StopCoroutine(annoyedTimer);

        string dialogue;

        if (isFromPlayer)
        {
            dialogue = $"I would like to purchase {iteminMind.NameAndDescription.Name} from you.";
        }
        else
        {
            dialogue = $"Would you be able to repair my {iteminMind.NameAndDescription.Name}?";
        }

        DialogueSystem.UpdateText(dialogue, 5, storedData.NPCtalkSound, this.gameObject, SpeechType.Choice);

        isSuccessful = true;
    }

    public void OptionYes()
    {
        //AddRepair part!
        DialogueSystem.UpdateText("Yay! thank you.", 5, storedData.NPCtalkSound, this.gameObject, SpeechType.Default);

        //delete me!
        EndInteraction();

        PlayerManager._instance.FrontDeskSelect();
    }

    public void OptionNo()
    {
        DialogueSystem.UpdateText("oh... ok then...", 5, storedData.NPCtalkSound, this.gameObject, SpeechType.Default);

        EndInteraction();

        PlayerManager._instance.FrontDeskSelect();
    }

    public void EndInteraction()
    {
        Leave();
    }
}