using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace TumbleRumble.Audio
{
    /// <summary>
    /// Central audio management system
    /// Handles music, SFX, and audio mixing
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        #region Singleton
        private static AudioManager _instance;
        public static AudioManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<AudioManager>();
                }
                return _instance;
            }
        }
        #endregion

        #region Inspector Fields
        [Header("Audio Mixers")]
        [SerializeField] private AudioMixer masterMixer;
        [SerializeField] private AudioMixerGroup musicGroup;
        [SerializeField] private AudioMixerGroup sfxGroup;

        [Header("Music")]
        [SerializeField] private AudioClip menuMusic;
        [SerializeField] private AudioClip[] gameplayMusic;
        [SerializeField] private AudioClip victoryMusic;

        [Header("SFX Collections")]
        [SerializeField] private AudioClip[] impactSounds;
        [SerializeField] private AudioClip[] jumpSounds;
        [SerializeField] private AudioClip[] grabSounds;
        [SerializeField] private AudioClip[] punchSounds;

        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private List<AudioSource> sfxPool = new List<AudioSource>();
        [SerializeField] private int sfxPoolSize = 10;
        #endregion

        #region Private Fields
        private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
        private float _masterVolume = 1f;
        private float _musicVolume = 0.7f;
        private float _sfxVolume = 1f;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeAudioSources();
            LoadVolumes();
        }

        private void Start()
        {
            PlayMenuMusic();
        }
        #endregion

        #region Initialization
        private void InitializeAudioSources()
        {
            // Create music source if not assigned
            if (musicSource == null)
            {
                GameObject musicObj = new GameObject("MusicSource");
                musicObj.transform.SetParent(transform);
                musicSource = musicObj.AddComponent<AudioSource>();
                musicSource.loop = true;
                musicSource.playOnAwake = false;
                musicSource.outputAudioMixerGroup = musicGroup;
            }

            // Create SFX pool
            for (int i = 0; i < sfxPoolSize; i++)
            {
                GameObject sfxObj = new GameObject($"SFXSource_{i}");
                sfxObj.transform.SetParent(transform);
                AudioSource sfxSource = sfxObj.AddComponent<AudioSource>();
                sfxSource.playOnAwake = false;
                sfxSource.outputAudioMixerGroup = sfxGroup;
                sfxPool.Add(sfxSource);
            }
        }

        private void LoadVolumes()
        {
            _masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
            _musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
            _sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

            ApplyVolumes();
        }
        #endregion

        #region Music Control
        public void PlayMenuMusic()
        {
            PlayMusic(menuMusic);
        }

        public void PlayGameplayMusic()
        {
            if (gameplayMusic != null && gameplayMusic.Length > 0)
            {
                int randomIndex = Random.Range(0, gameplayMusic.Length);
                PlayMusic(gameplayMusic[randomIndex]);
            }
        }

        public void PlayVictoryMusic()
        {
            PlayMusic(victoryMusic);
        }

        private void PlayMusic(AudioClip clip)
        {
            if (clip == null || musicSource == null) return;

            if (musicSource.isPlaying && musicSource.clip == clip) return;

            musicSource.clip = clip;
            musicSource.Play();
        }

        public void StopMusic()
        {
            if (musicSource != null)
            {
                musicSource.Stop();
            }
        }

        public void FadeOutMusic(float duration = 1f)
        {
            StartCoroutine(FadeOutCoroutine(duration));
        }

        private System.Collections.IEnumerator FadeOutCoroutine(float duration)
        {
            float startVolume = musicSource.volume;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                musicSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            musicSource.volume = 0f;
            musicSource.Stop();
            musicSource.volume = startVolume;
        }
        #endregion

        #region SFX Control
        public void PlaySFX(AudioClip clip, float volume = 1f)
        {
            if (clip == null) return;

            AudioSource availableSource = GetAvailableSFXSource();
            if (availableSource != null)
            {
                availableSource.PlayOneShot(clip, volume * _sfxVolume);
            }
        }

        public void PlaySFX(string clipName, float volume = 1f)
        {
            if (_audioClips.ContainsKey(clipName))
            {
                PlaySFX(_audioClips[clipName], volume);
            }
        }

        public void PlayRandomImpact()
        {
            if (impactSounds != null && impactSounds.Length > 0)
            {
                AudioClip clip = impactSounds[Random.Range(0, impactSounds.Length)];
                PlaySFX(clip);
            }
        }

        public void PlayRandomJump()
        {
            if (jumpSounds != null && jumpSounds.Length > 0)
            {
                AudioClip clip = jumpSounds[Random.Range(0, jumpSounds.Length)];
                PlaySFX(clip);
            }
        }

        public void PlayRandomGrab()
        {
            if (grabSounds != null && grabSounds.Length > 0)
            {
                AudioClip clip = grabSounds[Random.Range(0, grabSounds.Length)];
                PlaySFX(clip);
            }
        }

        public void PlayRandomPunch()
        {
            if (punchSounds != null && punchSounds.Length > 0)
            {
                AudioClip clip = punchSounds[Random.Range(0, punchSounds.Length)];
                PlaySFX(clip);
            }
        }

        private AudioSource GetAvailableSFXSource()
        {
            foreach (var source in sfxPool)
            {
                if (!source.isPlaying)
                {
                    return source;
                }
            }

            // All sources busy, return first one (will interrupt)
            return sfxPool[0];
        }
        #endregion

        #region Volume Control
        public void SetMasterVolume(float volume)
        {
            _masterVolume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat("MasterVolume", _masterVolume);
            ApplyVolumes();
        }

        public void SetMusicVolume(float volume)
        {
            _musicVolume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat("MusicVolume", _musicVolume);
            ApplyVolumes();
        }

        public void SetSFXVolume(float volume)
        {
            _sfxVolume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat("SFXVolume", _sfxVolume);
            ApplyVolumes();
        }

        private void ApplyVolumes()
        {
            if (masterMixer != null)
            {
                masterMixer.SetFloat("MasterVolume", VolumeToDecibels(_masterVolume));
                masterMixer.SetFloat("MusicVolume", VolumeToDecibels(_musicVolume));
                masterMixer.SetFloat("SFXVolume", VolumeToDecibels(_sfxVolume));
            }

            if (musicSource != null)
            {
                musicSource.volume = _musicVolume * _masterVolume;
            }
        }

        private float VolumeToDecibels(float volume)
        {
            return volume > 0f ? 20f * Mathf.Log10(volume) : -80f;
        }
        #endregion

        #region Utility
        public void RegisterAudioClip(string name, AudioClip clip)
        {
            if (!_audioClips.ContainsKey(name))
            {
                _audioClips.Add(name, clip);
            }
        }
        #endregion
    }
}
