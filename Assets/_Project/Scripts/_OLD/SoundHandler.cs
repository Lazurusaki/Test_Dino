using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Configs.Localization;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField]
    private List<AudioClip> balloonClips = new List<AudioClip>();

    [SerializeField]
    private AudioClip failClip;
    [SerializeField]
    private AudioClip winClip;
    [SerializeField]
    private AudioClip completePuzzleClip;
    [SerializeField]
    private AudioClip startClip;
    [SerializeField]
    private AudioClip clapsClip;
    [SerializeField]
    private AudioClip EnglishNameClip;
    [SerializeField]
    private AudioClip RussianNameClip;

    [SerializeField]
    private float startClipTime = 0.5f;
	
 public AudioClip[] MusicList;
 public AudioSource music_src2;
 
  int RndMusic=0;
  bool EndSound= false;
      public void RandomMusic()
    {
    RndMusic++;
	if (!EndSound) {
            if (RndMusic == MusicList.Length) {
                music_src2.Stop();
             //   ButtonMusic.GetComponent<Animator>().enabled = false;
              //  ButtonMusic.GetComponent<Image>().sprite = MusicSprite[0];
                RndMusic = -1;
            } else {
             //   ButtonMusic.GetComponent<Image>().sprite = MusicSprite[RndMusic + 1];
             //   ButtonMusic.GetComponent<Animator>().enabled = true;
                music_src2.clip = MusicList[RndMusic];
                music_src2.Play();


	}}
	else
	{
		PlayCompleteClip();
	}
      
    }
	
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        StartCoroutine(StartClip());
		
		
		    RndMusic = UnityEngine.Random.Range(0, MusicList.Length);
        music_src2.clip = MusicList[RndMusic];
        
		// Если взрослый режим отключим музыку
	        music_src2.Play();
		

	
	
    }

    public void PlayWinClip() => _audioSource.PlayOneShot(winClip);
	

    public void PlayFailClip() => _audioSource.PlayOneShot(failClip);

    public void PlayBalloonClip() => _audioSource.PlayOneShot(balloonClips[Random.Range(0, balloonClips.Count - 1)]);

    public void PlayCompleteClip() => _audioSource.PlayOneShot(completePuzzleClip);

    public void PlayStartClip() => _audioSource.PlayOneShot(startClip);

    public void PlayNameClip()
    {
		if (!EndSound) {
        AudioClip nameClip = (LanguageHandler.language == Language.English) ? EnglishNameClip : RussianNameClip;
        _audioSource.PlayOneShot(nameClip);
		}
	EndSound=true;
    }

    public void PlayClapsClip()
    {
        _audioSource.PlayOneShot(clapsClip);
	
    }

    public void WaitPlayNameClip()
    {
        StartCoroutine(StartNameClip());
		
	
    }

    private IEnumerator StartClip()
    {
        yield return new WaitForSeconds(startClipTime);
        PlayStartClip();
	
    }

    private IEnumerator StartNameClip()
    {
			if (!EndSound) {
		          music_src2.Stop();
        yield return new WaitForSeconds(0.5f);

        PlayNameClip();
		yield return new WaitForSeconds(1f);
			PlayCompleteClip();
			}
    }

}
