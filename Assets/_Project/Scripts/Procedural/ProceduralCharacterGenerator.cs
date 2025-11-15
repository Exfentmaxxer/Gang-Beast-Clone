using UnityEngine;
using System.Collections.Generic;

namespace TumbleRumble.Procedural
{
    /// <summary>
    /// Procedurally generates alien character models
    /// Creates unique geometry for each species type
    /// </summary>
    public class ProceduralCharacterGenerator : MonoBehaviour
    {
        public enum SpeciesType
        {
            Gelatinous,
            Tentacled,
            Crystalline,
            Gaseous,
            Symbiotic
        }

        [Header("Species Configuration")]
        public SpeciesType species = SpeciesType.Gelatinous;
        public Color primaryColor = Color.cyan;
        public Color emissiveColor = Color.cyan;
        public float emissiveIntensity = 1f;

        [Header("Procedural Settings")]
        public int detailLevel = 2; // 0=low, 1=medium, 2=high
        public bool generateOnStart = true;

        private GameObject visualRoot;
        private Material characterMaterial;

        private void Start()
        {
            if (generateOnStart)
            {
                GenerateCharacter();
            }
        }

        public void GenerateCharacter()
        {
            // Clean up existing visual
            if (visualRoot != null)
            {
                DestroyImmediate(visualRoot);
            }

            // Create visual root
            visualRoot = new GameObject("ProceduralVisual");
            visualRoot.transform.SetParent(transform);
            visualRoot.transform.localPosition = Vector3.zero;

            // Create material
            CreateMaterial();

            // Generate based on species
            switch (species)
            {
                case SpeciesType.Gelatinous:
                    GenerateGelatinous();
                    break;
                case SpeciesType.Tentacled:
                    GenerateTentacled();
                    break;
                case SpeciesType.Crystalline:
                    GenerateCrystalline();
                    break;
                case SpeciesType.Gaseous:
                    GenerateGaseous();
                    break;
                case SpeciesType.Symbiotic:
                    GenerateSymbiotic();
                    break;
            }
        }

        private void CreateMaterial()
        {
            characterMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            characterMaterial.color = primaryColor;
            characterMaterial.SetFloat("_Metallic", 0f);
            characterMaterial.SetFloat("_Smoothness", 0.7f);
            characterMaterial.EnableKeyword("_EMISSION");
            characterMaterial.SetColor("_EmissionColor", emissiveColor * emissiveIntensity);
        }

        #region Gelatinous Generation
        private void GenerateGelatinous()
        {
            // Create blob body
            GameObject body = CreateSphere(visualRoot.transform, "Body", Vector3.up * 1f, 0.8f);
            body.GetComponent<Renderer>().material = characterMaterial;

            // Create pseudopods (4)
            float angleStep = 90f;
            for (int i = 0; i < 4; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                Vector3 direction = new Vector3(Mathf.Cos(angle), -0.5f, Mathf.Sin(angle));
                Vector3 position = Vector3.up * 0.8f + direction * 0.3f;

                GameObject pseudopod = CreateCapsule(visualRoot.transform, $"Pseudopod_{i}", position, 0.15f, 0.5f);
                pseudopod.transform.LookAt(position + direction);
                pseudopod.GetComponent<Renderer>().material = characterMaterial;
            }

            // Create glowing core
            GameObject core = CreateSphere(visualRoot.transform, "Core", Vector3.up * 1f, 0.3f);
            Material coreMaterial = new Material(characterMaterial);
            coreMaterial.SetColor("_EmissionColor", emissiveColor * emissiveIntensity * 2f);
            core.GetComponent<Renderer>().material = coreMaterial;
        }
        #endregion

        #region Tentacled Generation
        private void GenerateTentacled()
        {
            // Create body pod
            GameObject body = CreateSphere(visualRoot.transform, "Body", Vector3.up * 1.2f, 0.6f);
            body.GetComponent<Renderer>().material = characterMaterial;

            // Create eyes
            for (int i = 0; i < 2; i++)
            {
                float xOffset = (i == 0) ? -0.2f : 0.2f;
                GameObject eye = CreateSphere(visualRoot.transform, $"Eye_{i}",
                    Vector3.up * 1.3f + Vector3.right * xOffset + Vector3.forward * 0.5f, 0.15f);
                Material eyeMaterial = new Material(characterMaterial);
                eyeMaterial.color = Color.white;
                eyeMaterial.SetColor("_EmissionColor", Color.white);
                eye.GetComponent<Renderer>().material = eyeMaterial;
            }

            // Create tentacles (6)
            for (int i = 0; i < 6; i++)
            {
                float angle = i * 60f * Mathf.Deg2Rad;
                Vector3 basePos = Vector3.up * 0.9f + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * 0.4f;

                CreateTentacle(visualRoot.transform, $"Tentacle_{i}", basePos, angle);
            }
        }

        private void CreateTentacle(Transform parent, string name, Vector3 basePos, float angle)
        {
            GameObject tentacle = new GameObject(name);
            tentacle.transform.SetParent(parent);
            tentacle.transform.localPosition = Vector3.zero;

            // Create segments
            int segments = 4;
            for (int i = 0; i < segments; i++)
            {
                float t = i / (float)segments;
                Vector3 direction = new Vector3(Mathf.Cos(angle), -0.5f - t * 0.5f, Mathf.Sin(angle));
                Vector3 position = basePos + direction * (0.2f + i * 0.15f);

                GameObject segment = CreateCapsule(tentacle.transform, $"Segment_{i}", position, 0.08f - i * 0.015f, 0.2f);
                segment.GetComponent<Renderer>().material = characterMaterial;
            }
        }
        #endregion

        #region Crystalline Generation
        private void GenerateCrystalline()
        {
            // Create faceted body
            GameObject body = CreateCrystal(visualRoot.transform, "Body", Vector3.up * 1f, 0.6f, 1.2f, 8);
            Material crystalMaterial = new Material(characterMaterial);
            crystalMaterial.SetFloat("_Metallic", 0.2f);
            crystalMaterial.SetFloat("_Smoothness", 0.9f);
            body.GetComponent<Renderer>().material = crystalMaterial;

            // Create limb crystals
            Vector3[] limbPositions = new Vector3[]
            {
                new Vector3(0.4f, 0.7f, 0.2f),   // Right arm
                new Vector3(-0.4f, 0.7f, 0.2f),  // Left arm
                new Vector3(0.2f, 0.3f, 0f),     // Right leg
                new Vector3(-0.2f, 0.3f, 0f)     // Left leg
            };

            foreach (Vector3 pos in limbPositions)
            {
                GameObject limb = CreateCrystal(visualRoot.transform, "Limb", pos, 0.15f, 0.5f, 6);
                limb.GetComponent<Renderer>().material = crystalMaterial;
            }
        }

        private GameObject CreateCrystal(Transform parent, string name, Vector3 position, float radius, float height, int facets)
        {
            GameObject crystal = new GameObject(name);
            crystal.transform.SetParent(parent);
            crystal.transform.localPosition = position;

            MeshFilter mf = crystal.AddComponent<MeshFilter>();
            MeshRenderer mr = crystal.AddComponent<MeshRenderer>();

            // Generate crystal mesh
            Mesh mesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            // Base vertices
            for (int i = 0; i < facets; i++)
            {
                float angle = i / (float)facets * Mathf.PI * 2f;
                vertices.Add(new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius));
            }

            // Tip vertex
            vertices.Add(new Vector3(0f, height, 0f));

            // Create triangles
            for (int i = 0; i < facets; i++)
            {
                int next = (i + 1) % facets;
                triangles.Add(i);
                triangles.Add(next);
                triangles.Add(facets);
            }

            // Base cap
            for (int i = 1; i < facets - 1; i++)
            {
                triangles.Add(0);
                triangles.Add(i + 1);
                triangles.Add(i);
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            mf.mesh = mesh;

            return crystal;
        }
        #endregion

        #region Gaseous Generation
        private void GenerateGaseous()
        {
            // Create force field sphere
            GameObject forceField = CreateSphere(visualRoot.transform, "ForceField", Vector3.up * 1f, 0.7f);
            Material gasMaterial = new Material(characterMaterial);
            gasMaterial.color = new Color(primaryColor.r, primaryColor.g, primaryColor.b, 0.3f);
            gasMaterial.SetFloat("_Surface", 1); // Transparent
            forceField.GetComponent<Renderer>().material = gasMaterial;

            // Create energy core
            GameObject core = CreateSphere(visualRoot.transform, "EnergyCore", Vector3.up * 1f, 0.4f);
            Material coreMaterial = new Material(characterMaterial);
            coreMaterial.SetColor("_EmissionColor", emissiveColor * emissiveIntensity * 3f);
            core.GetComponent<Renderer>().material = coreMaterial;

            // Create energy tendrils
            for (int i = 0; i < 4; i++)
            {
                float angle = i * 90f * Mathf.Deg2Rad;
                Vector3 direction = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
                GameObject tendril = CreateCapsule(visualRoot.transform, $"Tendril_{i}",
                    Vector3.up * 1f + direction * 0.3f, 0.1f, 0.4f);
                tendril.GetComponent<Renderer>().material = gasMaterial;
            }
        }
        #endregion

        #region Symbiotic Generation
        private void GenerateSymbiotic()
        {
            // Create swarm of small creatures
            int creatureCount = 9;
            float radius = 0.3f;

            for (int i = 0; i < creatureCount; i++)
            {
                // Arrange in humanoid shape
                Vector3 position = GetSymbioticPosition(i, creatureCount);

                GameObject creature = CreateSphere(visualRoot.transform, $"Creature_{i}", position, 0.12f);

                // Vary colors slightly
                Material creatureMaterial = new Material(characterMaterial);
                float hueShift = (i / (float)creatureCount) * 0.1f;
                Color shiftedColor = ShiftHue(primaryColor, hueShift);
                creatureMaterial.color = shiftedColor;
                creatureMaterial.SetColor("_EmissionColor", shiftedColor * emissiveIntensity);

                creature.GetComponent<Renderer>().material = creatureMaterial;
            }
        }

        private Vector3 GetSymbioticPosition(int index, int total)
        {
            // Arrange in rough humanoid formation
            switch (index)
            {
                case 0: return Vector3.up * 1.4f; // Head
                case 1: return Vector3.up * 1.1f; // Chest
                case 2: return Vector3.up * 0.8f; // Pelvis
                case 3: return Vector3.up * 1.2f + Vector3.right * 0.3f; // Right shoulder
                case 4: return Vector3.up * 1.2f + Vector3.left * 0.3f; // Left shoulder
                case 5: return Vector3.up * 0.9f + Vector3.right * 0.4f; // Right hip
                case 6: return Vector3.up * 0.9f + Vector3.left * 0.4f; // Left hip
                case 7: return Vector3.up * 0.5f + Vector3.right * 0.2f; // Right leg
                case 8: return Vector3.up * 0.5f + Vector3.left * 0.2f; // Left leg
                default: return Vector3.up * 1f;
            }
        }
        #endregion

        #region Primitive Helpers
        private GameObject CreateSphere(Transform parent, string name, Vector3 position, float radius)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = name;
            sphere.transform.SetParent(parent);
            sphere.transform.localPosition = position;
            sphere.transform.localScale = Vector3.one * radius * 2f;

            // Remove collider (physics handled by main collider)
            Destroy(sphere.GetComponent<Collider>());

            return sphere;
        }

        private GameObject CreateCapsule(Transform parent, string name, Vector3 position, float radius, float height)
        {
            GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            capsule.name = name;
            capsule.transform.SetParent(parent);
            capsule.transform.localPosition = position;
            capsule.transform.localScale = new Vector3(radius * 2f, height / 2f, radius * 2f);

            // Remove collider
            Destroy(capsule.GetComponent<Collider>());

            return capsule;
        }

        private GameObject CreateCube(Transform parent, string name, Vector3 position, Vector3 scale)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = name;
            cube.transform.SetParent(parent);
            cube.transform.localPosition = position;
            cube.transform.localScale = scale;

            // Remove collider
            Destroy(cube.GetComponent<Collider>());

            return cube;
        }

        private Color ShiftHue(Color color, float shift)
        {
            float h, s, v;
            Color.RGBToHSV(color, out h, out s, out v);
            h = (h + shift) % 1f;
            return Color.HSVToRGB(h, s, v);
        }
        #endregion
    }
}
