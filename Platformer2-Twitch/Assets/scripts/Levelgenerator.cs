using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levelgenerator : MonoBehaviour
{
    #region Variablen - Zufallszahlen
    [Tooltip("Zufallszahl nach welcher das Level generiert wird")]
    public string seed;
    private System.Random myRandom;
    #endregion Variablen - Zufallszahlen

    #region Variablen - Spielelemente
    [Tooltip("Tile Anfang")]
    public GameObject Tile_Left;
    [Tooltip("Tile Mitte")]
    public GameObject Tile_Middle;
    [Tooltip("Tile Ende")]
    public GameObject Tile_Right;

    [Tooltip("Länge eines einzelnen Tiles")]
    public float TileLength = 1.28f;

    [Tooltip("Minimale Höhe der Platform im Spiel (Koordinatensystem)")]
    public int platformHeightMin = 0;
    [Tooltip("Maximale Höhe der Platform im Spiel (Koordinatensystem)")]
    public int platformHeightMax = 5;

    [Tooltip("Minimale Länge der Platform (Anzahl Tiles!)")]
    public int platformLengthMin = 3;
    [Tooltip("Maximale Länge der Platform (Anzahl Tiles!)")]
    public int platformLengthMax = 5;

    [Tooltip("Anzahl der Platformen in diesem Level")]
    public int platformCount = 10;

    /// <summary>Liste aller generierten Platformen</summary>
    private List<Platform> lstPlatforms = new List<Platform>();
    #endregion Variablen - Spielelemente

    // Start is called before the first frame update
    void Start()
    {
        CreateLevel();
    }

    #region Functions - Zufallszahlen
    /// <summary>
    /// Generieren der Zufallszahlen falls nötig
    /// </summary>
    void generateRandom()
    {
        // Generiere Zufallszahl
        if (seed.Trim() == "" || seed.Trim() == "0")
        {
            System.Random rnd = new System.Random();
            int _seed = rnd.Next(1, 999999999);
            seed = _seed.ToString();

        }

        // Generiere aktuelle Randomzahl aus dem Hashcode von Seed
        myRandom = new System.Random(seed.GetHashCode());
    }

    /// <summary>
    /// Zusatzfunktion für Zufallszahl aus eigener Randomzahl
    /// Wichtig hier NICHT System.Random zu nehmen, da sonst ein "levelneustart" nicht mehr funktioniert!
    /// </summary>
    /// <param name="min">Minimale Zahl</param>
    /// <param name="max">Maximale Zahl</param>
    /// <returns></returns>
    int GetMyRandomNumber(int min, int max)
    {
        // Hinweis: max ist immer der Maximalwert, bei "Next" Funktion wird dieser Wert aber niemals erreicht! Daher setzen wir hier "max + 1"
        max = max + 1;

        // Generiere neue "Zufallszahl"
        return myRandom.Next(min, max);
    }
    #endregion Functions - Zufallszahlen

    #region Functions Platforms
    void CreatePlatform()
    {
        // Aktueller Startwert
        int currentStartIndex = 0;

        // Löschen aller Platformen (falls Level neustart)
        this.lstPlatforms.Clear();

        int currentHeight = GetMyRandomNumber(platformHeightMin, platformHeightMax);

        for ( int i = 0; i < platformCount; i++)
        {
            // Zufallszahl zwischen 0 und 2 // 0 = Höher / 1 = gleich bleibend / 2 = runter
            int randomHeight = GetMyRandomNumber(0, 2);

            // Setzen der neuen Höhe
            switch ( randomHeight )
            {
                case 0:
                    currentHeight++;
                    break;
                case 2:
                    currentHeight--;
                    break;
            }

            // Generieren einer Zufallslänge
            int currentLength = GetMyRandomNumber(platformLengthMin, platformLengthMax);

            // Platform erstellen
            Platform currentPlatform = new Platform(
                _start: currentStartIndex,
                _length: currentLength,
                _height: currentHeight
                );

            // Hinzufügen der Platform zur Liste
            lstPlatforms.Add(currentPlatform);

            // Aktuellen Startwert hochsetzen
            currentStartIndex += currentPlatform.length;
        }
        

        // Platform(en) erzeugen
    }
    #endregion Functions Platforms

    #region Functions Level
    /// <summary>
    /// Erstellen des Levels
    /// - Hinweis: Muss in extra Funktion, da dieses auch bei Spielertod ausgeführt werden muss
    /// </summary>
    void CreateLevel()
    {
        // Zufallszahl erstellen falls nötig
        generateRandom();

        // Platform(en) erstellen
        CreatePlatform();

        // Elemente auf Platform setzen

        // Level erstellen
        BuildLevel();
    }

    void BuildLevel()
    {
        // Platformen erstellen
        BuildLevel_Platforms();
        // AddOns hinzufügen
        // Gegner hinzufügen
        // Ziel hinzufügen
        // Spieler hinzufügen
    }

    void BuildLevel_Platforms()
    {
        if ( this.lstPlatforms.Count > 0 )
        {
            // Durchlaufe alle Platformen
            foreach ( Platform currPlatform in lstPlatforms )
            {
                // Durchlaufe alle "Tiles" auf der Platform
                for ( int i = currPlatform.start; i < currPlatform.end(); i++)
                {
                    GameObject currentTile = Tile_Middle;
                    if ( i == currPlatform.start )
                    {
                        currentTile = Tile_Left;
                    }

                    if ( i == currPlatform.end() - 1)
                    {
                        currentTile = Tile_Right;
                    }

                    float x = i * TileLength;
                    // Setze aktuelle Position der Platform (Achtung! i gilt dabei als index für "alle" Platformen!!!)
                    Vector3 pos = new Vector3(x, currPlatform.height, 0);
                    // Instanzieren eines neuen "Tiles" an der Position
                    GameObject currTile = (GameObject)Instantiate(currentTile, pos, Quaternion.identity);

                }
            }
        }
    }
    #endregion Functions Level
}
