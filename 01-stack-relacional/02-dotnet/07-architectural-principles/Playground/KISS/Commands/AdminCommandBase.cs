using KISS.Commands.CommandEntities;
using KISS.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISS.Commands
{
    internal class AdminCommandBase<TRequest, TResponse> : LoggedinUserCommandBase<TRequest, TResponse>
        where TRequest : CommandRequestBase
        where TResponse : CommandResponseBase, new()
    {
        public AdminCommandBase(TRequest request, User user) : base(request, user)
        {
        }

        public override void Validate()
        {
            if (!User.IsAdmin)
            {
                throw new InvalidOperationException("El usuario no es administrador.");
            }
            base.Validate();
        }
    }
}
