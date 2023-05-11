using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform
{
    /// <summary>
    /// Startindex - Wo soll die gesamte Platform starten / Wo wird erstes "Tile" gesetzt
    /// </summary>
    public int start;

    /// <summary>
    /// Länge der gesamten Platform
    /// </summary>
    public int length;

    /// <summary>
    /// Höhe der Platform (wie hoch soll diese angesiedelt sein)
    /// </summary>
    public int height;

    /// <summary>
    /// Gibt an ob diese Platform einen Abgrund hat
    /// </summary>
    public bool hasAbyss;

    public Platform(int _start, int _length, int _height)
    {
        this.start = _start;
        this.length = _length;
        this.height = _height;
    }

    /// <summary>
    /// Gebe das Ende der Platform aus
    /// </summary>
    /// <returns>int - Ende der Platform</returns>
    public int end()
    {
        return this.start + this.length;
    }

    public void Build_Platform(Levelgenerator levelgenerator)
    {
        // Durchlaufe alle "Tiles" auf der Platform
        for (int i = this.start; i < this.end(); i++)
        {
            int tmpEnd = this.hasAbyss ? this.end() - 1 : this.end();

            // Hier gehts weiter ... Abgrund wird nicht angezeigt, stattdessen wird das "middle" geladen!

            if (i <= tmpEnd)
            {

                GameObject currentTile = levelgenerator.Tile_Middle;
                if (i == this.start)
                {
                    currentTile = levelgenerator.Tile_Left;
                }

                if (i == tmpEnd - 1)
                {
                    currentTile = levelgenerator.Tile_Right;
                }

                // ToDo: ggfl. TileLength automatisch auslesen!
                float x = i * levelgenerator.TileLength;
                // Setze aktuelle Position der Platform (Achtung! i gilt dabei als index für "alle" Platformen!!!)
                Vector3 pos = new Vector3(x, this.height, 0);
                // Instanzieren eines neuen "Tiles" an der Position
                GameObject currTile = (GameObject)UnityEngine.GameObject.Instantiate(currentTile, pos, Quaternion.identity);
            }

        }
    }
}
