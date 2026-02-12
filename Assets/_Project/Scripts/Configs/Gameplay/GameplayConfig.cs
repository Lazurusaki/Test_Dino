using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Configs.Gameplay
{
    [CreateAssetMenu(fileName = "GameplayConfig", menuName = "Configs/Gameplay/GameplayConfig")]
    public class GameplayConfig : ScriptableObject
    {
        [field: SerializeField] public AssetReferenceT<AudioClip> StartGameSound { get; private set; }

        [field: SerializeField] public AssetReferenceT<AudioClip> WinSound { get; private set; }
        [field: SerializeField] public AssetReferenceT<AudioClip> LooseSound { get; private set; }
        [field: SerializeField] public AssetReferenceT<AudioClip> CheersSound { get; private set; }
        [field: SerializeField] public AssetReferenceT<AudioClip> BalloonHitSound { get; private set; }
        [field: SerializeField] public List<AssetReferenceT<AudioClip>> GameplayMusicList { get; private set; }
        
        [field: SerializeField] public AssetReference WinEffect { get; private set; }
        [field: SerializeField] public AssetReference BalloonHitEffect { get; private set; }
    }
}