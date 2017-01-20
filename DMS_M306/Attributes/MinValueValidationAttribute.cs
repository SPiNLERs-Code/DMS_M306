using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DMS_M306.Attributes
{
    public class MinValueValidationAttribute : ValidationAttribute
    {
        private readonly int _maxValue;

        public MinValueValidationAttribute(int maxValue)
        {
            _maxValue = maxValue;
        }

        public override bool IsValid(object value)
        {
            if (value == null) return false;
            return (int)value >= _maxValue;
        }
    }
}