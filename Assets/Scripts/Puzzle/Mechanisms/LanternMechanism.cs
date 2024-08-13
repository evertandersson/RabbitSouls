using System;
using UnityEngine;

namespace Puzzle.Mechanisms
{
    public class LanternMechanism : Mechanism {
        [SerializeField] private bool isLightOn;

        [SerializeField] private Light lightSource;

        private void Awake() {
            ToggleLight();
        }

        public override void Activate() {
            isLightOn = !isLightOn;
            ToggleLight();
        }
        private void ToggleLight() {
            lightSource.enabled = isLightOn;
        }
    }
}
