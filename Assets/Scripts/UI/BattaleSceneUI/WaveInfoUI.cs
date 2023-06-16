using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveInfoUI : MonoBehaviour
{
    private void Update()
    {
        GetComponent<TMP_Text>().text = GameManager.Battle.curentWave.ToString();
        //GetComponent<TMP_Text>().text = BattleManager.Instance.curentWave.ToString();
    }
}
