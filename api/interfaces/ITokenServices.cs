using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.interfaces
{
    public interface ITokenServices
    {
        string CreateToken(AppUser user);
    }
}