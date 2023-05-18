using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    /// <summary>Variablen, welche dem aktuellem GameObject zugewiesen sind</summary>
    protected ObjectController objC;

    /// <summary>
    /// Wird nur einmal beim Start bzw. beim Instanzieren des GameObjects ausgef�hrt
    /// Achtung! Bei Abh�ngigkeiten MUSS "base.Start()" ausgef�hrt werden!
    /// </summary>
    protected void Start()
    {
        #region Laden der ben�tigten Controller
        this.objC = gameObject.GetComponent<ObjectController>();                            // Variablen welche �ber den inspektor zugewiesen sind
        #endregion

        #region Setzen der Standardparamter f�r Health Bar
        if (this.objC.healthBar)                                // Wenn Healthbar vorhanden ist ...
        {       
            this.objC.healthBar.maxValue = this.objC.health;    // ... setze maximales Leben
            this.objC.healthBar.value = this.objC.health;       // ... setze aktuelles Leben
        }
        #endregion Setzen der Standardparameter f�r HealthBar
    }

    /// <summary>
    /// Funktion wird zu einer bestimmten Zeit ausgef�hrt
    /// (ProjectSettings->Time->FixedTimeStep)
    /// Achtung! Bei Abh�ngigkeiten MUSS "base.FixedUpdate" ausgef�hrt werden!
    /// </summary>
    private void FixedUpdate()
    {
        this.CheckHealth();     // Pr�fe aktuelle Gesundheit und Healthbar
    }

    /// <summary>
    /// Pr�fe die aktuelle Gesundheit
    /// </summary>
    void CheckHealth()
    {
        if (this.objC.healthBar)                            // Wenn Healthbar vorhandne ist ...
        {
            this.objC.healthBar.value = this.objC.health;   // Setze aktuellen Status mit aktuellem Leben aus ObjectController
        }

        if (this.objC.health <= 0)                          // Wenn aktuelles Leben <= 0 ...
        {
            Die();                                          // ... f�hre Sterbefunktion aus
        }
    }

    /// <summary>
    /// Sterbefunktion
    /// </summary>
    void Die()
    {
        // ToDo: Starte "Dying" Animation
        // ToDo: Timer solange Dying animation l�uft

        if (objC.objectType != ObjectController.ObjectType.Player)      // Wenn nicht Player ...
        {
            Destroy(gameObject);                                        // ... zerst�re aktuelles Gameobject
        }
    }
}
