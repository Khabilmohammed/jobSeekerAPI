using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.IWhetherForCastService
{
    public interface IWheatherForcaset
    {
        IEnumerable<WeatherForecast> Get();
    }
}
