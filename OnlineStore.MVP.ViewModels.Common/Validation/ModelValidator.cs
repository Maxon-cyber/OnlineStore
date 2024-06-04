using System.ComponentModel.DataAnnotations;

namespace OnlineStore.MVP.ViewModels.Common.Validation;

public static class ModelValidator
{
    public static IList<ValidationResult> Validate(object obj)
    {
        List<ValidationResult> validationResults = new List<ValidationResult>();
        ValidationContext context = new ValidationContext(obj, serviceProvider: null, items: null);

        Validator.TryValidateObject(obj, context, validationResults, true);

        return validationResults;
    }
}