using System;

namespace CoreApi.ApplicationCore.Dtos
{
    public record RunTextTreatmentDto(Guid UserId, string Content, DateTime DateTime);
}