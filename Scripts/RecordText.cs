using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class RecordText : MonoBehaviour
{
    private TextMeshProUGUI tmpro;
    void Start()
    {
        tmpro = GetComponent<TextMeshProUGUI>();
        tmpro.text = $"You record is {PlayerPrefs.GetInt("record")} dungeons";
    }
}
