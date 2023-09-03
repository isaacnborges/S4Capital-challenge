using FluentValidation;
using S4Capital.Api.Api.Dtos.Commands;

namespace S4Capital.Api.Domain.Validators;

public class EditPostCommandValidator : AbstractValidator<EditPostCommand>
{
    public EditPostCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("[Title] must be informed");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("[Content] must be informed");
    }
}
