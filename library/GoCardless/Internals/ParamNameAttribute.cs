using System;

namespace GoCardless.Internals
{
    internal class ParamNameAttribute : Attribute
    {
        public string Value { get; }

        public ParamNameAttribute(string value)
        {
            Value = value;
        }
    }
}
