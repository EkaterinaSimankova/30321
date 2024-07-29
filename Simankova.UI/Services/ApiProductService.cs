using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Simankova.Domain.Entities;
using Simankova.Domain.Models;
using Simankova.UI.Interfaces;

namespace Simankova.UI.Services;

public class ApiProductService(HttpClient httpClient) : IProductService
{

    public async Task<ResponseData<ProductListModel<Product>>>
        GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
    {
        var uri = httpClient.BaseAddress;
        var queryData = new Dictionary<string, string>();
        queryData.Add("pageNo", pageNo.ToString());
        if (!String.IsNullOrEmpty(categoryNormalizedName))
        {
            queryData.Add("category", categoryNormalizedName);
        }

        var query = QueryString.Create(queryData);

        var result = await httpClient.GetAsync(uri + query.Value);
        if (result.IsSuccessStatusCode)
        {
            var realResponse = await result.Content
                .ReadFromJsonAsync<ResponseData<ProductListModel<Product>>>();
            return realResponse;
        }

        ;
        var response = new ResponseData<ProductListModel<Product>>
            { Success = false, ErrorMessage = "Ошибка чтения API" };
        return response;
    }

    public async Task<ResponseData<Product>> CreateProductAsync(Product product, IFormFile?
        formFile)
    {
        var serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        // Подготовить объект, возвращаемый методом
        var responseData = new ResponseData<Product>();
        // Послать запрос к API для сохранения объекта
        var response = await httpClient.PostAsJsonAsync(httpClient.BaseAddress,
            product);
        if (!response.IsSuccessStatusCode)
        {
            responseData.Success = false;
            responseData.ErrorMessage = $"Не удалось создать объект:{response.StatusCode}";
            return responseData;
        }

        // Если файл изображения передан клиентом
        if (formFile != null)
        {
            // получить созданный объект из ответа Api-сервиса
            var savedProduct = await response.Content.ReadFromJsonAsync<Product>();
            // создать объект запроса
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{httpClient.BaseAddress.AbsoluteUri}{savedProduct.Id}")
            };
            // Создать контент типа multipart form-data
            var content = new MultipartFormDataContent();
            // создать потоковый контент из переданного файла
            var streamContent = new StreamContent(formFile.OpenReadStream());
            // добавить потоковый контент в общий контент по именем "image"
            content.Add(streamContent, "image", formFile.FileName);
            // поместить контент в запрос
            request.Content = content;
            // послать запрос к Api-сервису
            response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                responseData.Success = false;
                responseData.ErrorMessage = $"Не удалось сохранить изображение:{response.StatusCode}";
            }
        }

        return responseData;
    }

    public async Task<ResponseData<Product>> GetProductByIdAsync(int id)
    {
        var uri = httpClient.BaseAddress;

        var result = await httpClient.GetAsync($"{uri}{id}");
        if (result.IsSuccessStatusCode)
        {
            var realResponse = await result.Content
                .ReadFromJsonAsync<ResponseData<Product>>();

            return realResponse;
        }

        ;
        var response = new ResponseData<Product>
            { Success = false, ErrorMessage = "Ошибка чтения API" };
        return response;
    }
}