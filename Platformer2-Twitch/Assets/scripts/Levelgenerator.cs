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

    [Tooltip("L�nge eines einzelnen Tiles")]
    public float TileLength = 1.28f;

    [Tooltip("Minimale H�he der Platform im Spiel (Koordinatensystem)")]
    public int platformHeightMin = 0;
    [Tooltip("Maximale H�he der Platform im Spiel (Koordinatensystem)")]
    public int platformHeightMax = 5;

    [Tooltip("Minimale L�nge der Platform (Anzahl Tiles!)")]
    public int platformLengthMin = 3;
    [Tooltip("Maximale L�nge der Platform (Anzahl Tiles!)")]
    public int platformLengthMax = 5;

    [Tooltip("Anzahl der Platformen in diesem Level")]
    public int platformCount = 10;
    [Tooltip("Prozentuale Anzahl an Platformen mit Abgrund"), Range(0, 1)]
    public float platformAbysPercent = 0.5f;

    /// <summary>Liste aller generierten Platformen</summary>
    [HideInInspector]
    public List<Platform> lstPlatforms = new List<Platform>();
    #endregion Variablen - Spielelemente

    // Start is called before the first frame update
    void Start()
    {
        CreateLevel();
    }

    #region Functions - Zufallszahlen
    /// <summary>
    /// Generieren der Zufallszahlen falls n�tig
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
    /// Zusatzfunktion f�r Zufallszahl aus eigener Randomzahl
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

    /// <summary>
    /// Laden einer Liste von Zufallszahlen
    /// - Dabei werden alle m�glichen Zahlen in einen Zwischenspeicher geladen und dann entsprechend per zufall ausgew�hlt. �hnlich wie beim Lotto
    /// - Hintergrund: Es muss sichergestellt werden, dass keine Zahl doppelt ist!
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    List<int> GetMyRandomNumbers(int min, int max, int count)
    {
        // Hinweis: max ist immer der Maximalwert, bei "Next" Funktion wird dieser Wert aber niemals erreicht! Daher setzen wir hier "max + 1"
        max = max + 1;

        List<int> response = new List<int>();
        List<int> temp = new List<int>();

        // F�lle Zwischenspeicher
        for ( int i = min; i < max; i++)
        {
            temp.Add(i);
        }

        // F�lle R�ckgabe
        for ( int i = 0; i < count; i++ )
        {
            // W�hle eine Zahl aus dem Zwischenspeicher aus
            // Hier muss Count-1 gerechnet werden, da sonst ein Fehler auftreten k�nnte! 
            // Hintergrund: Array f�ngt bei "0" an und max Wert wird in "GetMyRandomNumer" einen hochgez�hlt!
            int tmp = GetMyRandomNumber(0, temp.Count - 1);
            // F�ge Zahl dem Response hinzu
            response.Add(temp[tmp]);
            // L�sche Zahl aus Zwischenspeicher
            temp.RemoveAt(tmp);
        }
        return response;
    }
    #endregion Functions - Zufallszahlen

    #region Functions Platforms
    void CreatePlatform()
    {
        // Aktueller Startwert
        int currentStartIndex = 0;

        // L�schen aller Platformen (falls Level neustart)
        this.lstPlatforms.Clear();

        int currentHeight = GetMyRandomNumber(platformHeightMin, platformHeightMax);

        for ( int i = 0; i < platformCount; i++)
        {
            // Zufallszahl zwischen 0 und 2 // 0 = H�her / 1 = gleich bleibend / 2 = runter
            int randomHeight = GetMyRandomNumber(0, 2);

            // Speichere vorherige H�he (n�tig damit tiles besser ausgew�hlt werden k�nnen)
            int heightBefore = currentHeight;

            // Setzen der neuen H�he
            switch ( randomHeight )
            {
                case 0:
                    currentHeight++;
                    break;
                case 2:
                    currentHeight--;
                    break;
            }

            // Pr�fen ob Minimale bzw. Maximale H�he nicht �berschritten wurde
            currentHeight = Mathf.Max(currentHeight, platformHeightMin);
            currentHeight = Mathf.Min(currentHeight, platformHeightMax);


            // Generieren einer Zufallsl�nge
            int currentLength = GetMyRandomNumber(platformLengthMin, platformLengthMax);

            // Platform erstellen
            Platform currentPlatform = new Platform(
                _id: i,
                _start: currentStartIndex,
                _length: currentLength,
                _height: currentHeight
                );

            // Hinzuf�gen der Platform zur Liste
            lstPlatforms.Add(currentPlatform);

            // Aktuellen Startwert hochsetzen
            currentStartIndex += currentPlatform.length;
        }
        

        // Platform(en) erzeugen
    }

    void CreatePlatform_Abyss()
    {
        // Berechnung welche Platform alles einen Abgrund haben soll (Prozentuale Angabe �ber public Variable)
        List<int> lstHasAbyss = GetMyRandomNumbers(0, platformCount, Mathf.RoundToInt(platformAbysPercent * platformCount));
        // Setzen der Abgr�ne
        if ( lstHasAbyss.Count > 0 )
        {
            foreach ( int hasAbyss in lstHasAbyss )
            {
                if ( lstPlatforms.Count > hasAbyss )
                {
                    lstPlatforms[hasAbyss].hasAbyss = true;
                }
            }
        }
    }
    #endregion Functions Platforms

    #region Functions Level
    /// <summary>
    /// Erstellen des Levels
    /// - Hinweis: Muss in extra Funktion, da dieses auch bei Spielertod ausgef�hrt werden muss
    /// </summary>
    void CreateLevel()
    {
        // Zufallszahl erstellen falls n�tig
        generateRandom();

        // Platform(en) erstellen
        CreatePlatform();

        // Abgr�ne hinzuf�gen
        CreatePlatform_Abyss();

        // Elemente auf Platform setzen

        // Level erstellen
        BuildLevel();
    }

    void BuildLevel()
    {
        // Platformen erstellen
        BuildLevel_Platforms();
        // AddOns hinzuf�gen
        // Gegner hinzuf�gen
        // Ziel hinzuf�gen
        // Spieler hinzuf�gen
    }

    void BuildLevel_Platforms()
    {
        if ( this.lstPlatforms.Count > 0 )
        {
            // Durchlaufe alle Platformen
            foreach ( Platform currPlatform in lstPlatforms )
            {
                currPlatform.Build_Platform(this);
            }
        }
    }
    #endregion Functions Level
}
