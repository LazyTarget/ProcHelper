using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FullCtrl.Base;

namespace FullCtrl.API.Models
{
    public class ExecutionContext : IExecutionContext
    {
        public IClientInfo ClientInfo { get; set; }
        public ILog Logger { get; set; }
    }
}
