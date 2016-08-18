using System.ComponentModel;
using Roton.Core;

namespace Roton.Interface.Audio.Controls
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