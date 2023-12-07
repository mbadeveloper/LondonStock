using System.ComponentModel.DataAnnotations;

namespace LondonStock.Resources.Validators
{
    public static class GeneralValidators
    {
        public static void ValidateBrokerId(Guid brokerId)
        {
            if (!Helpers.IsGuid(brokerId))
            {
                throw new ValidationException("Invalid brokerId");
            }
        }
    }
}
