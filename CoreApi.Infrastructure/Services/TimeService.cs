using System;
using CoreApi.ApplicationCore.Contracts;

namespace CoreApi.Infrastructure.Services
{
    public class TimeService : ITimeService
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}