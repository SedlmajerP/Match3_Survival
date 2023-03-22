using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WavesText : MonoBehaviour
{
    [SerializeField] private Text wavesText;


    public void UpdateWavesText()
    {
        wavesText.text = $"Wave: {GameManager.Instance.numWaves}";
    }
}
