using System.Diagnostics;

namespace Remotus.Base
{
    public class ProcessStartInfoDto
    {
        private readonly ProcessStartInfo _processStartInfo;
        private string _fileName;
        private string _arguments;
        private string _workingDirectory;
        private string _domain;
        private string _userName;
        private bool _useShellExecute;
        private bool _redirectStandardOutput;
        private bool _redirectStandardError;
        private bool _redirectStandardInput;
        private string _verb;
        private string[] _verbs;

        public ProcessStartInfoDto()
        {
            
        }

        public ProcessStartInfoDto(ProcessStartInfo processStartInfo)
            : this()
        {
            _processStartInfo = processStartInfo;
        }


        public string FileName
        {
            get { return _processStartInfo?.FileName ?? _fileName; }
            set { _fileName = value; }
        }

        public string Arguments
        {
            get { return _processStartInfo?.Arguments ?? _arguments; }
            set { _arguments = value; }
        }

        public string WorkingDirectory
        {
            get { return _processStartInfo?.WorkingDirectory ?? _workingDirectory; }
            set { _workingDirectory = value; }
        }

        public string Domain
        {
            get { return _processStartInfo?.Domain ?? _domain; }
            set { _domain = value; }
        }

        public string UserName
        {
            get { return _processStartInfo?.UserName ?? _userName; }
            set { _userName = value; }
        }

        public bool UseShellExecute
        {
            get { return _processStartInfo?.UseShellExecute ?? _useShellExecute; }
            set { _useShellExecute = value; }
        }

        public bool RedirectStandardOutput
        {
            get { return _processStartInfo?.RedirectStandardOutput ?? _redirectStandardOutput; }
            set { _redirectStandardOutput = value; }
        }

        public bool RedirectStandardError
        {
            get { return _processStartInfo?.RedirectStandardError ?? _redirectStandardError; }
            set { _redirectStandardError = value; }
        }

        public bool RedirectStandardInput
        {
            get { return _processStartInfo?.RedirectStandardInput ?? _redirectStandardInput; }
            set { _redirectStandardInput = value; }
        }

        public string Verb
        {
            get { return _processStartInfo?.Verb ?? _verb; }
            set { _verb = value; }
        }

        public string[] Verbs
        {
            get { return _processStartInfo?.Verbs ?? _verbs; }
            set { _verbs = value; }
        }

    }
}
