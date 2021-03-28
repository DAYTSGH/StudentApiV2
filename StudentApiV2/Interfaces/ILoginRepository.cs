using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Interfaces
{
    public interface ILoginRepository
    {
        bool IsValid(LoginRequestDto request);
    }
}
