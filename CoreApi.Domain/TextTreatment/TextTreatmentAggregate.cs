using System;
using CoreApi.Domain.Users;

namespace CoreApi.Domain.TextTreatment
{
    public record TextTreatmentId(Guid Value);

    public class TextTreatmentAggregate : Aggregate<TextTreatmentId>
    {
        public UserId UserId { get; private set; }
        public string Content { get; private set; }

        public TextTreatmentAggregate(TextTreatmentId id, UserId userId, string content) : base(id)
        {
            UserId = userId;
            Content = content;
        }
    }
}