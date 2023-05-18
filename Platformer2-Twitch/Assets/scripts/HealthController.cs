using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    protected ObjectController objC;

    // Start is called before the first frame update
    void Start()
    {
        this.objC = gameObject.GetComponent<ObjectController>();

        // Setzen der HealthBar
        if (this.objC.healthBar)
        {
            this.objC.healthBar.maxValue = this.objC.health;
            this.objC.healthBar.value = this.objC.health;
        }
    }

    private void FixedUpdate()
    {
        this.CheckHealth();
    }

    /// <summary>
    /// Pr�fe die aktuelle Gesundheit
    /// </summary>
    void CheckHealth()
    {
        if (this.objC.healthBar)
        {
            this.objC.healthBar.value = this.objC.health;
        }

        if (this.objC.health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Stirb!
    /// </summary>
    void Die()
    {
        // Starte "Dying" Animation
        // Timer solange Dying animation l�uft

        // Wenn nicht Player, zerst�re aktuelles Gameobject
        if (objC.objectType != ObjectController.ObjectType.Player)
        {
            Destroy(gameObject);
        }
    }
}
