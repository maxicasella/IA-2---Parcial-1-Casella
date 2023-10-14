using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public Texts texts;

    void OnMouseDown()
    {
        FindObjectOfType<SystemDialogue>().ImageOn(texts);
    }
}
