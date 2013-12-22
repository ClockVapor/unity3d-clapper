/************************************************************************

 Unity3D Clapper
 Copyright (C) 2013 Nick Lowery (nick.a.lowery@gmail.com)

 This program is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.

 www.gnu.org/copyleft/gpl.html

************************************************************************/

using UnityEngine;
using System.Collections;

public class AudioFile
{
    private readonly AudioClip clip;
    public AudioClip Clip
    {
        get
        {
            return clip;
        }
    }

    public AudioFile(string filePath)
    {
        clip = new WWW(filePath).audioClip;
    }
}
