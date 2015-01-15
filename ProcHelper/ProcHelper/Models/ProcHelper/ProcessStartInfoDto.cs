using System;
using System.Diagnostics;

namespace ProcHelper
{
    public class ProcessStartInfoDto
    {
        private readonly ProcessStartInfo _processStartInfo;

        public ProcessStartInfoDto(ProcessStartInfo processStartInfo)
        {
            _processStartInfo = processStartInfo;
        }


        public string FileName
        {
            get { return _processStartInfo.FileName; }
        }

        public string Arguments
        {
            get { return _processStartInfo.Arguments; }
        }

        public string WorkingDirectory
        {
            get { return _processStartInfo.WorkingDirectory; }
        }

        public string Domain
        {
            get { return _processStartInfo.Domain; }
        }

        public string UserName
        {
            get { return _processStartInfo.UserName; }
        }

        public bool UseShellExecute
        {
            get { return _processStartInfo.UseShellExecute; }
        }

        public bool RedirectStandardOutput
        {
            get { return _processStartInfo.RedirectStandardOutput; }
        }

        public bool RedirectStandardError
        {
            get { return _processStartInfo.RedirectStandardError; }
        }

        public bool RedirectStandardInput
        {
            get { return _processStartInfo.RedirectStandardInput; }
        }

        public string Verb
        {
            get { return _processStartInfo.Verb; }
        }

        public string[] Verbs
        {
            get { return _processStartInfo.Verbs; }
        }

    }
}
