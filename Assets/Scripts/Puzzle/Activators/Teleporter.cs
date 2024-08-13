using Actors;
using Actors.Controllers;
using Gamemode;
using System;
using System.Linq;
using UnityEngine;

namespace Puzzle.Activators
{
    public class Teleporter : Activator {

        [SerializeField] Teleporter teleporterExit;

        protected override void Interact(GameObject player) {
            Transform playerTransform = player.transform.root;

            playerTransform.position = teleporterExit.transform.position;
            playerTransform.GetComponent<MoveIndicator>().DisplayValidIndicators();
            playerTransform.GetComponent<PlayerController>().ResetMoveTarget();
        }
    }
}
