using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SystemDialogue : MonoBehaviour
{
    [SerializeField] Animator _myAnim;
    [SerializeField] TextMeshProUGUI _txtScreen;
    [SerializeField] CharacterController _character;
    Queue <string> _dialoguesQueue;
    Texts _texts;

    void Start()
    {
        _dialoguesQueue = new Queue<string>();
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
            ImageOff();
            return;
        }
        string actualText = _dialoguesQueue.Dequeue();
        _txtScreen.text = actualText;
        StartCoroutine(PrintText(actualText));
    }
    void ImageOff()
    {
        _myAnim.SetBool("Text", false);
        _character.enabled = true;
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
}
