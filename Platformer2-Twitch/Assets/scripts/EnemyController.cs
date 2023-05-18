using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterController
{

    private new void Start()
    {
        base.Start();
        gameObject.tag = "enemy";
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();

        Enemy_Walk();

        CheckAttackRange();
    }

    /// <summary>
    /// Funktion wird NUR bei Enemy ausgeführt !!!
    /// </summary>
    void CheckAttackRange()
    {
        if (this.objC.weapon1_active && this.objC.weapon1 && this.objC.weapon1_spawnPoint)
        {
            RaycastHit2D hit = Physics2D.Raycast(this.objC.weapon1_spawnPoint.transform.position, (this.objC.isLookRight ? Vector2.right : Vector2.left), this.objC.weapon1_range);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Player" && this.objC.isAttacking1 == false)
                {
                    this.objC.isAttacking1 = true;
                    this.objC.weapon1_active = false;
                    Invoke("Weapon1SetActive", this.objC.weapon1_delay);

                }
            }
        }
    }


    void Weapon1SetActive()
    {
        this.objC.weapon1_active = true;
    }

    void Enemy_Walk()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 0.6f;

        if (this.objC.isLookRight)
        {
            position.x += 0.5f;
            this.objC.speed = Mathf.Abs(this.objC.speed);
        }
        else
        {
            position.x -= 0.5f;
            this.objC.speed = Mathf.Abs(this.objC.speed) * -1;
        }

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance);
        //Vector3 directionDebug = transform.TransformDirection(Vector3.down) * distance;
        //Debug.DrawRay(position, directionDebug, Color.red);
        if (hit.collider == null)
        {
            FlipChar();
        }
        this.objC._rb2d.velocity = new Vector2(objC.speed, this.objC._rb2d.velocity.y);

        // Setze aktuelle Geschwindigkeit (z.B. für Animator)
        this.objC.currentSpeed = Mathf.Abs(this.objC.speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hit");
        /*
        if (this.objectType == ObjectType.Enemy)
        {
            ObjectController objectController = collision.GetComponent<ObjectController>();
            if (objectController && objectController.objectType == ObjectType.Player)
            {
                // Ziehe getroffenem Leben ab
                objectController.health -= damageOnEnter;
            }
        }
        */
    }

}
