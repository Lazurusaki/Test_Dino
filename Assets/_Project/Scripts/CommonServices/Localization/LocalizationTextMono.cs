using TMPro;
using UnityEngine;

namespace _Project.Scripts.CommonServices.Localization
{
    public class LocalizationTextMono : MonoBehaviour
    {
        [field:SerializeField] public LocalizationAudioKey AudioKey { get; private set; }
        [field:SerializeField] public TMP_Text Text { get; private set; }
    }
}