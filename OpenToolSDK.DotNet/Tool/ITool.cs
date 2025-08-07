using OpenToolSDK.DotNet.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenToolSDK.DotNet.Tool
{
    public abstract class ITool
    {
        public abstract Task<Dictionary<string, object>> Call(string name, Dictionary<string, object> arguments);

        public virtual Task<OpenTool> Load()
        {
            return Task.FromResult<OpenTool>(null);
        }
    }
}
