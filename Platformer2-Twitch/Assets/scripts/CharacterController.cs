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
    /// Wird nur einmal beim Start bzw. beim Instanzieren des GameObjects ausgef�hrt
    /// Achtung! Bei Abh�ngigkeiten MUSS "base.Start()" ausgef�hrt werden!
    /// </summary>
    protected void Start()
    {
        #region Laden der ben�tigten Controller
        this.objC = gameObject.GetComponent<ObjectController>();                            // Variablen welche �ber den inspektor zugewiesen sind
        this.attackC = gameObject.AddComponent<AttackController>() as AttackController;     // Hinzuf�gen des AngriffsControllers
        this.healthC = gameObject.AddComponent<HealthController>() as HealthController;     // Hinzuf�gen des Gesundheits- und SterbeControllers
        #endregion

    }

    /// <summary>
    /// Funktion wird zu einer bestimmten Zeit ausgef�hrt
    /// (ProjectSettings->Time->FixedTimeStep)
    /// Achtung! Bei Abh�ngigkeiten MUSS "base.FixedUpdate" ausgef�hrt werden!
    /// </summary>
    protected void FixedUpdate()
    {
        
        // Pr�fe ob aktuelles GameObject auf dem Boden steht
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
    /// Wird jeden Frame ausgef�hrt
    /// Achtung! Bei Abh�ngigkeiten MUSS "base.Update()" ausgef�hrt werden
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
        Vector3 myScale = transform.localScale;             // Laden der Aktuellen Scale (da nicht direkt im Transform �nderbar)
        myScale.x *= -1;                                    // Andern der Scale auf "x" Variable
        transform.localScale = myScale;                     // Zur�ckschreiben in GameObject.Transform
    }
}
