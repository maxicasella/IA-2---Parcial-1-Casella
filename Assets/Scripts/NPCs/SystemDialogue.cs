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
    Coroutine _typingCoroutine;
    bool _textFullyShown = false;
    bool _isTyping = false;
    bool _dialogueActive = false;
    string _currentText;

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
        _dialogueActive = true;
        StartCoroutine(InitialDelay());
    }
    IEnumerator InitialDelay()
    {
        yield return new WaitForEndOfFrame(); // <- evita conflicto con el mismo clic
        yield return new WaitForSeconds(0.1f); // <- da tiempo a soltar el mouse
        NextText();
    }
    public void OnClickNext() 
    {
        if (_dialogueActive) return;
        if (_isTyping)
        {
            if (_typingCoroutine != null) StopCoroutine(_typingCoroutine);

            _txtScreen.text = _currentText;
            _isTyping = false;
            _textFullyShown = true;
        }
        else if (_textFullyShown) NextText();
    }
    public void NextText()
    {
        if (_dialoguesQueue.Count == 0)
        {
            _dialogueActive = false;
            Destroy(_npc);
            Destroy(_img);
            ImageOff();
            _npc = null;
            return;
        }
        
        _currentText = _dialoguesQueue.Dequeue();
        
        if (_typingCoroutine != null) StopCoroutine(_typingCoroutine);

        _textFullyShown = false;
        _typingCoroutine = StartCoroutine(PrintText(_currentText));
    }
    void ImageOff()
    {
        _myAnim.SetBool("Text", false);
    }
    IEnumerator PrintText(string text) //IA 2 - Parcial 1
    {
        _isTyping = true;
        _textFullyShown = false;
        _txtScreen.text = "";
        foreach (char chars in text.ToCharArray())
        {
            _txtScreen.text += chars;
            yield return new WaitForSeconds(0.02f);
        }
        _isTyping = false;
        _textFullyShown = true;
        _dialogueActive = false;
    }

    public void EnableCharacter()
    {
        _character.enabled = true;
    }
}
