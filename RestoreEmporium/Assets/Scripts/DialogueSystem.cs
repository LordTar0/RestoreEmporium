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

    Coroutine speechCoroutine;

    EventInstance currentTalkSound;

    public void IsTalking(bool istalking)
    {
        myAnimator.SetBool("isTalking", istalking);
    }

    public void UpdateText(string Text, float TalkingSpeed, EventReference speechSound, GameObject owner)
    {
        dialogueTextBox.text = string.Empty;

        if  (speechCoroutine != null) {StopCoroutine(speechCoroutine);}
        speechCoroutine = StartCoroutine(Speech(Text, TalkingSpeed, speechSound, owner));
    }

    private IEnumerator Speech(string Text, float TalkSpeed, EventReference speechSound, GameObject owner)
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

        yield return new WaitForSeconds(3);

        IsTalking(false);
    }

    public void UpdateIconBox(Sprite Icon)
    {
        IconBox.sprite = Icon;
    }
}
