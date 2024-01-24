using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoDialer.Contracts
{
    public interface IPhoneRepository
    {
        string? GetPhone();
        void SetPhoneAsUsed();
    }
}
