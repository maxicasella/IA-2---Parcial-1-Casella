using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DetectEnemies : MonoBehaviour
{
    [SerializeField] GameObject _enemyUi;
    [SerializeField] SquareQuery _myQuery = null;
    [SerializeField] Image _lifeFill;

    void Update()
    {
        DetectEnemy();
    }

    void DetectEnemy() //IA2-P2
    {
        var nearestEnemy = _myQuery.Query().OfType<Enemy>().OrderBy(x => Vector3.Distance(x.Position, _myQuery.transform.position)).FirstOrDefault();

        if (nearestEnemy != null)
        {
            _enemyUi.SetActive(true);
            UpdateLifeBar(nearestEnemy.UpdateUILife());
        }
        else _enemyUi.SetActive(false);
    }

    void UpdateLifeBar(float amount)
    {
        _lifeFill.fillAmount = amount;
    }
}
