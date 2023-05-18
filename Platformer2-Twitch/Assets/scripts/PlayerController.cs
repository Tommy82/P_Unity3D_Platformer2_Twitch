using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{
    /// <summary>
    /// Wird nur einmal beim Start bzw. beim Instanzieren des GameObjects ausgef�hrt
    /// Achtung! Bei Abh�ngigkeiten MUSS "base.Start()" ausgef�hrt werden!
    /// </summary>
    private new void Start()
    {
        base.Start();                       // Ausf�hren des "Parent" Start()
        gameObject.tag = "Player";          // Setzen des "tag" f�r aktuelles GameObject
    }

    /// <summary>
    /// Wird bei jedem Frame ausgef�hrt
    /// Achtung! Bei Abh�ngigkeiten MUSS "base.Update()" ausgef�hrt werden!
    /// </summary>
    new void Update()
    {
        base.Update();          // Ausf�hren des "Parent" Udpate()

        // Wenn "Springen"-Taste gedr�ckt wird (default: Space) UND der Spieler sich nicht auf dem Boden befindet ...
        if (Input.GetButtonDown("Jump") && this.objC.isGrounded)
        {
            // ... setze Variable f�r Springen (Sprunganimation wird in FixedUpdate ausgef�hrt)
            this.objC.isJumping = true;
        }

        // Wenn "Angriff1"-Taste gedr�ckt wird (default left ctrl) UND Spieler gerade nicht angreift ...
        if (Input.GetButtonDown("Fire1") && !this.objC.isAttacking1)
        {
            // ... setze Variable f�r Angriff1 (Angriff wird in FixedUpdate ausgef�hrt)
            this.objC.isAttacking1 = true;
        }

    }

    /// <summary>
    /// Funktion wird zu einer bestimmten Zeit ausgef�hrt
    /// (ProjectSettings->Time->FixedTimeStep)
    /// Achtung! Bei Abh�ngigkeiten MUSS "base.FixedUpdate" ausgef�hrt werden!
    /// </summary>
    private new void FixedUpdate()
    {
        base.FixedUpdate();     // Ausf�hren der "Parent" FixedUpdate() Methode

        #region Bewegung Links / Rechts
        float hor = Input.GetAxis("Horizontal");        // Aktuelle Tastenstellung / Tastatureingabe
        this.objC._rb2d.velocity = new Vector2(hor * this.objC.speed, this.objC._rb2d.velocity.y); // Bewegung des Characters

        this.objC.currentSpeed = Mathf.Abs(hor);         // Setze aktuelle Geschwindigkeit (z.B. f�r Animator)

        // Character bei Bedarf drehen
        if ((hor > 0 && !this.objC.isLookRight)          // Wenn Horizontale Eingabe > 0 UND Spieler schaut nach links ...
             || (hor < 0 && this.objC.isLookRight)       // Wenn Horizontale Eingabe < 0 UND Spieler schaut nach rechts ...
            )
        {
            FlipChar();                                 // ... drehe Character
        }

        #endregion Bewegung Links / Rechts

        #region Springen
        if (this.objC.isJumping)                                                    // Wenn �ber Update Sprungvariable gesetzt ...
        {
            this.objC._rb2d.AddForce(new Vector2(x: 0, y: this.objC.jumpForce));    // Gebe eine Physikalische Kraft (JumpForce) auf Rigidbody (y Kooridiate / x bleibt 0)
            this.objC.isJumping = false;                                            // Reset der Sprungvariable
        }
        #endregion Springen

    }
}
