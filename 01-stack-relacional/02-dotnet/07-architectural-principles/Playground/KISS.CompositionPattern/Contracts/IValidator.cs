using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISS.Contracts;

internal interface IValidator<TEntity>
{
    void Validate(TEntity entity);
}
