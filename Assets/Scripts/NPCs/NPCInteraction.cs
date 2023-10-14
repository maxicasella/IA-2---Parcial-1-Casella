using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    public Texts texts;
    [SerializeField] Image _img;

    void OnMouseDown()
    {
        FindObjectOfType<SystemDialogue>().ImageOn(texts);
        FindObjectOfType<SystemDialogue>().NPCs(this, _img);
    }
}
