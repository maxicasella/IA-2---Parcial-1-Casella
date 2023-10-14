using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SystemDialogue : MonoBehaviour
{
    [SerializeField] Animator _myAnim;
    [SerializeField] TextMeshProUGUI _txtScreen;
    [SerializeField] CharacterController _character;
    Queue <string> _dialoguesQueue;
    Texts _texts;
    NPCInteraction _npc;
    Image _img;

    void Start()
    {
        _dialoguesQueue = new Queue<string>();
    }

    public void NPCs(NPCInteraction npc, Image img)
    {
        _npc = npc;
        _img = img;
    }
    public void ImageOn(Texts text)
    {
        _myAnim.SetBool("Text", true);
        _texts = text;
        _character.enabled = false;
    }
    public void TextOn()
    {
        _dialoguesQueue.Clear();
        foreach (string textToSave in _texts.textsArray)
        {
            _dialoguesQueue.Enqueue(textToSave);
        }
        NextText();
    }
    public void NextText()
    {
        if (_dialoguesQueue.Count == 0)
        {
            Destroy(_npc);
            Destroy(_img);
            ImageOff();
            _npc = null;
            return;
        }
        string actualText = _dialoguesQueue.Dequeue();
        _txtScreen.text = actualText;
        StartCoroutine(PrintText(actualText));
    }
    void ImageOff()
    {
        _myAnim.SetBool("Text", false);
    }
    IEnumerator PrintText(string text)
    {
        _txtScreen.text = "";
        foreach (char chars in text.ToCharArray())
        {
            _txtScreen.text += chars;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void EnableCharacter()
    {
        _character.enabled = true;
    }
}
