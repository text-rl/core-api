using CoreApi.ApplicationCore.Contracts;
using CoreApi.Domain.TextTreatment;

namespace CoreApi.ApplicationCore.Write.Contracts
{
    public interface ITextTreatmentRepository : IAggregateRepository<TextTreatmentId, TextTreatmentAggregate>
    {
    }
}