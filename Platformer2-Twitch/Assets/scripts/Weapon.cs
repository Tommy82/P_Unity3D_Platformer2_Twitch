using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Tooltip("Schaden welchen diese Waffe verursacht")]
    public float damage = 1.0f;
    [Tooltip("Waffe kann Spieler verletzen")]
    public bool canDamage_Player = false;
    [Tooltip("Waffe kann Gegner verletzen")]
    public bool canDamage_Enemy = false;
    [Tooltip("Waffe kann Wände zerstören")]
    public bool canDamage_Wall = false;
    [Tooltip("Waffe zerstört sich selbst bei Treffer")]
    public bool destroyOnTrigger = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ObjectController objectController = collision.GetComponent<ObjectController>();
        if ( objectController )
        {
            if ( 
                canDamage_Enemy && objectController.objectType == ObjectController.ObjectType.Enemy 
                || canDamage_Player && objectController.objectType == ObjectController.ObjectType.Player
                || canDamage_Wall && objectController.objectType == ObjectController.ObjectType.Wall
                )
            {
                // Ziehe getroffenem Leben ab
                objectController.health -= damage;

                // Zerstöre Waffe
                if ( destroyOnTrigger )
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
