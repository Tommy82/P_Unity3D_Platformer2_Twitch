using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    /// <summary>Variablen, welche dem aktuellem GameObject zugewiesen sind</summary>
    protected ObjectController objC;
    /// <summary>Angriffscontroller</summary>
    protected AttackController attackC;
    /// <summary>Gesundheits- und Sterbecontroller</summary>
    protected HealthController healthC;

    /// <summary>
    /// Wird nur einmal beim Start bzw. beim Instanzieren des GameObjects ausgeführt
    /// Achtung! Bei Abhängigkeiten MUSS "base.Start()" ausgeführt werden!
    /// </summary>
    protected void Start()
    {
        #region Laden der benötigten Controller
        this.objC = gameObject.GetComponent<ObjectController>();                            // Variablen welche über den inspektor zugewiesen sind
        this.attackC = gameObject.AddComponent<AttackController>() as AttackController;     // Hinzufügen des AngriffsControllers
        this.healthC = gameObject.AddComponent<HealthController>() as HealthController;     // Hinzufügen des Gesundheits- und SterbeControllers
        #endregion

    }

    /// <summary>
    /// Funktion wird zu einer bestimmten Zeit ausgeführt
    /// (ProjectSettings->Time->FixedTimeStep)
    /// Achtung! Bei Abhängigkeiten MUSS "base.FixedUpdate" ausgeführt werden!
    /// </summary>
    protected void FixedUpdate()
    {
        
        // Prüfe ob aktuelles GameObject auf dem Boden steht
        if (this.objC.groundCheck != null)
        {
            this.objC.isGrounded = Physics2D.OverlapCircle(this.objC.groundCheck.transform.position, 0.1f, this.objC.layerGroundOrWall);
        }

        // Setzen der Animationen
        if (this.objC._anim)
        {
            this.objC._anim.SetFloat("Speed", this.objC.currentSpeed);      // Geschwindigkeit -> Gehen oder Stehen
            this.objC._anim.SetBool("isJumping", !this.objC.isGrounded);    // Springen -> Springen oder Stehen
        }
    }

    /// <summary>
    /// Wird jeden Frame ausgeführt
    /// Achtung! Bei Abhängigkeiten MUSS "base.Update()" ausgeführt werden
    /// </summary>
    protected void Update()
    {
        
    }

    /// <summary>
    /// Drehe Character
    /// </summary>
    public void FlipChar()
    {
        this.objC.isLookRight = !this.objC.isLookRight;     // Wenn Character nach links schaut, drehe nach Rechts sonst drehe nach Links (Drehe nach !aktuellGedreht)
        Vector3 myScale = transform.localScale;             // Laden der Aktuellen Scale (da nicht direkt im Transform änderbar)
        myScale.x *= -1;                                    // Andern der Scale auf "x" Variable
        transform.localScale = myScale;                     // Zurückschreiben in GameObject.Transform
    }
}
