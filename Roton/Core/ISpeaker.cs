﻿namespace Roton.Core
{
    public interface ISpeaker
    {
        void PlayDrum(int drum);
        void PlayNote(int note);
        void Stop();
    }
}