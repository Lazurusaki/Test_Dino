using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.CommonServices.ScenesManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Project.Scripts.CommonServices.AudioManagement
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioService : MonoBehaviour
    {
        private const float DefaultMusicVolume = 0.3f;
        private const float DefaultSoundVolume = 1.0f;
        
        private List<AudioClip> _musicList = new();

        private int _currentIndex;
        
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void AddMusic(AudioClip audioClip)
        {
            _musicList.Add(audioClip);
        }
        
        public void AddMusic(AudioClip[] audioClips)
        {
            _musicList.AddRange(audioClips);
        }

        public void SetMusic(AudioClip music, bool play = false , float volume = DefaultMusicVolume)
        {
            _audioSource.clip = music;
            _audioSource.volume = DefaultMusicVolume;
            
            if (play)
                PlayMusic();
        }
        
        public void PlayMusic()
        {
            if (_audioSource.clip == null)
                return;
            
            _audioSource.Play();
        }

        public void PlayRandomMusic()
        {
            if (_musicList.Count == 0)
                return;

            int randomIndex;
            
            do randomIndex = Random.Range(0, _musicList.Count);
            while (_audioSource.clip == _musicList[randomIndex]);
                
            _currentIndex =  randomIndex;
            _audioSource.clip = _musicList[_currentIndex];
            PlayMusic();
        }
        
        public void PlayNextMusic()
        {
            if (_musicList.Count == 0)
                return;

            _currentIndex++;

            if (_currentIndex >= _musicList.Count)
                _currentIndex = 0;

            _audioSource.clip = _musicList[_currentIndex];
            PlayMusic();
        }

        
        public void StopMusic()
        {
            _audioSource.Stop();
        }
        
        

        public void PlaySound(AudioClip audioClip, float volume = DefaultSoundVolume)
        {
            _audioSource.PlayOneShot(audioClip, volume);
        }

        public void Clear()
        {
            _audioSource.clip = null;
            _musicList.Clear();
        }
    }
}