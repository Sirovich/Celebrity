using FluentValidation;

namespace Celebpretty.Api.Models.V1.UpdateCelebrity;

public class UpdateCelebrityReqValidator : AbstractValidator<UpdateCelebrityReq>
{
    public UpdateCelebrityReqValidator()
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
