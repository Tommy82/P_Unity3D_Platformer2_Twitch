using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3.0f;
    public float jumpForce = 500.0f;
    public float gravity = 3;

    private Rigidbody2D _rb2d;
    private bool lookRight = false;

    // Start is called before the first frame update
    void Start()
    {
        this._rb2d = GetComponent<Rigidbody2D>();        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float hor = Input.GetAxis("Horizontal");
        this._rb2d.velocity = new Vector2(hor * this.speed, this._rb2d.velocity.y);

        // FlipChar (?)
        if ( ( hor > 0 && this.lookRight)          // Wenn Horizontale Eingabe > 0 UND Spieler schaut nach links ...
             || ( hor < 0 && !this.lookRight )       // Wenn Horizontale Eingabe < 0 UND Spieler schaut nach rechts ...
            )
        {
            this.lookRight = !this.lookRight;
            Vector3 myScale = transform.localScale;
            myScale.x *= -1;
            transform.localScale = myScale;
        }
    }

    
}
