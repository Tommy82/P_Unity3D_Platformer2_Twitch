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
}
