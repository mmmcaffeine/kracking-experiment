using System;

namespace AsIfByMagic.Extensions.Validation
{
    public interface IRule<T>
    {
        bool SatisfiedBy(T value);
        Exception CreateException(T value);
    }
}