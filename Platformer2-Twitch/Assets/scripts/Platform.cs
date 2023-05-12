using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform
{
    /// <summary>
    /// Aktuelle Platform
    /// </summary>
    public int id;

    /// <summary>
    /// Startindex - Wo soll die gesamte Platform starten / Wo wird erstes "Tile" gesetzt
    /// </summary>
    public int start;

    /// <summary>
    /// L�nge der gesamten Platform
    /// </summary>
    public int length;

    /// <summary>
    /// H�he der Platform (wie hoch soll diese angesiedelt sein)
    /// </summary>
    public int height;

    /// <summary>
    /// Gibt an ob diese Platform einen Abgrund hat
    /// </summary>
    public bool hasAbyss;

    public Platform(int _id, int _start, int _length, int _height)
    {
        this.id = _id;
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
        // H�he der vorherigen Platform ermitteln
        int heightBefore = this.id == 0 ? 0 : levelgenerator.lstPlatforms[this.id - 1].height;
        // H�he der folgenden Platform ermitteln
        int heightAfter = this.id == levelgenerator.lstPlatforms.Count -1 ? 0 : levelgenerator.lstPlatforms[this.id + 1].height;
        // Pr�fen ob vorherige Platform am ende einen "Abgrund" hat
        bool abyssBefore = this.id == 0 ? false : levelgenerator.lstPlatforms[this.id - 1].hasAbyss;


        Debug.Log("ID: " + this.id + " / " + levelgenerator.lstPlatforms.Count);

        // Durchlaufe alle "Tiles" auf der Platform
        for (int i = this.start; i < this.end(); i++)
        {
            // Tempor�res Ende der Platform ermitteln (wenn Abgrund vorhanden, L�nge -1)
            int tmpEnd = this.hasAbyss ? this.end() - 1 : this.end();

            if (i < tmpEnd)
            {

                GameObject currentTile = levelgenerator.Tile_Middle;
                // Wenn Start der Platform und NICHT erste Platform und vorherige H�he <> aktuelle H�he -> Setze "LeftTile"
                if (i == 0  || (i == this.start && (( this.id > 0 && heightBefore != this.height) || abyssBefore)))
                {
                    currentTile = levelgenerator.Tile_Left;
                }

                // Wenn ende der Platform und (Aktuelle Platform hat einen Abgrund ODER H�he der folgenden Platform <> aktuelle H�he ODER letzte Platform)
                if (i == tmpEnd - 1 && (
                    this.hasAbyss                                           // Abgrund vorhanden
                    || heightAfter != this.height                           // Nachfolgende H�he <> aktuelle H�he
                    || this.id == levelgenerator.lstPlatforms.Count - 1     // Letzte Platform
                    ))
                {
                    currentTile = levelgenerator.Tile_Right;
                }

                // ToDo: ggfl. TileLength automatisch auslesen!
                float x = i * levelgenerator.TileLength;
                // Setze aktuelle Position der Platform (Achtung! i gilt dabei als index f�r "alle" Platformen!!!)
                Vector3 pos = new Vector3(x, this.height, 0);
                // Instanzieren eines neuen "Tiles" an der Position
                GameObject currTile = (GameObject)UnityEngine.GameObject.Instantiate(currentTile, pos, Quaternion.identity);
            }

        }
    }
}
