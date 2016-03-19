using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace Remotus.Base
{
    public class ProcessModuleDto
    {
        private readonly ProcessModule _processModule;
        private string _fileName;
        private FileVersionInfoDto _fileVersionInfo;
        private Bitmap _iconRaw;

        public ProcessModuleDto()
        {
            
        }

        public ProcessModuleDto(ProcessModule processModule)
            : this()
        {
            _processModule = processModule;
        }

        public ProcessModule GetBase()
        {
            return _processModule;
        }


        public string FileName
        {
            get { return _processModule?.FileName ?? _fileName; }
            set { _fileName = value; }
        }

        public FileVersionInfoDto FileVersionInfo
        {
            get
            {
                if (_processModule != null)
                    return new FileVersionInfoDto(_processModule.FileVersionInfo);
                return _fileVersionInfo;
            }
            set { _fileVersionInfo = value; }
        }


        [JsonConverter(typeof (BitmapConverter))]
        public Bitmap IconRaw
        {
            get
            {
                if (_iconRaw != null)
                    return _iconRaw;
                if (!string.IsNullOrWhiteSpace(FileName))
                    _iconRaw = GetIcon(null, FileName);
                return _iconRaw;
            }
            set { _iconRaw = value; }
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
