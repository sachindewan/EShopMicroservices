﻿namespace Ordering.Domain.ValueObjects
{
    public record OrderName
    {
        private const int DefaultLength = 5;
        public string Value { get; }
        private OrderName(string value) => Value = value;
        public static OrderName Of(string value)
        {
            //[TODO] we can use erroror/ result pattern
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            //ArgumentOutOfRangeException.ThrowIfNotEqual(value.Length, DefaultLength);

            return new OrderName(value);
        }
    }
}