using UnityEngine;

namespace TumbleRumble.Audio
{
    /// <summary>
    /// Generates procedural audio clips at runtime
    /// Creates simple sound effects for impacts, jumps, UI, etc.
    /// </summary>
    public class ProceduralAudioGenerator : MonoBehaviour
    {
        private const int SAMPLE_RATE = 44100;

        /// <summary>
        /// Generate a simple impact sound
        /// </summary>
        public static AudioClip GenerateImpactSound(float duration = 0.2f, float frequency = 220f)
        {
            int sampleCount = Mathf.FloorToInt(SAMPLE_RATE * duration);
            float[] samples = new float[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                float t = i / (float)SAMPLE_RATE;

                // Decaying noise with low-pass filter
                float envelope = Mathf.Exp(-t * 10f);
                float noise = Random.Range(-1f, 1f);
                float tone = Mathf.Sin(2f * Mathf.PI * frequency * t * (1f + noise * 0.1f));

                samples[i] = (noise * 0.3f + tone * 0.7f) * envelope;
            }

            AudioClip clip = AudioClip.Create("ProceduralImpact", sampleCount, 1, SAMPLE_RATE, false);
            clip.SetData(samples, 0);
            return clip;
        }

        /// <summary>
        /// Generate a jump sound
        /// </summary>
        public static AudioClip GenerateJumpSound(float duration = 0.15f)
        {
            int sampleCount = Mathf.FloorToInt(SAMPLE_RATE * duration);
            float[] samples = new float[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                float t = i / (float)SAMPLE_RATE;
                float progress = t / duration;

                // Frequency sweep upward
                float frequency = 200f + progress * 300f;
                float envelope = 1f - progress;

                samples[i] = Mathf.Sin(2f * Mathf.PI * frequency * t) * envelope * 0.5f;
            }

            AudioClip clip = AudioClip.Create("ProceduralJump", sampleCount, 1, SAMPLE_RATE, false);
            clip.SetData(samples, 0);
            return clip;
        }

        /// <summary>
        /// Generate UI button sound
        /// </summary>
        public static AudioClip GenerateUIClick(float duration = 0.1f)
        {
            int sampleCount = Mathf.FloorToInt(SAMPLE_RATE * duration);
            float[] samples = new float[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                float t = i / (float)SAMPLE_RATE;
                float progress = t / duration;

                float frequency = 800f + Mathf.Sin(progress * Mathf.PI) * 200f;
                float envelope = 1f - progress;

                samples[i] = Mathf.Sin(2f * Mathf.PI * frequency * t) * envelope * 0.3f;
            }

            AudioClip clip = AudioClip.Create("ProceduralClick", sampleCount, 1, SAMPLE_RATE, false);
            clip.SetData(samples, 0);
            return clip;
        }

        /// <summary>
        /// Generate grab sound
        /// </summary>
        public static AudioClip GenerateGrabSound(float duration = 0.25f)
        {
            int sampleCount = Mathf.FloorToInt(SAMPLE_RATE * duration);
            float[] samples = new float[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                float t = i / (float)SAMPLE_RATE;
                float progress = t / duration;

                // Suction cup sound
                float frequency = 150f - progress * 50f;
                float envelope = Mathf.Sin(progress * Mathf.PI);
                float noise = Random.Range(-0.2f, 0.2f);

                samples[i] = (Mathf.Sin(2f * Mathf.PI * frequency * t) + noise) * envelope * 0.4f;
            }

            AudioClip clip = AudioClip.Create("ProceduralGrab", sampleCount, 1, SAMPLE_RATE, false);
            clip.SetData(samples, 0);
            return clip;
        }

        /// <summary>
        /// Generate punch/whoosh sound
        /// </summary>
        public static AudioClip GeneratePunchSound(float duration = 0.2f)
        {
            int sampleCount = Mathf.FloorToInt(SAMPLE_RATE * duration);
            float[] samples = new float[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                float t = i / (float)SAMPLE_RATE;
                float progress = t / duration;

                // Whoosh + impact
                float frequency = 100f + progress * 100f;
                float envelope = Mathf.Exp(-progress * 8f);
                float noise = Random.Range(-1f, 1f) * (1f - progress * 0.5f);

                samples[i] = (Mathf.Sin(2f * Mathf.PI * frequency * t) * 0.3f + noise * 0.7f) * envelope * 0.5f;
            }

            AudioClip clip = AudioClip.Create("ProceduralPunch", sampleCount, 1, SAMPLE_RATE, false);
            clip.SetData(samples, 0);
            return clip;
        }

        /// <summary>
        /// Generate victory fanfare
        /// </summary>
        public static AudioClip GenerateVictorySound(float duration = 1.5f)
        {
            int sampleCount = Mathf.FloorToInt(SAMPLE_RATE * duration);
            float[] samples = new float[sampleCount];

            // Simple ascending chord arpeggio
            float[] notes = new float[] { 261.63f, 329.63f, 392f, 523.25f }; // C, E, G, C

            for (int i = 0; i < sampleCount; i++)
            {
                float t = i / (float)SAMPLE_RATE;
                float progress = t / duration;

                int noteIndex = Mathf.FloorToInt(progress * notes.Length);
                noteIndex = Mathf.Clamp(noteIndex, 0, notes.Length - 1);

                float frequency = notes[noteIndex];
                float envelope = Mathf.Sin(Mathf.Frac(progress * notes.Length) * Mathf.PI);

                samples[i] = Mathf.Sin(2f * Mathf.PI * frequency * t) * envelope * 0.3f;
            }

            AudioClip clip = AudioClip.Create("ProceduralVictory", sampleCount, 1, SAMPLE_RATE, false);
            clip.SetData(samples, 0);
            return clip;
        }

        /// <summary>
        /// Generate elimination sound
        /// </summary>
        public static AudioClip GenerateEliminationSound(float duration = 1f)
        {
            int sampleCount = Mathf.FloorToInt(SAMPLE_RATE * duration);
            float[] samples = new float[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                float t = i / (float)SAMPLE_RATE;
                float progress = t / duration;

                // Descending whoosh
                float frequency = 400f * (1f - progress);
                float envelope = Mathf.Exp(-progress * 3f);
                float wobble = Mathf.Sin(t * 20f) * 0.1f;

                samples[i] = Mathf.Sin(2f * Mathf.PI * frequency * (1f + wobble) * t) * envelope * 0.4f;
            }

            AudioClip clip = AudioClip.Create("ProceduralElimination", sampleCount, 1, SAMPLE_RATE, false);
            clip.SetData(samples, 0);
            return clip;
        }

        /// <summary>
        /// Generate simple background music loop
        /// </summary>
        public static AudioClip GenerateSimpleMusic(float duration = 4f)
        {
            int sampleCount = Mathf.FloorToInt(SAMPLE_RATE * duration);
            float[] samples = new float[sampleCount];

            // Simple bass + lead pattern
            float[] bassNotes = new float[] { 130.81f, 164.81f, 196f, 164.81f }; // C, E, G, E
            float[] leadNotes = new float[] { 523.25f, 659.25f, 783.99f, 659.25f }; // High C, E, G, E

            for (int i = 0; i < sampleCount; i++)
            {
                float t = i / (float)SAMPLE_RATE;
                float progress = (t % duration) / duration;

                int noteIndex = Mathf.FloorToInt(progress * bassNotes.Length) % bassNotes.Length;

                float bass = Mathf.Sin(2f * Mathf.PI * bassNotes[noteIndex] * t) * 0.2f;
                float lead = Mathf.Sin(2f * Mathf.PI * leadNotes[noteIndex] * t) * 0.1f;
                float envelope = Mathf.Sin(Mathf.Frac(progress * bassNotes.Length) * Mathf.PI);

                samples[i] = (bass + lead) * envelope;
            }

            AudioClip clip = AudioClip.Create("ProceduralMusic", sampleCount, 1, SAMPLE_RATE, false);
            clip.SetData(samples, 0);
            return clip;
        }
    }
}
