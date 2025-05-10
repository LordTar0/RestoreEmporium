using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueTextBox;
    [SerializeField] private Image IconBox;
    [SerializeField] private Animator myAnimator;

    Coroutine speechCoroutine;

    public void IsTalking(bool istalking)
    {
        myAnimator.SetBool("isTalking", istalking);
    }

    public void UpdateText(string Text, float TalkingSpeed)
    {
        dialogueTextBox.text = string.Empty;

        if  (speechCoroutine != null) {StopCoroutine(speechCoroutine);}
        speechCoroutine = StartCoroutine(Speech(Text, TalkingSpeed));
    }

    private IEnumerator Speech(string Text, float TalkSpeed)
    {
        foreach (char letter in Text.ToCharArray())
        {
            dialogueTextBox.text += letter;
            yield return new WaitForSeconds(TalkSpeed);
        }
    }

    public void UpdateIconBox(Sprite Icon)
    {
        IconBox.sprite = Icon;
    }
}
