using Infrastructure.Data.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public static class ExceptionMessage
    {
        public static void CatchException(Exception ex)
        {
            Logs.Error(ex.ToString());
            throw ex;
        }
    }
}
