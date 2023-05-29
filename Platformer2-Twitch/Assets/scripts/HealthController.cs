using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{
    /// <summary>Variablen, welche dem aktuellem GameObject zugewiesen sind</summary>
    protected ObjectController objC;

    /// <summary>
    /// Wird nur einmal beim Start bzw. beim Instanzieren des GameObjects ausgeführt
    /// Achtung! Bei Abhängigkeiten MUSS "base.Start()" ausgeführt werden!
    /// </summary>
    protected void Start()
    {
        #region Laden der benötigten Controller
        this.objC = gameObject.GetComponent<ObjectController>();                            // Variablen welche über den inspektor zugewiesen sind
        #endregion

        #region Setzen der Standardparamter für Health Bar
        if (this.objC.healthBar)                                // Wenn Healthbar vorhanden ist ...
        {       
            this.objC.healthBar.maxValue = this.objC.health;    // ... setze maximales Leben
            this.objC.healthBar.value = this.objC.health;       // ... setze aktuelles Leben
        }
        #endregion Setzen der Standardparameter für HealthBar
    }

    /// <summary>
    /// Funktion wird zu einer bestimmten Zeit ausgeführt
    /// (ProjectSettings->Time->FixedTimeStep)
    /// Achtung! Bei Abhängigkeiten MUSS "base.FixedUpdate" ausgeführt werden!
    /// </summary>
    private void FixedUpdate()
    {
        this.CheckHealth();     // Prüfe aktuelle Gesundheit und Healthbar
    }

    /// <summary>
    /// Prüfe die aktuelle Gesundheit
    /// </summary>
    void CheckHealth()
    {
        if (this.objC.healthBar)                            // Wenn Healthbar vorhandne ist ...
        {
            this.objC.healthBar.value = this.objC.health;   // Setze aktuellen Status mit aktuellem Leben aus ObjectController
        }

        if (this.objC.health <= 0)                          // Wenn aktuelles Leben <= 0 ...
        {
            Die();                                          // ... führe Sterbefunktion aus
        }
    }

    /// <summary>
    /// Sterbefunktion
    /// </summary>
    void Die()
    {

        objC._anim.SetBool("isDying", true);    // Setze Animation
        objC.speed = 0;                         // Setze aktuelle Geschwindigkeit auf 0
        objC.healthBar.enabled = false;         // Deaktiviere Healthbar

        if (objC.objectType != ObjectController.ObjectType.Player)      // Wenn nicht Player ...
        {
            // Wenn nicht Spieler
            objC.DestroyObject(objC.destroyAfterTime, false);
        } else
        {
            // Wenn Spieler
            Invoke("ReloadLevel", objC.destroyAfterTime);
        }
    }

    void ReloadLevel()
    {
        // Reset Animation
        objC._anim.SetBool("isDying", false);

        // Reload Level / ToDo: Seed aus automatischer Levelgenerierung berücksichtigen
        Scene current = SceneManager.GetActiveScene();  // Lade aktuelle Scene
        SceneManager.LoadScene(current.name);           // Starte Level neu / ToDo: Automatische Levelgenerierung (seed) hinzufügen!
    }
}


