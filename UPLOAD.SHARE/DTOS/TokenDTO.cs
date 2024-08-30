using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPLOAD.SHARE.DTOS
{
    //
    /// <summary>
    /// la repuesta cuando me logue...me devuelve el token y la fecha de expiracion
    /// </summary>
    public class TokenDTO
    {
        public string Token { get; set; } = null!;

        public DateTime Expiration { get; set; }

    }
}
