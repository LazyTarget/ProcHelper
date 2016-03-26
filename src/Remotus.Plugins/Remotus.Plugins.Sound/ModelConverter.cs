using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace Remotus.Plugins.Sound
{
    public class ModelConverter
    {
        public virtual AudioSession FromAudioSession(AudioSwitcher.AudioApi.Session.IAudioSession state)
        {
            if (state == null)
                return null;

            var icon = GetIcon(state.IconPath, state.ExecutablePath);
            var result = new AudioSession
            {
                ID = state.Id,
                DeviceID = state.Device.Id,
                Name = state.DisplayName,
                IconPath = state.IconPath,
                IconRaw = icon,
                Muted = state.IsMuted,
                Volume = state.Volume,
                IsSystemSession = state.IsSystemSession,
            };
            return result;
        }

        public virtual AudioDevice FromAudioDevice(AudioSwitcher.AudioApi.CoreAudio.CoreAudioDevice state)
        {
            if (state == null)
                return null;

            var icon = GetIcon(state.IconPath, null);
            var result = new AudioDevice
            {
                ID = state.Id,
                FriendlyName = state.Name,
                InterfaceName = state.InterfaceName,
                Muted = state.IsMuted,
                Volume = state.Volume,
                DeviceType = (AudioDeviceType)Enum.Parse(typeof(AudioDeviceType), ((int)state.DeviceType).ToString()),
                State = (AudioDeviceState)Enum.Parse(typeof(AudioDeviceState), ((int)state.State).ToString()),
                DeviceIconType = (AudioDeviceIcon)Enum.Parse(typeof(AudioDeviceIcon), ((int)state.Icon).ToString()),
                IconPath = state.IconPath,
                IconRaw = icon,
                DefaultDevice = state.IsDefaultDevice,
                IsDefaultCommunicationsDevice = state.IsDefaultCommunicationsDevice,
            };
            return result;
        }




        [DllImport("shell32.dll", EntryPoint = "ExtractIcon")]
        private extern static IntPtr ExtractIcon(IntPtr hInst, string lpszExeFileName, int nIconIndex);

        protected virtual Bitmap GetIcon(string iconReference, string exePath)
        {
            Bitmap icon = null;
            if (string.IsNullOrWhiteSpace(iconReference) && string.IsNullOrWhiteSpace(exePath))
                return null;

            if (!string.IsNullOrWhiteSpace(iconReference))
            {
                try
                {
                    var iconPath = iconReference;
                    var iconIdx = 0;
                    if (iconPath.IndexOf(',') >= 0)
                    {
                        var i = iconPath.IndexOf(',');
                        iconPath = iconReference.Substring(0, i).Trim('@').Trim();
                        iconPath = Environment.ExpandEnvironmentVariables(iconPath);
                        int.TryParse(iconReference.Substring(i + 1).Trim(), out iconIdx);
                    }
                    
                    if (File.Exists(iconPath))
                    {
                        var ptr = ExtractIcon(IntPtr.Zero, iconPath, iconIdx);
                        Icon ico = Icon.FromHandle(ptr);
                        icon = ico?.ToBitmap();
                    }
                }
                catch (Exception ex)
                {

                }

                if (icon != null)
                    return icon;
                else
                {
                    
                }
            }
            
            if (!string.IsNullOrWhiteSpace(exePath))
            {
                try
                {
                    var ico = Icon.ExtractAssociatedIcon(exePath);
                    icon = ico?.ToBitmap();
                }
                catch (Exception ex)
                {

                }

                if (icon != null)
                    return icon;
                else
                {
                    
                }
            }
            return icon;
        }

    }
}
