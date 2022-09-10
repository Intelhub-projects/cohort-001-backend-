using MassTransit;
using System;

namespace Domain.Contracts
{
    public abstract class BaseEntity
    {
        public Guid Id { get;  set; } = NewId.Next().ToGuid();
    }
}