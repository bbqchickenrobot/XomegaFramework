﻿// Copyright (c) 2021 Xomega.Net. All rights reserved.

using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Xomega.Framework
{
    /// <summary>
    /// Computed binding for updating data object editability.
    /// </summary>
    public class ComputedEditableObjectBinding : ComputedBinding
    {
        private readonly DataObject dataObject;

        /// <summary>
        /// Constructs a new computed binding for updating data object editability.
        /// </summary>
        /// <param name="dataObject">The data object to update based on the computed result.</param>
        /// <param name="expression">Lambda expression used to compute the result.</param>
        /// <param name="args">Arguments for the specified expression to use for evaluation.</param>
        public ComputedEditableObjectBinding(DataObject dataObject, LambdaExpression expression, params object[] args)
            : base(null, expression, args)
        {
            if (dataObject == null) throw new ArgumentException("Data object cannot be null", nameof(dataObject));
            if (expression.ReturnType != typeof(bool))
                throw new Exception("Supplied expression should return a bool.");
            this.dataObject = dataObject;
        }

        /// <inheritdoc/>
        public override Task UpdateAsync(DataRow row, CancellationToken token)
        {
            dataObject.Editable = (bool)GetComputedValue(row);
            return Task.CompletedTask;
        }
    }
}
