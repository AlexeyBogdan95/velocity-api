using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Velocity.Domain;

namespace Velocity.Infrastructure;

public class ThresholdRepository: IThresholdRepository
{
    private readonly DbSet<Threshold> _rules;

    public ThresholdRepository(DbSet<Threshold> rules)
    {
        _rules = rules;
    }

    public async Task<IReadOnlyCollection<Threshold>> Get(
        int customerId, DateTime[] dateTimes, CancellationToken cancellationToken)
    {
        return await _rules
            .Where(x => x.CustomerId == customerId && dateTimes.Contains(x.Date))
            .OrderByDescending(x => x.Version)
            .ToListAsync(cancellationToken);
    }

    public void AddMany(IEnumerable<Threshold> rule, CancellationToken cancellationToken)
    {
        _rules.AddRange(rule);
    }
}