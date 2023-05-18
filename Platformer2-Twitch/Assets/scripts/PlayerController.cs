using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{

    private new void Start()
    {
        base.Start();
        gameObject.tag = "Player";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && this.objC.isGrounded)
        {
            this.objC.isJumping = true;
        }

        if (Input.GetButtonDown("Fire1") && !this.objC.isAttacking1)
        {
            this.objC.isAttacking1 = true;
        }

    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();

        // Bewegung (Links / Rechts)
        float hor = Input.GetAxis("Horizontal");
        this.objC._rb2d.velocity = new Vector2(hor * this.objC.speed, this.objC._rb2d.velocity.y);

        // Springen
        if (this.objC.isJumping)
        {
            this.objC._rb2d.AddForce(new Vector2(0, this.objC.jumpForce));
            this.objC.isJumping = false;
        }

        // Setze aktuelle Geschwindigkeit (z.B. für Animator)
        this.objC.currentSpeed = Mathf.Abs(hor);

        // Character bei bedarf drehen
        if ((hor > 0 && !this.objC.isLookRight)          // Wenn Horizontale Eingabe > 0 UND Spieler schaut nach links ...
             || (hor < 0 && this.objC.isLookRight)       // Wenn Horizontale Eingabe < 0 UND Spieler schaut nach rechts ...
            )
        {
            FlipChar();
        }
    }
}
