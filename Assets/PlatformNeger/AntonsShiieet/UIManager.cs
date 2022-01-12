using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image healthBar;
    PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

    }

    // Update is called once per frame
    void Update()
    {
        float c_hp = player.currentHealth;
        float m_hp = player.maxHealth;

        healthBar.fillAmount = c_hp / m_hp;
    }
}
