using UnityEngine;
using System.Collections.Generic;

namespace TumbleRumble.VFX
{
    /// <summary>
    /// Procedural VFX generator - creates all visual effects at runtime
    /// Generates particle systems for impacts, abilities, knockouts, etc.
    /// </summary>
    public class ProceduralVFXGenerator : MonoBehaviour
    {
        private static ProceduralVFXGenerator _instance;
        public static ProceduralVFXGenerator Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("ProceduralVFXGenerator");
                    _instance = go.AddComponent<ProceduralVFXGenerator>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        private Dictionary<string, GameObject> vfxPrefabs = new Dictionary<string, GameObject>();

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            // Pre-generate common VFX prefabs
            GenerateAllVFX();
        }

        private void GenerateAllVFX()
        {
            Debug.Log("[ProceduralVFXGenerator] Generating all VFX prefabs...");

            vfxPrefabs["Impact"] = CreateImpactVFX();
            vfxPrefabs["Knockout"] = CreateKnockoutVFX();
            vfxPrefabs["Elimination"] = CreateEliminationVFX();
            vfxPrefabs["EnergyBurst"] = CreateEnergyBurstVFX();
            vfxPrefabs["SpeedBoost"] = CreateSpeedBoostVFX();
            vfxPrefabs["ShieldBubble"] = CreateShieldBubbleVFX();
            vfxPrefabs["Grab"] = CreateGrabVFX();
            vfxPrefabs["Spawn"] = CreateSpawnVFX();

            Debug.Log($"[ProceduralVFXGenerator] Generated {vfxPrefabs.Count} VFX prefabs");
        }

        /// <summary>
        /// Spawn a VFX effect at a position
        /// </summary>
        public void SpawnVFX(string vfxName, Vector3 position, Quaternion rotation, Color? color = null)
        {
            if (vfxPrefabs.TryGetValue(vfxName, out GameObject prefab))
            {
                GameObject instance = Instantiate(prefab, position, rotation);

                // Apply custom color if provided
                if (color.HasValue)
                {
                    ParticleSystem ps = instance.GetComponent<ParticleSystem>();
                    if (ps != null)
                    {
                        var main = ps.main;
                        main.startColor = color.Value;
                    }
                }

                // Auto-destroy after particle lifetime
                Destroy(instance, 3f);
            }
            else
            {
                Debug.LogWarning($"[ProceduralVFXGenerator] VFX '{vfxName}' not found!");
            }
        }

        private GameObject CreateImpactVFX()
        {
            GameObject vfx = new GameObject("ImpactVFX");
            ParticleSystem ps = vfx.AddComponent<ParticleSystem>();

            var main = ps.main;
            main.duration = 0.5f;
            main.startLifetime = 0.3f;
            main.startSpeed = 3f;
            main.startSize = 0.2f;
            main.startColor = Color.white;
            main.gravityModifier = 0.5f;
            main.maxParticles = 20;

            var emission = ps.emission;
            emission.rateOverTime = 0;
            emission.SetBursts(new ParticleSystem.Burst[] {
                new ParticleSystem.Burst(0f, 15, 20)
            });

            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.radius = 0.1f;

            var colorOverLifetime = ps.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] {
                    new GradientColorKey(Color.white, 0f),
                    new GradientColorKey(Color.gray, 1f)
                },
                new GradientAlphaKey[] {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(0f, 1f)
                }
            );
            colorOverLifetime.color = gradient;

            var sizeOverLifetime = ps.sizeOverLifetime;
            sizeOverLifetime.enabled = true;
            AnimationCurve sizeCurve = new AnimationCurve();
            sizeCurve.AddKey(0f, 1f);
            sizeCurve.AddKey(1f, 0f);
            sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, sizeCurve);

            var renderer = vfx.GetComponent<ParticleSystemRenderer>();
            renderer.material = CreateParticleMaterial();

            return vfx;
        }

        private GameObject CreateKnockoutVFX()
        {
            GameObject vfx = new GameObject("KnockoutVFX");
            ParticleSystem ps = vfx.AddComponent<ParticleSystem>();

            var main = ps.main;
            main.duration = 1f;
            main.startLifetime = 1f;
            main.startSpeed = 2f;
            main.startSize = 0.3f;
            main.startColor = new Color(1f, 0.8f, 0f, 1f); // Yellow
            main.gravityModifier = -0.5f; // Float upward
            main.maxParticles = 30;

            var emission = ps.emission;
            emission.rateOverTime = 30;

            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.angle = 25f;
            shape.radius = 0.5f;

            var colorOverLifetime = ps.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] {
                    new GradientColorKey(Color.yellow, 0f),
                    new GradientColorKey(Color.red, 0.5f),
                    new GradientColorKey(Color.black, 1f)
                },
                new GradientAlphaKey[] {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(0f, 1f)
                }
            );
            colorOverLifetime.color = gradient;

            var renderer = vfx.GetComponent<ParticleSystemRenderer>();
            renderer.material = CreateParticleMaterial();

            return vfx;
        }

        private GameObject CreateEliminationVFX()
        {
            GameObject vfx = new GameObject("EliminationVFX");
            ParticleSystem ps = vfx.AddComponent<ParticleSystem>();

            var main = ps.main;
            main.duration = 2f;
            main.startLifetime = 1.5f;
            main.startSpeed = 5f;
            main.startSize = 0.5f;
            main.startColor = new Color(1f, 0f, 0f, 1f); // Red
            main.gravityModifier = 0f;
            main.maxParticles = 50;

            var emission = ps.emission;
            emission.rateOverTime = 0;
            emission.SetBursts(new ParticleSystem.Burst[] {
                new ParticleSystem.Burst(0f, 50)
            });

            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.radius = 1f;

            var colorOverLifetime = ps.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] {
                    new GradientColorKey(Color.red, 0f),
                    new GradientColorKey(new Color(1f, 0.5f, 0f), 0.5f),
                    new GradientColorKey(Color.black, 1f)
                },
                new GradientAlphaKey[] {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(0f, 1f)
                }
            );
            colorOverLifetime.color = gradient;

            var renderer = vfx.GetComponent<ParticleSystemRenderer>();
            renderer.material = CreateParticleMaterial();

            return vfx;
        }

        private GameObject CreateEnergyBurstVFX()
        {
            GameObject vfx = new GameObject("EnergyBurstVFX");
            ParticleSystem ps = vfx.AddComponent<ParticleSystem>();

            var main = ps.main;
            main.duration = 0.5f;
            main.startLifetime = 0.5f;
            main.startSpeed = 10f;
            main.startSize = 0.3f;
            main.startColor = Color.cyan;
            main.gravityModifier = 0f;
            main.maxParticles = 100;

            var emission = ps.emission;
            emission.rateOverTime = 0;
            emission.SetBursts(new ParticleSystem.Burst[] {
                new ParticleSystem.Burst(0f, 100)
            });

            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.radius = 0.5f;

            var colorOverLifetime = ps.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] {
                    new GradientColorKey(Color.cyan, 0f),
                    new GradientColorKey(Color.blue, 1f)
                },
                new GradientAlphaKey[] {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(0f, 1f)
                }
            );
            colorOverLifetime.color = gradient;

            var renderer = vfx.GetComponent<ParticleSystemRenderer>();
            renderer.material = CreateParticleMaterial();

            return vfx;
        }

        private GameObject CreateSpeedBoostVFX()
        {
            GameObject vfx = new GameObject("SpeedBoostVFX");
            ParticleSystem ps = vfx.AddComponent<ParticleSystem>();

            var main = ps.main;
            main.duration = 2f;
            main.startLifetime = 0.5f;
            main.startSpeed = 2f;
            main.startSize = 0.2f;
            main.startColor = Color.yellow;
            main.gravityModifier = 0f;
            main.maxParticles = 50;
            main.loop = true;

            var emission = ps.emission;
            emission.rateOverTime = 25;

            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.angle = 10f;
            shape.radius = 0.3f;
            shape.rotation = new Vector3(-90f, 0f, 0f); // Trail behind

            var renderer = vfx.GetComponent<ParticleSystemRenderer>();
            renderer.material = CreateParticleMaterial();

            return vfx;
        }

        private GameObject CreateShieldBubbleVFX()
        {
            GameObject vfx = new GameObject("ShieldBubbleVFX");

            // Create sphere mesh for shield bubble
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.SetParent(vfx.transform);
            sphere.transform.localPosition = Vector3.zero;
            sphere.transform.localScale = Vector3.one * 2.5f;

            // Remove collider
            Destroy(sphere.GetComponent<Collider>());

            // Create transparent shield material
            Material shieldMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            shieldMat.SetFloat("_Surface", 1); // Transparent
            shieldMat.SetFloat("_AlphaClip", 0);
            shieldMat.SetFloat("_Blend", 0);
            shieldMat.SetFloat("_ZWrite", 0);
            shieldMat.color = new Color(0f, 1f, 1f, 0.3f);
            shieldMat.EnableKeyword("_EMISSION");
            shieldMat.SetColor("_EmissionColor", Color.cyan * 2f);

            sphere.GetComponent<Renderer>().material = shieldMat;

            // Add pulsing animation
            var pulser = vfx.AddComponent<ShieldPulser>();

            return vfx;
        }

        private GameObject CreateGrabVFX()
        {
            GameObject vfx = new GameObject("GrabVFX");
            ParticleSystem ps = vfx.AddComponent<ParticleSystem>();

            var main = ps.main;
            main.duration = 0.3f;
            main.startLifetime = 0.2f;
            main.startSpeed = 1f;
            main.startSize = 0.15f;
            main.startColor = Color.green;
            main.gravityModifier = 0f;
            main.maxParticles = 10;

            var emission = ps.emission;
            emission.rateOverTime = 0;
            emission.SetBursts(new ParticleSystem.Burst[] {
                new ParticleSystem.Burst(0f, 10)
            });

            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 0.5f;

            var renderer = vfx.GetComponent<ParticleSystemRenderer>();
            renderer.material = CreateParticleMaterial();

            return vfx;
        }

        private GameObject CreateSpawnVFX()
        {
            GameObject vfx = new GameObject("SpawnVFX");
            ParticleSystem ps = vfx.AddComponent<ParticleSystem>();

            var main = ps.main;
            main.duration = 1f;
            main.startLifetime = 1f;
            main.startSpeed = 3f;
            main.startSize = 0.3f;
            main.startColor = Color.white;
            main.gravityModifier = -1f; // Rise up
            main.maxParticles = 30;

            var emission = ps.emission;
            emission.rateOverTime = 30;

            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 1f;

            var renderer = vfx.GetComponent<ParticleSystemRenderer>();
            renderer.material = CreateParticleMaterial();

            return vfx;
        }

        private Material CreateParticleMaterial()
        {
            // Create additive particle material
            Material mat = new Material(Shader.Find("Universal Render Pipeline/Particles/Unlit"));
            mat.SetFloat("_Surface", 1); // Transparent
            mat.SetFloat("_Blend", 1); // Additive
            mat.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;
            return mat;
        }
    }

    /// <summary>
    /// Simple shield pulser component for shield bubble effect
    /// </summary>
    public class ShieldPulser : MonoBehaviour
    {
        private float pulseSpeed = 2f;
        private float pulseAmount = 0.2f;
        private Vector3 baseScale;

        private void Start()
        {
            baseScale = transform.GetChild(0).localScale;
        }

        private void Update()
        {
            float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
            transform.GetChild(0).localScale = baseScale * pulse;
        }
    }
}
