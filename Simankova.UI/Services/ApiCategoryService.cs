using Simankova.Domain.Entities;
using Simankova.Domain.Models;
using ICategoryService = Simankova.UI.Interfaces.ICategoryService;

namespace Simankova.Api.Services;

public class ApiCategoryService(HttpClient httpClient) : ICategoryService
{
    public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
    {
        var result = await httpClient.GetAsync(httpClient.BaseAddress);
        if (result.IsSuccessStatusCode)
        {
            var realResponse = await result.Content
                .ReadFromJsonAsync<ResponseData<List<Category>>>();
            return realResponse;
        };
        var response = new ResponseData<List<Category>>
            { Success = false, ErrorMessage = "Ошибка чтения API" };
        return response;
    }
}