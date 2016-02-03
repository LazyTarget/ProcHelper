using System;

namespace FullCtrl.Base
{
    public interface IProcessDto
    {
        int Id { get; }
        string ProcessName { get; }
        string MachineName { get; }
        ProcessStartInfoDto StartInfo { get; }
        DateTime StartTime { get; }
        bool Responding { get; }
        bool HasExited { get; }
        int ExitCode { get; }
        DateTime ExitTime { get; }
        TimeSpan PrivilegedProcessorTime { get; }
        TimeSpan TotalProcessorTime { get; }
        TimeSpan UserProcessorTime { get; }
        ProcessModuleDto MainModule { get; }
        string StandardError { get; }
        string StandardOutput { get; }
    }
}