using System;

namespace Contracts.Attributes
{
    /// <summary>
    /// Атрибут по финансовому идентификатору.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field, Inherited=false)]
    public class FigiAttribute : Attribute
    {
        public string Value { get; private set; }

        public FigiAttribute(string figi)
        {
            Value = figi;
        }
    }
}
