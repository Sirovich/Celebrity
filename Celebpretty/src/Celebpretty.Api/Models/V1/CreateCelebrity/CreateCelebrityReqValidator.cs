using FluentValidation;

namespace Celebpretty.Api.Models.V1.CreateCelebrity;

public class CreateCelebrityReqValidator : AbstractValidator<CreateCelebrityReq>
{
    public CreateCelebrityReqValidator()
    {
        RuleFor(req => req.BirthDate)
            .NotEmpty();

        RuleFor(req => req.Name)
            .NotEmpty();

        RuleFor(req => req.Role)
            .NotEmpty();

        RuleFor(req => req.Image)
            .NotEmpty();

        RuleFor(req => req.Gender)
            .NotEmpty();
    }
}
