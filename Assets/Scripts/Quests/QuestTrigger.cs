using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestTrigger : MonoBehaviour
{
    [SerializeField] QuestManager _questManager;
    [SerializeField] TextMeshProUGUI _txt;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            RandomQuest actualQuest = _questManager.GetQuest();

            if (actualQuest != null) _txt.text = actualQuest.texts.textsArray[0];
            Destroy(this.gameObject);
        }
    }
}
