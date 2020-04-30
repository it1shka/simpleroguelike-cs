using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DeathDetecter : MonoBehaviour
{
    private GameObject player;
    public GameObject main, pause;
    public Button resume;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (!player) Activate();
    }

    void Activate()
    {
        Time.timeScale = 0f;
        resume.interactable = false;
        main.SetActive(false);
        pause.SetActive(true);
        Destroy(gameObject);
    }
}
