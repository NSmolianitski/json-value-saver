using Application.Dto;
using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ValueService(ApplicationDbContext context) : IValueService
{
    public async Task<List<ResponseValueDto>> GetValuesAsync()
    {
        var values = await context.SavedValues.ToListAsync();
        return values
            .Select(v => new ResponseValueDto(v.OrderId, v.Code, v.Value))
            .ToList();
    }

    public async Task ReplaceValuesAsync(ReplaceValuesRequestDto dto)
    {
        // Для удобства используется SQLite, где нет возможности сделать TRUNCATE TABLE, поэтому используется такое решение
        context.SavedValues.RemoveRange(context.SavedValues);

        var sortedValues = dto.Values.OrderBy(v => v.Code);
        var newValues = sortedValues.Select((v, index) => new SavedValue
        {
            OrderId = index,
            Code = v.Code,
            Value = v.Value
        });

        context.SavedValues.AddRange(newValues);
        await context.SaveChangesAsync();
    }
}