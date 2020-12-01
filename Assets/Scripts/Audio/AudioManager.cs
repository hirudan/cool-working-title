using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {

        private string musicFolder = Path.Combine("Music", "stage01");
        private string introTrackName = "bgm_stage01{0}_01";
        private string sectionOneTrackName = "bgm_stage01{0}_02";
        private string transitionTrackName = "bgm_stage01{0}_03";
        private string sectionTwoTrackName = "bgm_stage01{0}_04";
        private string sectionThreeTrackName = "bgm_stage01{0}_05";
        private string outroTrackName = "bgm_stage01{0}_06";
        private string bossTrackName = "bgm_stage01{0}_boss";
        private string resultsTrackName = "bgm_end";

        public AudioClip[] currentMusicBuffer;
        public AudioClip[] currentSliptimeBuffer;
        public AudioClip[] preloadMusicBuffer;
        public AudioClip[] preloadSliptimeBuffer;

        public string currentSong;
        public string nextSong;
        public float slipTimeSlowDown = 0.5f;

        // Used for main music
        public AudioSource sourceMain;

        // Used for Sliptime Effects
        public AudioSource sourceSecondary;

        public bool hasLoopSection = false;
        public bool nextHasLoopSection = false;
        public bool inLoopSection = false;

        // Beats per second
        public float bps = 133f / 60;
        // When we can switch to next track
        public int barsMod = 8;
        private float quantumSeconds;
        public bool inSlipTime = false;
        public bool awaitingNextTrack = false;
        public bool awaitingNextLoop = false;
        private IEnumerator fadeOut;
        public float secondsElapsed = 0.0f;

        float nfmod(float a,float b)
        {
            return a - b * Mathf.Floor(a / b);
        }

        private AudioClip[] LoadMusic(string trackName, bool hasLoopSection = false, bool slipTimeVariant = false)
        {
            string subfolder = "normal";
            if (slipTimeVariant)
            {
                trackName = string.Format(trackName, "_sliptime");
                subfolder = "sliptime";
            }
            else
            {
                trackName = string.Format(trackName, "");
            }

            if (!hasLoopSection)
            {
                return new[] { Resources.Load<AudioClip>(Path.Combine(musicFolder, subfolder, trackName)) };
            }

            string firstTrack = trackName + "-first";
            string loopTrack = trackName + "-loop";
            return new [] { Resources.Load<AudioClip>(Path.Combine(musicFolder, subfolder, firstTrack)),
                            Resources.Load<AudioClip>(Path.Combine(musicFolder, subfolder, loopTrack)) };
        }

        public void Play(bool immediate = false)
        {
            sourceMain.clip = currentMusicBuffer[0];
            sourceMain.loop = false;
            sourceMain.time = 0f;

            sourceSecondary.clip = currentSliptimeBuffer[0];
            sourceSecondary.loop = false;
            sourceSecondary.time = 0f;

            inLoopSection = false;

            if (inSlipTime)
            {
                sourceMain.Pause();
                sourceSecondary.Play();
            }
            else
            {
                sourceSecondary.Pause();
                sourceMain.Play();
            }
        }

        protected void PlayLoopTrack()
        {
            // We can't play the next track yet
            if (inSlipTime)
            {
                if (secondsElapsed <= 2f && sourceSecondary.time - (nfmod(sourceSecondary.time, quantumSeconds) * quantumSeconds) > 0.1f)
                {
                    Debug.Log(sourceSecondary.time - (nfmod(sourceSecondary.time, quantumSeconds) * quantumSeconds));
                    awaitingNextTrack = true;
                    secondsElapsed += Time.deltaTime;
                    return;
                }
                Debug.Log(sourceSecondary.time - (nfmod(sourceSecondary.time, quantumSeconds) * quantumSeconds));
            }
            else
            {
                if (secondsElapsed <= 2f && sourceMain.time - (nfmod(sourceMain.time, quantumSeconds) * quantumSeconds) > 0.1f)
                {
                    Debug.Log(sourceMain.time - (nfmod(sourceMain.time, quantumSeconds) * quantumSeconds));
                    awaitingNextTrack = true;
                    secondsElapsed += Time.deltaTime;
                    return;
                }
                Debug.Log(sourceMain.time - (nfmod(sourceMain.time, quantumSeconds) * quantumSeconds));
            }

            secondsElapsed = 0.0f;

            sourceMain.clip = currentMusicBuffer[1];
            sourceMain.loop = true;
            sourceMain.time = 0f;

            sourceSecondary.clip = currentSliptimeBuffer[1];
            sourceSecondary.loop = true;
            sourceSecondary.time = 0f;

            inLoopSection = true;
            awaitingNextLoop = false;

            if (inSlipTime)
            {
                sourceMain.Pause();
                sourceSecondary.Play();
            }
            else
            {
                sourceMain.Play();
                sourceSecondary.Pause();
            }
        }

        /// <summary>
        /// Gets next song name. Returns a boolean indicating if there is a loop part to the song.
        /// </summary>
        public bool SetNextSongState()
        {
            bool hasLoop = true;
            if (currentSong == introTrackName) { nextSong = sectionOneTrackName; }
            else if (currentSong == sectionOneTrackName) { nextSong = transitionTrackName; hasLoop = false; }
            else if (currentSong == transitionTrackName) { nextSong = sectionTwoTrackName; }
            else if (currentSong == sectionTwoTrackName) { nextSong = sectionThreeTrackName; }
            else if (currentSong == sectionThreeTrackName) { nextSong = outroTrackName; hasLoop = false; }
            else if (currentSong == outroTrackName) { nextSong = bossTrackName; }
            else if (currentSong == bossTrackName) { nextSong = resultsTrackName; }
            else { nextSong = introTrackName; hasLoop = false; }
            return hasLoop;
        }

        public void PlayNextSection(bool force = false)
        {
            // If we absolutely want to cut over to the next song now, fade out the current one
            if (force)
            {
                StartCoroutine(fadeOut);
            }
            // We can't play the next track yet
            if (inSlipTime)
            {
                if (secondsElapsed <= 2f && sourceSecondary.time - (nfmod(sourceSecondary.time, quantumSeconds) * quantumSeconds) > 0.1f)
                {
                    Debug.Log(sourceSecondary.time - (nfmod(sourceSecondary.time, quantumSeconds) * quantumSeconds));
                    awaitingNextTrack = true;
                    secondsElapsed += Time.deltaTime;
                    return;
                }
                Debug.Log(sourceSecondary.time - (nfmod(sourceSecondary.time, quantumSeconds) * quantumSeconds));
            }
            else
            {
                if (secondsElapsed <= 2f && sourceMain.time - (nfmod(sourceMain.time, quantumSeconds) * quantumSeconds) > 0.1f)
                {
                    Debug.Log(sourceMain.time - (nfmod(sourceMain.time, quantumSeconds) * quantumSeconds));
                    awaitingNextTrack = true;
                    secondsElapsed += Time.deltaTime;
                    return;
                }
                Debug.Log(sourceMain.time - (nfmod(sourceMain.time, quantumSeconds) * quantumSeconds));
            }
            currentSong = nextSong;
            Debug.Log(sourceMain.time - (nfmod(sourceMain.time, quantumSeconds) * quantumSeconds));
            secondsElapsed = 0.0f;

            // Get the next song state two states forward to preload
            bool hasLoop = SetNextSongState();

            // Load previous preload to current buffer
            // and play song
            currentMusicBuffer = preloadMusicBuffer;
            currentSliptimeBuffer = preloadSliptimeBuffer;
            hasLoopSection = nextHasLoopSection;
            awaitingNextTrack = false;

            // Play the song
            StopCoroutine(fadeOut);
            Play();
            // Load next song
            preloadMusicBuffer = LoadMusic(nextSong, hasLoop);
            preloadSliptimeBuffer = LoadMusic(nextSong, hasLoop, true);
            nextHasLoopSection = hasLoop;
        }

        public void ToggleSliptimeTrack()
        {
            if (inSlipTime)
            {
                sourceMain.time = sourceSecondary.time * slipTimeSlowDown;
                sourceMain.loop = sourceMain.loop;
                sourceSecondary.Pause();
                sourceMain.Play();
                inSlipTime = false;
                return ;
            }

            sourceSecondary.time = sourceMain.time / slipTimeSlowDown;
            sourceSecondary.loop = sourceMain.loop;
            sourceMain.Pause();
            sourceSecondary.Play();
            inSlipTime = true;
        }

        // Shamelessly copied from https://forum.unity.com/threads/fade-out-audio-source.335031/
        public static IEnumerator FadeOut (AudioSource audioSource, float FadeTime) {
            float startVolume = audioSource.volume;

            while (audioSource.volume > 0) {
                audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume;
        }

        private void Start()
        {
            fadeOut = FadeOut(sourceMain, 1.0f);
            // Seconds when we are able to toggle music
            quantumSeconds = bps * barsMod;

            currentMusicBuffer = LoadMusic(introTrackName);
            currentSliptimeBuffer = LoadMusic(introTrackName, false, true);
            currentSong = introTrackName;
            hasLoopSection = false;

            sourceMain.clip = currentMusicBuffer[0];

            bool hasLoop = SetNextSongState();
            nextHasLoopSection = hasLoop;

            // Preload the next song
            preloadMusicBuffer = LoadMusic(nextSong, hasLoop);
            preloadSliptimeBuffer = LoadMusic(nextSong, hasLoop, true);
        }

        private void Update()
        {
            if (awaitingNextTrack)
            {
                PlayNextSection();
                return;
            }
            else if (awaitingNextLoop)
            {
                PlayLoopTrack();
                return;
            }

            // Go to next section if non-looping clip
            // (which means transition) and not immediately playing.
            //
            // Once we hit a looping section, we will not continue until
            // Level calls PlayNextSection() manually.
            if (inSlipTime)
            {
                if (sourceSecondary.clip.length > sourceSecondary.time) { return; }
                else if (hasLoopSection && !inLoopSection) { PlayLoopTrack(); }
                else if (!inLoopSection) { PlayNextSection(); }
            }
            else
            {
                // Song not finished yet, do not transition
                if (sourceMain.clip.length > sourceMain.time) { return; }
                else if (hasLoopSection && !inLoopSection) { PlayLoopTrack(); }
                else if (!inLoopSection) { PlayNextSection(); }
            }
        }
    }
}
