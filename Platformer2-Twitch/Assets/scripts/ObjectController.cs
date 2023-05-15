using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    [HideInInspector]
    public enum ObjectType
    {
        NOTSET,
        Player,
        Enemy
    }

    [Tooltip("Art des Objektes")]
    public ObjectType objectType;
    [Tooltip("Geschwindigkeit des Objektes")]
    public float speed = 2.0f;
    [Tooltip("Sprungkraft des Objektes")]
    public float jumpForce = 500.0f;
    
    public GameObject groundCheck;
    public LayerMask layerGroundOrWall;

    // Rigidbody2D
    private Rigidbody2D _rb2d;
    // Animator
    private Animator _anim;
    // Schaut der Char gerade nach rechts ?
    private bool isLookRight = true;
    // Springt der Char gerade ?
    private bool isJumping = false;
    public bool isGrounded = false;
    public float currentSpeed = 0;


    // Start is called before the first frame update
    void Start()
    {
        this._rb2d = GetComponent<Rigidbody2D>();
        this._anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( objectType == ObjectType.Player )
        {
            if (Input.GetButtonDown("Jump") && this.isGrounded)
            {
                this.isJumping = true;
            }
        }
    }

    private void FixedUpdate()
    {
        switch ( this.objectType )
        {
            case ObjectType.Enemy:
                FixedUpdate_Enemy();
                break;
            case ObjectType.Player:
                FixedUpdate_Player();
                break;
        }        

        if ( this.groundCheck != null )
        {
            this.isGrounded = Physics2D.OverlapCircle(this.groundCheck.transform.position, 0.1f, layerGroundOrWall);
        }

        if ( this._anim )
        {
            Debug.Log("Speed: " + currentSpeed);
            this._anim.SetFloat("Speed", currentSpeed);
            this._anim.SetBool("isJumping", !isGrounded);

        }
    }


    /// <summary>
    /// Drehe Character
    /// </summary>
    void FlipChar()
    {
        this.isLookRight = !this.isLookRight;
        Vector3 myScale = transform.localScale;
        myScale.x *= -1;
        transform.localScale = myScale;
    }

    #region Method - Enemy
    void FixedUpdate_Enemy()
    {
        Enemy_Walk();
    }

    void Enemy_Walk()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 0.6f;

        if (this.isLookRight)
        {
            position.x += 0.5f;
            this.speed = Mathf.Abs(this.speed);
        }
        else
        {
            position.x -= 0.5f;
            this.speed = Mathf.Abs(this.speed) * -1;
        }

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance);
        //Vector3 directionDebug = transform.TransformDirection(Vector3.down) * distance;
        //Debug.DrawRay(position, directionDebug, Color.red);
        if (hit.collider == null)
        {
            FlipChar();
        }
        this._rb2d.velocity = new Vector2(speed, this._rb2d.velocity.y);

        // Setze aktuelle Geschwindigkeit (z.B. für Animator)
        this.currentSpeed = Mathf.Abs(this.speed);
    }
    #endregion Method - Enemy

    #region Method - Player
    void FixedUpdate_Player()
    {
        // Bewegung (Links / Rechts)
        float hor = Input.GetAxis("Horizontal");
        this._rb2d.velocity = new Vector2(hor * this.speed, this._rb2d.velocity.y);

        // Springen
        if (isJumping)
        {
            Debug.Log("Springen: " + this.isJumping.ToString());
            this._rb2d.AddForce(new Vector2(0, this.jumpForce));
            this.isJumping = false;
        }

        // Setze aktuelle Geschwindigkeit (z.B. für Animator)
        this.currentSpeed = Mathf.Abs(hor);

        // Character bei bedarf drehen
        if ((hor > 0 && !this.isLookRight)          // Wenn Horizontale Eingabe > 0 UND Spieler schaut nach links ...
             || (hor < 0 && this.isLookRight)       // Wenn Horizontale Eingabe < 0 UND Spieler schaut nach rechts ...
            )
        {
            FlipChar();
        }
    }

    #endregion Method - Player
}
