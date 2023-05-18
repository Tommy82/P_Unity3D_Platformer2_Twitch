using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    protected ObjectController objC;

    // Start is called before the first frame update
    void Start()
    {
        this.objC = gameObject.GetComponent<ObjectController>();
    }

    private void FixedUpdate()
    {
        if (this.objC.isAttacking1)
        {
            Attack1();
            this.objC.isAttacking1 = false;
        }

    }


    void Attack1()
    {
        if (this.objC.weapon1 && this.objC.weapon1_throwable)
        {
            // Waffe im Spiel instanzieren
            GameObject myWeapon = (GameObject)Instantiate(this.objC.weapon1, this.objC.weapon1_spawnPoint.position, Quaternion.identity);
            if (myWeapon)
            {
                Weapon myWeaponScript = myWeapon.GetComponent<Weapon>();
                if (myWeaponScript)
                {
                    myWeaponScript.damage = objC.weapon1_damage;
                    switch (objC.objectType)
                    {
                        case ObjectController.ObjectType.Enemy:
                            myWeaponScript.canDamage_Enemy = false;
                            myWeaponScript.canDamage_Player = true;
                            break;
                        case ObjectController.ObjectType.Player:
                            myWeaponScript.canDamage_Player = false;
                            myWeaponScript.canDamage_Enemy = true;
                            break;
                    }
                }

                myWeapon.tag = "weapon";
                // Waffe in passende Richtung drehen (je nachdem wohin Char gerade schaut)
                var myScale = myWeapon.transform.localScale;
                if (this.objC.isLookRight)
                {
                    myWeapon.GetComponent<Rigidbody2D>().AddForce(Vector3.right * this.objC.weapon1_speed);
                }
                if (!this.objC.isLookRight)
                {
                    myWeapon.GetComponent<Rigidbody2D>().AddForce(Vector3.left * this.objC.weapon1_speed);
                    myScale.x = myScale.x * -1;
                }
                myWeapon.transform.localScale = myScale;
            }
        }
    }
}
