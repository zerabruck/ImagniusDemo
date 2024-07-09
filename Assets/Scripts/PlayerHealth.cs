using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]private ManaMeter healthMeter;
    public float currentMana { get; set; } 
    public float maxMana = 80; 
    void Start()
    {
        currentMana = maxMana;
        healthMeter.SetMaxMana(maxMana);
    }

    public void ManaHeal(float _heal){
        currentMana = Mathf.Clamp(currentMana + _heal, 0, maxMana);
        healthMeter.SetMana(currentMana);
    }
    public void ManaDamage(float _damage){
        currentMana = Mathf.Clamp(currentMana - _damage, 0, maxMana);
        healthMeter.SetMana(currentMana);
    }
}
