using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] CharacterStats charstats;
    [SerializeField] Image[] bars;

    void Start()
    {
        
    }

    void Update()
    {
        foreach(Image b in bars)
        {
            b.fillAmount = charstats.currentHealth / charstats.maxHealth;
        }
    }
}
