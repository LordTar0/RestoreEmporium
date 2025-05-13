using FMOD.Studio;
using FMODUnity;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueTextBox;
    [SerializeField] private Image IconBox;
    [SerializeField] private Animator myAnimator;
    [SerializeField] private GameObject nextPrompt;
    [SerializeField] private GameObject choicesBox;

    Coroutine speechCoroutine;

    EventInstance currentTalkSound;

    public void IsTalking(bool istalking)
    {
        myAnimator.SetBool("isTalking", istalking);
    }

    public void UpdateConversation()
    {

    }

    public void UpdateText(string Text, float TalkingSpeed, EventReference speechSound, GameObject owner, SpeechType speechType)
    {
        dialogueTextBox.text = string.Empty;

        if  (speechCoroutine != null) {StopCoroutine(speechCoroutine);}
        speechCoroutine = StartCoroutine(Speech(Text, TalkingSpeed, speechSound, owner,speechType));
    }

    private IEnumerator Speech(string Text, float TalkSpeed, EventReference speechSound, GameObject owner,SpeechType speechType)
    {
        IsTalking(true);

        yield return new WaitForSeconds(1);

        if (currentTalkSound.IsUnityNull())
        {
            currentTalkSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            currentTalkSound.release();
        }

        currentTalkSound = AudioManager._instance.CreateEventInstance3D(speechSound,owner);
        currentTalkSound.start();

        foreach (char letter in Text.ToCharArray())
        {

            dialogueTextBox.text += letter;
            yield return new WaitForSeconds(TalkSpeed * Time.deltaTime);
        }

        currentTalkSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        switch (speechType)
        {
            case SpeechType.Choice: choicesBox.SetActive(true); while (choicesBox.activeInHierarchy) { yield return null; } break;
            default: break;
        }

        yield return new WaitForSeconds(3);

        IsTalking(false);
    }

    public void ChoiceMade()
    {
        choicesBox.SetActive(false);
    }

    public void UpdateIconBox(Sprite Icon)
    {
        IconBox.sprite = Icon;
    }
}

public enum SpeechType
{
    Default,
    Choice,
}