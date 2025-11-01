using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace Salhia.KidsLibrary.Infrastructure.Services.UnitOfWork;

public sealed class EfUnitOfWork : IUnitOfWork, IAsyncDisposable
{
    private readonly AppDbContext _db;
    private IDbContextTransaction? _tx;

    public EfUnitOfWork(AppDbContext db) => _db = db;

    public async Task BeginTransactionAsync(CancellationToken ct = default)
        => _tx ??= await _db.Database.BeginTransactionAsync(ct);

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _db.SaveChangesAsync(ct);
    }

    public async Task CommitAsync(CancellationToken ct = default)
    {
        if (_tx is null) return;
        await _tx.CommitAsync(ct);
        await _tx.DisposeAsync();
        _tx = null;
    }

    public async Task RollbackAsync(CancellationToken ct = default)
    {
        if (_tx is null) return;
        await _tx.RollbackAsync(ct);
        await _tx.DisposeAsync();
        _tx = null;
    }

    public async ValueTask DisposeAsync()
    {
        if (_tx != null) await _tx.DisposeAsync();
    }
}


