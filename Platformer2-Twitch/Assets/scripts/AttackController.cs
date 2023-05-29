using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    /// <summary>Variablen, welche dem aktuellem GameObject zugewiesen sind</summary>
    protected ObjectController objC;

    /// <summary>
    /// Wird nur einmal beim Start bzw. beim Instanzieren des GameObjects ausgef�hrt
    /// Achtung! Bei Abh�ngigkeiten MUSS "base.Start()" ausgef�hrt werden!
    /// </summary>
    void Start()
    {
        #region Laden der ben�tigten Controller
        this.objC = gameObject.GetComponent<ObjectController>();
        #endregion Laden der ben�tigten Controller
    }

    /// <summary>
    /// Funktion wird zu einer bestimmten Zeit ausgef�hrt
    /// (ProjectSettings->Time->FixedTimeStep)
    /// Achtung! Bei Abh�ngigkeiten MUSS "base.FixedUpdate" ausgef�hrt werden!
    /// </summary>
    private void FixedUpdate()
    {
        if (this.objC.canAttack && this.objC.isAttacking1)             // Wenn Variable f�r Anfriff1 gesetzt ...
        {
            Attack1();                          // ... f�hre Angriff1 aus
            this.objC.isAttacking1 = false;     // ... Setze Variable f�r Angriff zur�ck
        }

    }

    /// <summary>
    /// Ausf�hrung des Angriff1
    /// ToDo: Waffe nach gewisser Zeit / Laufzeit zerst�ren
    /// </summary>
    void Attack1()
    {
        if (objC.isDebugMode) { Debug.Log("Start Attack 1"); }

        // Wenn Waffe vorhanden UND Waffe eine Wurfwaffe (Fernwaffe) ...
        if (this.objC.weapon1 && this.objC.weapon1Throwable)
        {
            // ... Instanzieren der Waffe im Spiel
            GameObject myWeapon = (GameObject)Instantiate(this.objC.weapon1, this.objC.weapon1SpawnPoint.position, Quaternion.identity);
            
            if (myWeapon)   // Wenn Waffe vorhanden und instanziert ....
            {
                Weapon myWeaponScript = myWeapon.GetComponent<Weapon>();    // ... Lade Waffenscript von Waffe
                if (myWeaponScript)                                         // ... Wenn Waffenscript vorhanden ...
                {
                    myWeaponScript.damage = objC.weapon1Damage;            // ... ... setze Schaden der Waffe
                    switch (objC.objectType)
                    {
                        case ObjectController.ObjectType.Enemy:             // ... ... Wenn aktueller Type ein Gegner ...
                            myWeaponScript.canDamage_Enemy = false;         // ... ... ... ignoriere Schaden an anderen gegnern
                            myWeaponScript.canDamage_Player = true;         // ... ... ... erlaube Schaden bei Spieler
                            break;
                        case ObjectController.ObjectType.Player:            // ... ... Wenn aktueller Type ein Player ...
                            myWeaponScript.canDamage_Player = false;        // ... ... ... ignoriere Schaden anderen Playern
                            myWeaponScript.canDamage_Enemy = true;          // ... ... ... erlaube Schaden bei Gegnern
                            break;
                    }
                }

                myWeapon.tag = "weapon";                                    // ... Setze Tag auf Waffe damit andere Collider drauf reagieren k�nnen

                #region  Waffe in passende Richtung drehen (je nachdem wohin Char gerade schaut)
                var myScale = myWeapon.transform.localScale;            // ... lade aktuelle Scale des GameObjects
                if (this.objC.isLookRight)                              // ... Wenn Char nach rechts schaut ....
                {
                    myWeapon.GetComponent<Rigidbody2D>().AddForce(Vector3.right * this.objC.weapon1Speed); // ... ... setze eine Physikalische Kraft auf Waffe nach Rechts
                }
                if (!this.objC.isLookRight)                             // ... Wenn Char nach Links schaut ...
                {
                    myWeapon.GetComponent<Rigidbody2D>().AddForce(Vector3.left * this.objC.weapon1Speed);  // ... ... setze eine Physikalische Kraft auf Waffe nach Links
                    myScale.x = myScale.x * -1;                         // Setze X Richtung der Waffe (Achtung! Waffe MUSS standardm��ig nach rechts ausgerichtet sein!!!)
                }
                myWeapon.transform.localScale = myScale;                // ... Setze Richtung (Scale) der Waffe
                #endregion
            }
        }

        // ToDo: Keine Wurfwaffe (z.B. Schwert) einbauen
    }
}
