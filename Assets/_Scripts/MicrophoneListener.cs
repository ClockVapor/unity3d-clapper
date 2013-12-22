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

public class MicrophoneListener : MonoBehaviour
{
    public string MicrophoneName = null;
    public int Frequency = 128;
    public int ClipLength = 1;

    private AudioClip clip;
    private int clipStart = 0;
    private int oldMicPos = 0;

    private void OnEnable()
    {
        clipStart = 0;
        clip = Microphone.Start(MicrophoneName, true, ClipLength, Frequency);
    }

    private void OnDisable()
    {
        Microphone.End(MicrophoneName);
    }

    private void Update()
    {
        // If the microphone has looped recording, reset the start sample to 0
        int micPos = Microphone.GetPosition(MicrophoneName);
        if (Microphone.GetPosition(MicrophoneName) < oldMicPos)
        {
            clipStart = 0;
        }
        oldMicPos = micPos;
    }

    /// <summary>
    /// Get the microphone's recorded data, starting from the last call to GetNewClipData, or from the last time the microphone clip looped.
    /// </summary>
    /// <returns>The new clip data.</returns>
    public float[] GetNewClipData()
    {
        float[] data = new float[Microphone.GetPosition(MicrophoneName) - clipStart];
        clip.GetData(data, clipStart);
        clipStart += data.Length;

        return data;
    }
}
