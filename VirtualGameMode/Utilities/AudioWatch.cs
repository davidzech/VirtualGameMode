using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Windows;
using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;
namespace VirtualGameMode.Utilities
{
    public class AudioWatch
    {

        private static readonly CoreAudioController audioController;

        static AudioWatch()
        {
            audioController = new CoreAudioController();
        }

        private static IDisposable observer;

        public static void Start() { 
        
            observer = audioController.AudioDeviceChanged.Subscribe(async x => {
                if (x.ChangedType == DeviceChangedType.DefaultChanged) {
                    if (x.Device.IsDefaultDevice && !x.Device.IsDefaultCommunicationsDevice)
                    {
                        await x.Device.SetAsDefaultCommunicationsAsync();
                    }
                }
            });
        }

        public static void Stop()
        {
            observer?.Dispose();
        }
    }
}
