using System;

namespace CoreApi.ApplicationCore.Contracts
{
    public interface ITimeService
    {
        public DateTime Now();
    }
}