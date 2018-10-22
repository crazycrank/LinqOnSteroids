﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace CLinq
{
    public sealed class ComposableQuery<T> : IOrderedQueryable<T>, IDbAsyncEnumerable<T>
    {
        private readonly ComposableQueryProvider<T> _innerProvider;

        public ComposableQuery(IQueryable<T> inner)
        {
            this.InnerQuery = inner ?? throw new ArgumentNullException(nameof(inner));
            this._innerProvider = new ComposableQueryProvider<T>(this);
        }

        public IQueryable<T> InnerQuery 
        {
            get;
        }

        /// <inheritdoc />
        Expression IQueryable.Expression => this.InnerQuery.Expression;

        /// <inheritdoc />
        Type IQueryable.ElementType => typeof(T);

        /// <inheritdoc />
        IQueryProvider IQueryable.Provider => this._innerProvider;

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator() => this.InnerQuery.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => this.InnerQuery.GetEnumerator();

        /// <inheritdoc />
        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
            => this.InnerQuery is IDbAsyncEnumerable<T> asyncEnumerable
                   ? asyncEnumerable.GetAsyncEnumerator()
                   : new DbAsyncComposableEnumerator<T>(this.InnerQuery.GetEnumerator());

        /// <inheritdoc />
        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
            => this.GetAsyncEnumerator();

        /// <inheritdoc />
        public override string ToString() => this.InnerQuery.ToString();
    }
}