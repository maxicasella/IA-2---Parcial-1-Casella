using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    public Texts texts;
    [SerializeField] Image _img;
    bool _dialogueStarted = false;

    void OnMouseDown()
    {
        if (_dialogueStarted) return;

        _dialogueStarted = true;
        var dialogue = FindObjectOfType<SystemDialogue>();
        dialogue.ImageOn(texts);
        dialogue.NPCs(this, _img);
        dialogue.TextOn();
    }

}
