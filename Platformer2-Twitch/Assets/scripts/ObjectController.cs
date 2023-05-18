using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectController : MonoBehaviour
{
    [HideInInspector]
    public enum ObjectType
    {
        NOTSET,
        Player,
        Enemy,
        Wall,
    }

    [Tooltip("Art des Objektes")]
    public ObjectType objectType;
    [Tooltip("Geschwindigkeit des Objektes")]
    public float speed = 2.0f;
    [Tooltip("Sprungkraft des Objektes")]
    public float jumpForce = 500.0f;
    [Tooltip("Spielerschaden bei Berührung")]
    public float damageOnEnter = 1.0f;

    [Tooltip("Waffe 1")]
    public GameObject weapon1;
    [Tooltip("Geschwindigkeit der Waffe")]
    public float weapon1_speed = 250.0f;
    [Tooltip("Schaden der Waffe 1")]
    public float weapon1_damage = 1.0f;
    [Tooltip("Spawnpoint der Waffe 1")]
    public Transform weapon1_spawnPoint;
    [Tooltip("Ist die Waffe 1 eine Wurfwaffe?")]
    public bool weapon1_throwable;
    [Tooltip("Angriffsreichweite für Waffe 1")]
    public float weapon1_range = 100.0f;
    [Tooltip("Zeit bis nächster Schuss möglich")]
    public float weapon1_delay = 3;
    // Aktueller Waffenstatus
    [HideInInspector]
    public bool weapon1_active = true;
    /// <summary>Gibt an ob der Spieler gerade die erste Attacke ausführt</summary>
    [HideInInspector]
    public bool isAttacking1 = false;

    [Tooltip("Maximale Leben / Aktuelle Leben")]
    public float health = 5.0f;
    [Tooltip("Lebensbalken (Canvas Slider)")]
    public Slider healthBar;
    [Tooltip("Leeres Gameobject zum prüfen ob auf Boden")]
    public GameObject groundCheck;
    [Tooltip("Layer für Wand und Boden")]
    public LayerMask layerGroundOrWall;

    ///<summary>Physik - Rigidbody2D</summary>
    [HideInInspector]
    public Rigidbody2D _rb2d;
    /// <summary>Animator des Charakters</summary>
    [HideInInspector]
    public Animator _anim;
    /// <summary>Gibt an ob der Spieler gerade nach Rechts (true) oder Links (false) schaut</summary>
    [HideInInspector]
    public bool isLookRight = true;
    /// <summary>Gibt an ob der Spieler gerade springt</summary>
    [HideInInspector]
    public bool isJumping = false;
    /// <summary>Gibt an ob der Spieler sich gerade auf dem Boden befindet (true) oder Springt (false)</summary>
    public bool isGrounded = false;
    /// <summary>Aktuelle Geschwindigkeit des Characters</summary>
    public float currentSpeed = 0;

    public EnemyController enemyController;
    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        // Eigene Components einbinden
        this._rb2d = GetComponent<Rigidbody2D>();
        this._anim = GetComponent<Animator>();

        // Setze Tag
        switch ( this.objectType )
        {
            case ObjectType.Player:
                playerController = gameObject.AddComponent<PlayerController>() as PlayerController;
                break;
            case ObjectType.Enemy:
                enemyController = gameObject.AddComponent<EnemyController>() as EnemyController;
                break;
            default:
                break;
        }

    }
}
