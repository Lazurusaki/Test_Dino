using System.Threading.Tasks;
using YG;

namespace _Project.Scripts.CommonServices.AdvertisementManagement
{
    public class AdService
    {
        public int AdsCounter { get; private set; }

        public async Task ShowFullscreenAsync()
        {
            if (!YG2.isTimerAdvCompleted)
                return;

            var tcs = new TaskCompletionSource<bool>();

            void OnAdClosed()
            {
                YG2.onCloseInterAdv -= OnAdClosed;
                tcs.SetResult(true);
            }

            YG2.onCloseInterAdv += OnAdClosed;

            YG2.InterstitialAdvShow(); 
            AdsCounter++;

            await tcs.Task;
        }
    }
}