using System.Diagnostics;

namespace FullCtrl.Base
{
    public class ProcessModuleDto
    {
        private readonly ProcessModule _processModule;
        private string _fileName;
        private FileVersionInfoDto _fileVersionInfo;

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
    }
}
