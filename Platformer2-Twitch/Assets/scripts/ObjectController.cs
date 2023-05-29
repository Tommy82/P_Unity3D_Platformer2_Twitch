using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectController : MonoBehaviour
{
    /// <summary>
    /// Art des Objektes
    /// </summary>
    [HideInInspector]
    public enum ObjectType
    {
        NOTSET,
        Player,
        Enemy,
        Wall,
    }

    [Header("Globale Objekteinstellungen")]
    [Tooltip("Debugmode f�r dieses GameObject")]
    public bool isDebugMode = false;
    [Tooltip("Art des Objektes")]
    public ObjectType objectType;
    [HideInInspector, Tooltip("Physik - Rigidbody2D - Wird automatisch zugewiesen, Component muss dem Objekt zugewiesen sein!")]
    public Rigidbody2D _rb2d;
    [HideInInspector, Tooltip("Animator des Charakters - Wird automatisch zugewiesen, Component muss dem Objekt zugewiesen sein!")]
    public Animator _anim;
    [Tooltip("Leeres Gameobject zum pr�fen ob auf Boden")]
    public GameObject groundCheck;
    [Tooltip("Layer f�r Wand und Boden")]
    public LayerMask layerGroundOrWall;
    [HideInInspector, Tooltip("Controller f�r Gegner")]
    public EnemyController enemyController;
    [HideInInspector, Tooltip("Controller f�r Player")]
    public PlayerController playerController;

    [Header("Bewegung")]
    [Tooltip("Geschwindigkeit des Objektes")]
    public float speed = 2.0f;
    [Tooltip("Sprungkraft des Objektes")]
    public float jumpForce = 500.0f;
    [HideInInspector, Tooltip("Gibt an ob der Spieler gerade nach Rechts (true) oder Links (false) schaut")]
    public bool isLookRight = true;
    [HideInInspector, Tooltip("Gibt an ob der Spieler gerade springt")]
    public bool isJumping = false;
    [HideInInspector, Tooltip("Gibt an ob der Spieler sich gerade auf dem Boden befindet (true) oder Springt (false)")]
    public bool isGrounded = false;
    [HideInInspector, Tooltip("Aktuelle Geschwindigkeit des Characters")]
    public float currentSpeed = 0;

    [Header("Angriff - Allgemein")]
    [Tooltip("Objekt kann angreifen")]
    public bool canAttack = true;

    [Header("Angriff - Waffe 1")]
    [Tooltip("Waffe 1")]
    public GameObject weapon1;
    [Tooltip("Geschwindigkeit der Waffe")]
    public float weapon1Speed = 250.0f;
    [Tooltip("Schaden der Waffe 1")]
    public float weapon1Damage = 1.0f;
    [Tooltip("Spawnpoint der Waffe 1")]
    public Transform weapon1SpawnPoint;
    [Tooltip("Ist die Waffe 1 eine Wurfwaffe?")]
    public bool weapon1Throwable;
    [Tooltip("Angriffsreichweite f�r Waffe 1")]
    public float weapon1Range = 100.0f;
    [Tooltip("Zeit bis n�chster Schuss m�glich")]
    public float weapon1Delay = 3;
    [HideInInspector, Tooltip("Aktueller Waffenstatus")]
    public bool weapon1Active = true;
    [HideInInspector, Tooltip("Gibt an ob der Spieler gerade die erste Attacke ausf�hrt")]
    public bool isAttacking1 = false;
    [Tooltip("Aktuelle Anzahl dieser Waffe (-1 = unendlich)")]
    public int weapon1Count = -1;

    [Header("Gesundheit / Schaden")]
    [Tooltip("Unverwundbar in Sek. bei Treffer")]
    public float invulnerableOnHit = 3.0f;
    [Tooltip("nach welcher Zeit soll das Objekt zerst�rt werden? (Dauer der Sterbeanimation)")]
    public float destroyAfterTime = 0.0f;
    [Tooltip("Spielerschaden bei Ber�hrung")]
    public float damageOnEnter = 1.0f;
    [Tooltip("Maximale Leben / Aktuelle Leben")]
    public float health = 5.0f;
    [Tooltip("Lebensbalken (Canvas Slider)")]
    public Slider healthBar;
    [HideInInspector, Tooltip("Gibt an ob das Objekt blinken soll")]
    public bool isBlinking = false;
    [Tooltip("Wie lange soll das Objekt bei einem Treffer blinken?")]
    public float blinkInterval = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        #region Eigene Components einbinden
        this._rb2d = GetComponent<Rigidbody2D>();   // Physik - Rigidbody2D (muss dem Char �ber Inspektor zugewiesen sein!)
        this._anim = GetComponent<Animator>();      // Animator (muss dem Char �ber Inspektor zugewiesen sein!)
        #endregion

        #region Laden der Individuellen Controller
        switch ( this.objectType )
        {
            // Lade PlayerController
            case ObjectType.Player:
                playerController = gameObject.AddComponent<PlayerController>() as PlayerController;
                break;
            // Lade EnemyController
            case ObjectType.Enemy:
                enemyController = gameObject.AddComponent<EnemyController>() as EnemyController;
                break;
            default:
                break;
        }
        #endregion
    }

    private void Update()
    {
        if ( isBlinking )
        {
            Blink();
        }
    }

    #region Blinking 
    /// <summary>Gibt an wie lange das Objekt schon blinkt</summary>
    private float blinkTotal = 0.0f;     // Wie lange schon am Blinken
    /// <summary>Timer wie lange das Sprite aktiviert oder deaktiviert sein soll</summary>
    private float blinkTimer = 0.0f;

    /// <summary>
    /// Start des Blinkens
    /// </summary>
    private void Blink()
    {
        blinkTimer += Time.deltaTime;                                           // F�ge aktuelle Zeit dem Blinktimer hinzu
        if ( blinkTimer >= blinkInterval)                                  // Wenn BlinkTimer > Blinkdauer ...
        {
            blinkTimer = 0;                                                     // ... Setze blinkTimer auf 0
            if ( gameObject.GetComponent<SpriteRenderer>().enabled == true )    // ... Wenn aktueller Sprite an ist ...
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;      // ... ... deaktiviere Sprite
            }
            else                                                                // ... sonst ...
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;       // ... ... aktiviere Sprite
            }
        }

        blinkTotal += Time.deltaTime;                                           // F�ge aktuelle Zeit der totalen Blinkzeit hinzu
        if (blinkTotal >= this.invulnerableOnHit)                               // Wenn totale Blinkzeit > gew�nschte Blinkdauer
        {
            isBlinking = false;                                                 // Deaktiviere Blinken
            blinkTotal = 0;                                                     // Setze totale Blinkdauer zur�ck auf 0

            gameObject.GetComponent<SpriteRenderer>().enabled = true;           // Aktiviere Sprite (falls es zu dem zeitpunkt deaktiviert war)
        }

    }
    #endregion Blinking

    #region DestroyObject
    /// <summary>
    /// L�sst das Objekt nach einer gewissen Zeit sterben
    /// </summary>
    /// <param name="delay"></param>
    public void DestroyObject(float delay = 0, bool setBlink = false)
    {
        if ( setBlink )
        {
            this.isBlinking = true;
            this.invulnerableOnHit = delay; // Ben�tigt damit das Objekt lange genug blinkt
        }


        if ( delay > 0 )                        // Wenn Delay angegeben ...
        {
            Debug.Log("Delay: " + delay + " Sek");
            Invoke("_DestroyObject", delay);    // ... zerst�re Objekt nach der Zeit
        } else
        {
            Debug.Log("No Delay");
            _DestroyObject();                   // ... sonst zerst�re Objekt sofort
        }
    }

    /// <summary>
    /// Zerst�re Objekt
    /// </summary>
    private void _DestroyObject()
    {
        Destroy(this.gameObject);
    }
    #endregion DestroyObject
}
