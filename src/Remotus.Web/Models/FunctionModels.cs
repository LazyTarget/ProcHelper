﻿using Remotus.Base;

namespace Remotus.Web.Models
{
    public class PluginViewModel
    {
        public string PluginID { get; set; }
        public string PluginName { get; set; }

        public FunctionViewModel[] Functions { get; set; }
    }

    public class FunctionViewModel
    {
        public string FunctionID { get; set; }
        public string FunctionName { get; set; }

        public IParameterCollection ParameterCollection { get; set; }
    }
}