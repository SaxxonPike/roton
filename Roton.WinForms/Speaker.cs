﻿using System.ComponentModel;

namespace Roton.WinForms
{
    public partial class Speaker : Component, ISpeaker
    {
        public Speaker()
        {
            InitializeComponent();
        }

        public Speaker(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void PlayDrum(int drum)
        {
            
        }

        public void PlayNote(int note)
        {
            
        }

        public void Stop()
        {
            
        }
    }
}