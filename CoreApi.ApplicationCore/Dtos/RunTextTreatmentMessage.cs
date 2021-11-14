using System;
using CoreApi.Domain.Users;

namespace CoreApi.ApplicationCore.Dtos
{
    public record RunTextTreatmentMessage(UserId UserId, string Content, DateTime DateTime);
}