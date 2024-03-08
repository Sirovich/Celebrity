namespace Celebpretty.Application.Persistence;

public interface ISequencesRepository
{
    Task<int> SequenceInc(string seqName);
}
