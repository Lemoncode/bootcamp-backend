﻿using KISS.Contracts;
using KISS.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISS.CompositePattern.Validators
{
    internal class UserValidator : IValidator<User>
    {
        public void Validate(User user)
        {
            if (user is null)
            {
                throw new ArgumentNullException("El usuario es null.");
            }
        }
    }
}
