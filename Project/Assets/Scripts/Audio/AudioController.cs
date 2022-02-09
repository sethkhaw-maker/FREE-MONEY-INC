using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Audio {
    public class AudioController : MonoBehaviour
    {
        // members of data types
        public static AudioController instance;

        public bool debug;
        public AudioTrack[] tracks;

        private Hashtable m_AudioTable; // relationship between audio types (key) and audio tracks (value)
        private Hashtable m_JobTable; // relationship between audio types (key) and jobs (value) (Coroutine, IEnumerator)


        [System.Serializable]
        public class AudioObject {
            public AudioType type;
            public AudioClip clip;
        }
        [System.Serializable]
        public class AudioTrack {
            public AudioSource source;
            public AudioObject[] audio;
        }

        #region Unity Functions
        private void Awake()
        {
            //Awake always gets called before Start
            if (!instance)
            {
                instance = this;
                Configure();
            }
        }

        private void OnDisable()
        {
            Dispose();
        }
        #endregion

        #region Public Functions
        public void PlayAudio(AudioType _type)
        {

        }
        public void StopAudio(AudioType _type)
        {

        }
        public void RestartAudio(AudioType _type)
        {

        }

        #endregion

        #region Private Functions
        private void Configure()
        {
            instance = this;
            m_AudioTable = new Hashtable();
            m_JobTable = new Hashtable();
            GenerateAudioTable();
        }
        private void Dispose()
        {

        }

        private void GenerateAudioTable()
        {

        }

        private void Log(string _msg) {
            if (!debug) return;
            Debug.Log("[Audio Controller]: " + _msg);
        }

        private void LogWarning(string _msg)
        {
            if (!debug) return;
            Debug.LogWarning("[Audio Controller]: " + _msg);

        }


        #endregion 
    }
}
