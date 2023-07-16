using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tes3EditX.Backend.Models
{
    public class ConflictsChangedMessage : ValueChangedMessage<Dictionary<string, List<string>>>
    {
        public ConflictsChangedMessage(Dictionary<string, List<string>> conflicts) : base(conflicts)
        {
            // 
        }
    }
}
