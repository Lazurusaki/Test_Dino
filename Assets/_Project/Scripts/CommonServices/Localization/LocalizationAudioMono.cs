using UnityEngine;

namespace _Project.Scripts.CommonServices.Localization
{
    public class LocalizationAudioMono : MonoBehaviour
    {
        [field:SerializeField] public LocalizationAudioKey AudioKey { get; private set; }
    }
}