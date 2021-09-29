﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MistralMovieRating.Repository
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>, IDisposable
        where TContext : DbContext
    {
        private Dictionary<Type, object> repositories;
        private bool disposed = false;

        public UnitOfWork(TContext context)
        {
            this.DbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets the db context.
        /// </summary>
        /// <returns>The instance of type <typeparamref name="TContext"/>.</returns>
        public TContext DbContext { get; }

        /// <summary>
        /// Gets the specified repository for the <typeparamref name="TIRepository"/>.
        /// </summary>
        /// <typeparam name="TIRepository">The type of the entity.</typeparam>
        /// <returns>An instance of type inherited from <typeparamref name="TIRepository"/> interface.</returns>
        public TIRepository GetRepository<TIRepository>()
        {
            if (this.repositories == null)
            {
                this.repositories = new Dictionary<Type, object>();
            }

            var type = typeof(TIRepository);
            if (!this.repositories.ContainsKey(type))
            {
                this.repositories[type] = this.DbContext.GetService<TIRepository>();
            }

            return (TIRepository)this.repositories[type];
        }

        public void Commit()
        {
            var transaction = this.DbContext.Database.BeginTransaction();
            try
            {
                this.DbContext.SaveChanges();

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public void Rollback()
        {
            this.DbContext.Dispose();
        }

        public async Task CommitAsync()
        {
            var transaction = await this.DbContext.Database.BeginTransactionAsync();
            try
            {
                await this.DbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public ValueTask RollbackAsync()
        {
            return this.DbContext.DisposeAsync();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">The disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // clear repositories
                    if (this.repositories != null)
                    {
                        this.repositories.Clear();
                    }

                    // dispose the db context.
                    this.DbContext.Dispose();
                }
            }

            this.disposed = true;
        }
    }
}
