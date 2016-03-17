using System.Diagnostics;

namespace Remotus.Base
{
    public class FileVersionInfoDto
    {
        private readonly FileVersionInfo _fileVersionInfo;
        private string _fileName;
        private string _fileVersion;
        private string _productVersion;

        public FileVersionInfoDto()
        {
            
        }

        public FileVersionInfoDto(FileVersionInfo fileVersionInfo)
            : this()
        {
            _fileVersionInfo = fileVersionInfo;
        }

        public FileVersionInfo GetBase()
        {
            return _fileVersionInfo;
        }


        public string FileName
        {
            get { return _fileVersionInfo?.FileName ?? _fileName; }
            set { _fileName = value; }
        }

        public string FileVersion
        {
            get { return _fileVersionInfo?.FileVersion ?? _fileVersion; }
            set { _fileVersion = value; }
        }

        public string ProductVersion
        {
            get { return _fileVersionInfo?.ProductVersion ?? _productVersion; }
            set { _productVersion = value; }
        }
    }
}