using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {

        private string musicFolder = Path.Combine("Music", "stage01");
        private string introTrackName = "bgm_stage01_01";
        private string sectionOneTrackName = "bgm_stage01_02";
        private string transitionTrackName = "bgm_stage01_03";
        private string sectionTwoTrackName = "bgm_stage01_04";
        private string sectionThreeTrackName = "bgm_stage01_05";
        private string outroTrackName = "bgm_stage01_06";

        public AudioClip[] currentMusicBuffer;
        public AudioClip[] preloadMusicBuffer;

        public string currentSong;

        // Used for main music
        public AudioSource sourceMain;

        // Used for Sliptime Effects
        public AudioSource sourceSecondary;

        public bool inLoopSection = false;

        private AudioClip[] LoadMusic(string trackName, bool hasLoopSection = false)
        {
            if (!hasLoopSection)
            {
                return new[] { Resources.Load<AudioClip>(Path.Combine(musicFolder, "normal", trackName)) };
            }

            string firstTrack = trackName + "-first";
            string loopTrack = trackName + "-loop";
            return new [] { Resources.Load<AudioClip>(Path.Combine(musicFolder, "normal", firstTrack)),
                            Resources.Load<AudioClip>(Path.Combine(musicFolder, "normal", loopTrack)) };
        }

        public void Play(bool immediate = false)
        {
            inLoopSection = false;
            sourceMain.clip = currentMusicBuffer[0];
            sourceMain.loop = false;
            sourceMain.Play();

            // Go to next section if non-looping clip
            // (which means transition) and not immediately playing.
            //
            // Once we hit a looping section, we will not continue until
            // Level calls PlayNextSection() manually.
            if (currentMusicBuffer.Length == 1)
            {
                Invoke("PlayNextSection", currentMusicBuffer[0].length);
            }
            else
            {
                Invoke("PlayLoopTrack", currentMusicBuffer[0].length);
            }
        }

        protected void PlayLoopTrack()
        {
            sourceMain.clip = currentMusicBuffer[1];
            sourceMain.loop = true;
            sourceMain.Play();
            inLoopSection = true;
        }

        /// <summary>
        /// Get's next song name. Returns a boolean indicating if there is a loop part to the song.
        /// </summary>
        public bool SetNextSongState()
        {
            bool hasLoop = true;
            if (currentSong == introTrackName) { currentSong = sectionOneTrackName; }
            else if (currentSong == sectionOneTrackName) { currentSong = transitionTrackName; hasLoop = false; }
            else if (currentSong == transitionTrackName) { currentSong = sectionTwoTrackName; }
            else if (currentSong == sectionTwoTrackName) { currentSong = sectionThreeTrackName; }
            else if (currentSong == sectionThreeTrackName) { currentSong = outroTrackName; hasLoop = false; }
            else { currentSong = introTrackName; }
            return hasLoop;
        }

        public void PlayNextSection()
        {
            bool hasLoop = SetNextSongState();

            // Load previous preload to current buffer
            // and play song
            currentMusicBuffer = preloadMusicBuffer;

            // Play the song
            Play(true);

            // Load next song
            preloadMusicBuffer = LoadMusic(currentSong, hasLoop);
        }

        public void ToggleSliptimeTrack()
        {
            // TODO in next MR.
        }

        private void Start()
        {
            currentMusicBuffer = LoadMusic(introTrackName);
            currentSong = introTrackName;
            sourceMain.clip = currentMusicBuffer[0];

            bool hasLoop = SetNextSongState();

            // Preload the next song
            preloadMusicBuffer = LoadMusic(currentSong, hasLoop);

            // Set the state back to introTrackName since we are done preloading
            currentSong = introTrackName;
        }
    }
}
