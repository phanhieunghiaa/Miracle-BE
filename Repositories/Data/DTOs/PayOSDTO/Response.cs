using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Data.DTOs.PayOSDTO
{
    public record Response(
    int error,
    String message,
    object? data
);
}
