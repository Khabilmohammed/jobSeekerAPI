﻿using Microsoft.AspNetCore.Mvc.Filters;

namespace jobSeeker.Middlware
{
    public class Samplefilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
