using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    protected ObjectController objC;
    protected AttackController attackC;
    protected HealthController healthC;

    protected void Start()
    {
        this.objC = gameObject.GetComponent<ObjectController>();

        this.attackC = gameObject.AddComponent<AttackController>() as AttackController;
        this.healthC = gameObject.AddComponent<HealthController>() as HealthController;

    }

    protected void FixedUpdate()
    {
        Debug.Log("Character - FixedUpdate");

        if (this.objC.groundCheck != null)
        {
            this.objC.isGrounded = Physics2D.OverlapCircle(this.objC.groundCheck.transform.position, 0.1f, this.objC.layerGroundOrWall);
        }

        if (this.objC._anim)
        {
            this.objC._anim.SetFloat("Speed", this.objC.currentSpeed);
            this.objC._anim.SetBool("isJumping", !this.objC.isGrounded);
        }
    }

    /// <summary>
    /// Drehe Character
    /// </summary>
    public void FlipChar()
    {
        this.objC.isLookRight = !this.objC.isLookRight;
        Vector3 myScale = transform.localScale;
        myScale.x *= -1;
        transform.localScale = myScale;
    }



}
