using System.ComponentModel.DataAnnotations;

namespace EShop.Modern.Models;

// Mirrors the legacy Create/Edit price rule: positive, at most two decimal
// places, between 0 and 1,000,000 inclusive.
public class PriceRangeAttribute : ValidationAttribute
{
    private const decimal Min = 0m;
    private const decimal Max = 1_000_000m;

    public override bool IsValid(object? value)
    {
        if (value is not decimal price)
        {
            return false;
        }

        if (price < Min || price > Max)
        {
            return false;
        }

        return price == Math.Round(price, 2);
    }
}
