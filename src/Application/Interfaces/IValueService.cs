using Application.Dto;

namespace Application.Interfaces;

public interface IValueService
{
    Task<List<ResponseValueDto>> GetValuesAsync();
    Task ReplaceValuesAsync(ReplaceValuesRequestDto dto);
}