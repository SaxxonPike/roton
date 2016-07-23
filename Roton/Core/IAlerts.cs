using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Core
{
    public interface IAlerts
    {
        bool AlertAmmo { get; set; }
        bool AlertDark { get; set; }
        bool AlertEnergy { get; set; }
        bool AlertFake { get; set; }
        bool AlertForest { get; set; }
        bool AlertGem { get; set; }
        bool AlertNoAmmo { get; set; }
        bool AlertNoShoot { get; set; }
        bool AlertNotDark { get; set; }
        bool AlertNoTorch { get; set; }
        bool AlertTorch { get; set; }
    }
}
