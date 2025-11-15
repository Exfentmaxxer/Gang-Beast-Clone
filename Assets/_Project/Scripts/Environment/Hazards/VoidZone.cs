using UnityEngine;
using TumbleRumble.Player;

namespace TumbleRumble.Environment
{
    /// <summary>
    /// Elimination zone - instant KO when player enters
    /// Used for pits, edges, and out-of-bounds areas
    /// </summary>
    public class VoidZone : HazardBase
    {
        protected override void HandlePlayerContact(Collider playerCollider)
        {
            PlayerController player = playerCollider.GetComponentInParent<PlayerController>();
            if (player != null && !player.IsKnockedOut)
            {
                Debug.Log($"[VoidZone] Player {player.PlayerId} fell into void - ELIMINATED!");
                EliminatePlayer(player);
            }
        }
    }
}
