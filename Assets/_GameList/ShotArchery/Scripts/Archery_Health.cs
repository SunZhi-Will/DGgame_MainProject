using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archery_Health : MonoBehaviour
{
    public  float maxHealth;
    public float health;

    [SerializeField] Archery_Score score;
    
    public type m_type;
    public enum type { Player,Boss}

    public Archery_Player player;
    public Archery_Boss boss;
    public Archery_Item item;
    
    void Start()
    {
        health = maxHealth;
    }

    public void HealthChange(float healthChange)
    {
        health += healthChange;
        health = Mathf.Clamp(health,0,maxHealth);

        score.score();
    }
}
