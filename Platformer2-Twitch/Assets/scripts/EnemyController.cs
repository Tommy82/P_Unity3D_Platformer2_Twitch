using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterController
{

    /// <summary>
    /// Wird nur einmal beim Start bzw. beim Instanzieren des GameObjects ausgeführt
    /// Achtung! Bei Abhängigkeiten MUSS "base.Start()" ausgeführt werden!
    /// </summary>
    private new void Start()
    {
        base.Start();                       // Ausführen des "Parent" Start()
        gameObject.tag = "enemy";           // Setzen des "tag" für aktuelles GameObject
    }

    /// <summary>
    /// Funktion wird zu einer bestimmten Zeit ausgeführt
    /// (ProjectSettings->Time->FixedTimeStep)
    /// Achtung! Bei Abhängigkeiten MUSS "base.FixedUpdate" ausgeführt werden!
    /// </summary>
    private new void FixedUpdate()
    {
        base.FixedUpdate();     // Ausführen der "Parent" FixedUpdate() Methode
        Enemy_Walk();           // Bewegung des Characters
        CheckAttackRange();     // Prüfen der Angriffsreichweite
    }

    /// <summary>
    /// Prüfen ob anderes Objekt in Angriffsreichweite
    /// </summary>
    void CheckAttackRange()
    {
        // Prüfen ob aktueller ObejctType ein "Gegner" ist
        if ( this.objC.objectType == ObjectController.ObjectType.Enemy ) { 
            // Prüfen ob Waffen vorhanden sind
            if (this.objC.weapon1_active && this.objC.weapon1 && this.objC.weapon1_spawnPoint)
            {
                // Erzeuge ein Raycast (Linie welche auf andere Collider reagiert)
                RaycastHit2D hit = Physics2D.Raycast(this.objC.weapon1_spawnPoint.transform.position, (this.objC.isLookRight ? Vector2.right : Vector2.left), this.objC.weapon1_range);
                // Wenn Raycast (Linie) einen anderen Collider getroffen hat UND andere Collider ein Player ist UND Attacke1 erlaubt ist ...
                if (hit.collider != null && hit.collider.tag == "Player" && this.objC.isAttacking1 == false)
                {
                    // Setze Attacke1 (wird im AttackController:FixedUpdate ausgeführt)
                    this.objC.isAttacking1 = true;
                    // Setze aktuelle Waffe als NICHT aktiv
                    this.objC.weapon1_active = false;
                    // Warte bis Waffe wieder verfügbar
                    Invoke("Weapon1SetActive", this.objC.weapon1_delay);
                }

                // DebugMode
                if (this.objC.isDebugMode)
                {
                    Vector3 directionDebug = transform.TransformDirection(this.objC.isLookRight ? Vector2.right : Vector2.left) * this.objC.weapon1_range;
                    Debug.DrawRay(this.objC.weapon1_spawnPoint.transform.position, directionDebug, Color.red);
                }

            }
        }
    }

    /// <summary>
    /// Setze Waffe1 wieder als Verfügbar
    /// </summary>
    void Weapon1SetActive()
    {
        this.objC.weapon1_active = true;
    }

    /// <summary>
    /// Gehen des Gegners (KI)
    /// </summary>
    void Enemy_Walk()
    {

        Vector2 raycastPosition = transform.position;       // Lade aktuelle Position in Zwischenspeicher
        Vector2 raycastDirection = Vector2.down;            // Richtung des Raycast für Gehen
        float raycastDistance = 0.6f;                       // Länge des Raycast für Gehen

        if (this.objC.isLookRight)                          // Wenn Char nach Rechts schaut ...
        {
            raycastPosition.x += 0.5f;                      // Setze Raycast 0.5 Elemente rechts vom aktuellen GameObject (transform.position.x += 5)
            this.objC.speed = Mathf.Abs(this.objC.speed);   // Setze Geschwindigkeit Positive (>= 0) (Bewegung nach Rechts)
        }
        else                                                // Wenn Char nach Links schaut ...
        {
            raycastPosition.x -= 0.5f;                          // ... Setze Raycast 0.5 Einheiten links neben GameObject (transform.position.x -= 0.5)
            this.objC.speed = Mathf.Abs(this.objC.speed) * -1;  // ... Setze Geschwindigkeit ins Negative (<= 0) (Bewegung nach Links)
        }

        RaycastHit2D hit = Physics2D.Raycast(raycastPosition, raycastDirection, raycastDistance);   // Erzeuge Raycast neben GameObject
        
        if (hit.collider == null)           // Wenn Raycast (Linie) etwas trifft ...
        {
            FlipChar();                     // ... Drehe GameObject
        }

        this.objC._rb2d.velocity = new Vector2(objC.speed, this.objC._rb2d.velocity.y); // Setze GameObject in Bewegung

        this.objC.currentSpeed = Mathf.Abs(this.objC.speed);                            // Setze aktuelle Geschwindigkeit global (z.B. für Animator)

        // DebugMode
        if (this.objC.isDebugMode)
        {
            Vector3 directionDebug = transform.TransformDirection(Vector3.down) * raycastDistance;
            Debug.DrawRay(raycastPosition, directionDebug, Color.red);
        }

    }

}
