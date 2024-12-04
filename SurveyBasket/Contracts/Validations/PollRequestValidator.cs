using SurveyBasket.Contracts.Requests;

namespace SurveyBasket.Contracts.Validations
{
    public class PollRequestValidator : AbstractValidator<PollRequest>
    {
        public PollRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .Length(3, 100);
            RuleFor(x => x.Summary)
               .NotEmpty()
               .Length(3, 1500);
            RuleFor(x => x.StartsAt)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));
            RuleFor(x => x.EndsAt)
                .NotEmpty();
            RuleFor(x => x)
                .Must(HasValidDate)
                .WithName(nameof(PollRequest.EndsAt))
                .WithMessage("{PropertyName} should be greater than or equals start date");
        }
        private bool HasValidDate(PollRequest pollRequest)
        {
            var valid = pollRequest.EndsAt >= pollRequest.StartsAt;
            return valid;
        }
    }
}
