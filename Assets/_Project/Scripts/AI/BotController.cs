using UnityEngine;
using TumbleRumble.Player;
using System.Collections.Generic;

namespace TumbleRumble.AI
{
    /// <summary>
    /// AI bot controller - controls a player character using artificial intelligence
    /// Provides different difficulty levels and behaviors
    /// </summary>
    public class BotController : MonoBehaviour
    {
        [Header("Bot Settings")]
        [SerializeField] private BotDifficulty difficulty = BotDifficulty.Medium;
        [SerializeField] private float decisionInterval = 0.5f; // How often bot makes new decisions
        [SerializeField] private float reactionTime = 0.3f; // Delay before reacting to threats

        [Header("Behavior Weights")]
        [Range(0f, 1f)] [SerializeField] private float aggressiveness = 0.5f;
        [Range(0f, 1f)] [SerializeField] private float defensiveness = 0.3f;
        [Range(0f, 1f)] [SerializeField] private float opportunism = 0.7f;

        [Header("References")]
        [SerializeField] private PlayerController playerController;
        [SerializeField] private GrabSystem grabSystem;
        [SerializeField] private CombatSystem combatSystem;
        [SerializeField] private AbilitySystem abilitySystem;

        private Transform currentTarget;
        private Vector3 targetPosition;
        private BotState currentState = BotState.Idle;
        private float nextDecisionTime;
        private float nextActionTime;

        private List<Transform> potentialTargets = new List<Transform>();
        private Vector3 arenaCenter;
        private float arenaBounds = 20f;

        private enum BotState
        {
            Idle,
            Chasing,
            Attacking,
            Fleeing,
            Recovering,
            Positioning
        }

        public enum BotDifficulty
        {
            Easy,
            Medium,
            Hard,
            Expert
        }

        private void Start()
        {
            // Find components if not assigned
            if (playerController == null)
                playerController = GetComponent<PlayerController>();
            if (grabSystem == null)
                grabSystem = GetComponent<GrabSystem>();
            if (combatSystem == null)
                combatSystem = GetComponent<CombatSystem>();
            if (abilitySystem == null)
                abilitySystem = GetComponent<AbilitySystem>();

            // Configure difficulty
            ConfigureDifficulty();

            // Find arena center (assume 0,0,0 for now)
            arenaCenter = Vector3.zero;

            // Start decision making
            nextDecisionTime = Time.time + decisionInterval;
        }

        private void Update()
        {
            if (playerController == null || playerController.IsKnockedOut || playerController.IsEliminated)
                return;

            // Make decisions periodically
            if (Time.time >= nextDecisionTime)
            {
                MakeDecision();
                nextDecisionTime = Time.time + decisionInterval;
            }

            // Execute current behavior
            ExecuteBehavior();
        }

        private void ConfigureDifficulty()
        {
            switch (difficulty)
            {
                case BotDifficulty.Easy:
                    decisionInterval = 1f;
                    reactionTime = 0.6f;
                    aggressiveness = 0.3f;
                    defensiveness = 0.2f;
                    opportunism = 0.4f;
                    break;

                case BotDifficulty.Medium:
                    decisionInterval = 0.5f;
                    reactionTime = 0.3f;
                    aggressiveness = 0.5f;
                    defensiveness = 0.3f;
                    opportunism = 0.6f;
                    break;

                case BotDifficulty.Hard:
                    decisionInterval = 0.3f;
                    reactionTime = 0.15f;
                    aggressiveness = 0.7f;
                    defensiveness = 0.4f;
                    opportunism = 0.8f;
                    break;

                case BotDifficulty.Expert:
                    decisionInterval = 0.2f;
                    reactionTime = 0.05f;
                    aggressiveness = 0.9f;
                    defensiveness = 0.5f;
                    opportunism = 1f;
                    break;
            }
        }

        private void MakeDecision()
        {
            // Update potential targets
            FindTargets();

            // Evaluate threats and opportunities
            EvaluateSituation();

            // Choose best action
            ChooseState();
        }

        private void FindTargets()
        {
            potentialTargets.Clear();

            // Find all other players
            PlayerController[] allPlayers = FindObjectsOfType<PlayerController>();
            foreach (PlayerController player in allPlayers)
            {
                if (player != playerController && !player.IsEliminated)
                {
                    potentialTargets.Add(player.transform);
                }
            }

            // Choose closest target
            if (potentialTargets.Count > 0)
            {
                currentTarget = GetClosestTarget();
            }
            else
            {
                currentTarget = null;
            }
        }

        private Transform GetClosestTarget()
        {
            Transform closest = null;
            float closestDistance = float.MaxValue;

            foreach (Transform target in potentialTargets)
            {
                float distance = Vector3.Distance(transform.position, target.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = target;
                }
            }

            return closest;
        }

        private void EvaluateSituation()
        {
            // Check if too close to edge
            float distanceFromCenter = Vector3.Distance(transform.position, arenaCenter);
            bool nearEdge = distanceFromCenter > arenaBounds * 0.7f;

            // Check health status (using knocked out as proxy for low health)
            bool lowHealth = playerController.IsKnockedOut;

            // Store evaluation results for decision making
            if (nearEdge)
            {
                targetPosition = arenaCenter; // Move towards center
            }
        }

        private void ChooseState()
        {
            // Check if near edge - highest priority
            float distanceFromCenter = Vector3.Distance(transform.position, arenaCenter);
            if (distanceFromCenter > arenaBounds * 0.7f)
            {
                currentState = BotState.Positioning;
                targetPosition = arenaCenter;
                return;
            }

            // No targets - idle or position
            if (currentTarget == null)
            {
                currentState = BotState.Positioning;
                targetPosition = arenaCenter;
                return;
            }

            // Calculate distance to target
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);

            // Decision making based on distance and personality
            if (distanceToTarget < 2f)
            {
                // Close range - attack
                if (Random.value < aggressiveness)
                {
                    currentState = BotState.Attacking;
                }
                else
                {
                    currentState = BotState.Fleeing;
                }
            }
            else if (distanceToTarget < 10f)
            {
                // Medium range - chase
                if (Random.value < (aggressiveness + opportunism) * 0.5f)
                {
                    currentState = BotState.Chasing;
                    targetPosition = currentTarget.position;
                }
                else
                {
                    currentState = BotState.Positioning;
                    targetPosition = GetStrategicPosition();
                }
            }
            else
            {
                // Long range - position or chase
                if (Random.value < aggressiveness * 0.5f)
                {
                    currentState = BotState.Chasing;
                    targetPosition = currentTarget.position;
                }
                else
                {
                    currentState = BotState.Positioning;
                    targetPosition = GetStrategicPosition();
                }
            }
        }

        private void ExecuteBehavior()
        {
            switch (currentState)
            {
                case BotState.Idle:
                    ExecuteIdle();
                    break;

                case BotState.Chasing:
                    ExecuteChase();
                    break;

                case BotState.Attacking:
                    ExecuteAttack();
                    break;

                case BotState.Fleeing:
                    ExecuteFlee();
                    break;

                case BotState.Positioning:
                    ExecutePosition();
                    break;
            }
        }

        private void ExecuteIdle()
        {
            // Do nothing, wait for new decision
        }

        private void ExecuteChase()
        {
            if (currentTarget == null) return;

            // Move towards target
            Vector3 direction = (currentTarget.position - transform.position).normalized;
            SimulateInput(direction, false, false, false, false);

            // Jump occasionally to speed up movement
            if (Random.value < 0.05f && Time.time >= nextActionTime)
            {
                SimulateInput(direction, true, false, false, false);
                nextActionTime = Time.time + 1f;
            }
        }

        private void ExecuteAttack()
        {
            if (currentTarget == null) return;

            Vector3 direction = (currentTarget.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, currentTarget.position);

            if (distance < 2f)
            {
                // In attack range
                if (Time.time >= nextActionTime)
                {
                    // Randomly choose attack type
                    float actionRoll = Random.value;

                    if (actionRoll < 0.4f)
                    {
                        // Punch
                        SimulateInput(direction, false, false, true, false);
                    }
                    else if (actionRoll < 0.7f)
                    {
                        // Grab
                        SimulateInput(direction, false, true, false, false);
                    }
                    else
                    {
                        // Use ability
                        SimulateInput(direction, false, false, false, true);
                    }

                    nextActionTime = Time.time + reactionTime + Random.Range(0.2f, 0.5f);
                }
            }
            else
            {
                // Move closer
                SimulateInput(direction, false, false, false, false);
            }
        }

        private void ExecuteFlee()
        {
            if (currentTarget == null) return;

            // Move away from target
            Vector3 direction = (transform.position - currentTarget.position).normalized;
            SimulateInput(direction, false, false, false, false);

            // Jump to escape
            if (Random.value < 0.1f && Time.time >= nextActionTime)
            {
                SimulateInput(direction, true, false, false, false);
                nextActionTime = Time.time + 0.5f;
            }
        }

        private void ExecutePosition()
        {
            // Move towards strategic position
            Vector3 direction = (targetPosition - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, targetPosition);

            if (distance > 1f)
            {
                SimulateInput(direction, false, false, false, false);
            }
        }

        private Vector3 GetStrategicPosition()
        {
            // Get position between center and target
            if (currentTarget != null)
            {
                Vector3 midpoint = (arenaCenter + currentTarget.position) * 0.5f;
                midpoint.y = transform.position.y;
                return midpoint;
            }

            return arenaCenter;
        }

        private void SimulateInput(Vector3 moveDirection, bool jump, bool grab, bool punch, bool ability)
        {
            // Simulate movement input
            if (playerController != null)
            {
                // This is a simplified version - in reality, we'd need to integrate with the input system
                // For now, we'll apply forces directly
                Rigidbody rb = playerController.GetComponent<Rigidbody>();
                if (rb != null && moveDirection.magnitude > 0.1f)
                {
                    Vector3 force = moveDirection * 5f; // Adjust force as needed
                    rb.AddForce(force, ForceMode.Force);
                }
            }

            // Simulate jump
            if (jump && playerController != null)
            {
                Rigidbody rb = playerController.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(Vector3.up * 8f, ForceMode.Impulse);
                }
            }

            // Simulate grab
            if (grab && grabSystem != null)
            {
                grabSystem.TryGrab();
            }

            // Simulate punch
            if (punch && combatSystem != null)
            {
                combatSystem.Punch();
            }

            // Simulate ability
            if (ability && abilitySystem != null && Random.value < 0.3f)
            {
                // Use random ability
                if (Random.value < 0.5f)
                {
                    abilitySystem.UseEnergyBurst();
                }
                else
                {
                    abilitySystem.UseSpeedBoost();
                }
            }
        }

        /// <summary>
        /// Set the bot difficulty
        /// </summary>
        public void SetDifficulty(BotDifficulty newDifficulty)
        {
            difficulty = newDifficulty;
            ConfigureDifficulty();
        }

        /// <summary>
        /// Set arena bounds for positioning
        /// </summary>
        public void SetArenaBounds(Vector3 center, float bounds)
        {
            arenaCenter = center;
            arenaBounds = bounds;
        }
    }
}
