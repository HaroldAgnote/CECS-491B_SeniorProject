﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model.UnitEffects {
    interface ITurnEffect {
        int StartingTurns { get; }

        int CurrentTurn { get; set; }

        void UpdateTurns();

    }
}
