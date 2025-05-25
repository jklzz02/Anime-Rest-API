using AnimeApi.Server.Business.Dto;
using AnimeApi.Server.Business.Services.Interfaces;
using AnimeApi.Server.Business.Validators.Interfaces;
using AnimeApi.Server.DataAccess.Services.Interfaces;

namespace AnimeApi.Server.Business.Services.Helpers;

public class TypeHelper : ITypeHelper
{
    private readonly ITypeRepository _repository;
    private readonly ITypeValidator _validator;
    public Dictionary<string, string> ErrorMessages { get; private set; } = new();

    public TypeHelper(ITypeRepository repository, ITypeValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }
    
    public async Task<TypeDto?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<TypeDto>> GetByNameAsync(string name)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<TypeDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<TypeDto?> CreateAsync(TypeDto entity)
    {
        throw new NotImplementedException();
    }

    public async Task<TypeDto?> UpdateAsync(TypeDto entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}