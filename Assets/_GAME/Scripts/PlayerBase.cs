using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public Player Player;
    public int Health;
    public TextMeshProUGUI HealthText;
    public bool stopGame;

    private void Start()
    {
        UpdateHealth();
    }

    public void UpdateHealth() => HealthText.text = "Health:" + Health.ToString();

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && !stopGame)
        {
            other.GetComponent<Enemy>().Despawn();
            Health--;
            UpdateHealth();
            if(Health <= 0)
            {
                stopGame = true;
            }
        }
            
    }
}
