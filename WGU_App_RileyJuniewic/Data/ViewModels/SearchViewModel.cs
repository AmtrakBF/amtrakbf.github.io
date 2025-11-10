using System.ComponentModel.DataAnnotations;
using Ardalis.Result;
using WGU_App_RileyJuniewic.Data.Dtos;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Misc.Commands;
using WGU_App_RileyJuniewic.Data.Services;

namespace WGU_App_RileyJuniewic.Data.ViewModels;

public class SearchViewModel : BindingModel
{
    private readonly ISearchService _searchService;
    private string _query = string.Empty;
    private SearchResult _searchResults = new();

    [Required(AllowEmptyStrings = false)]
    public string Query
    {
        get => _query;
        set => SetValue(nameof(Query), ref _query, value);
    }

    public OnClickCommandAsync SearchCommandAsync { get; set; }

    public SearchResult SearchResults
    {
        get => _searchResults;
        set => SetValue(nameof(SearchResults), ref _searchResults, value);
    }

    public SearchViewModel(ISearchService searchService)
    {
        _searchService = searchService;
        SearchCommandAsync = new OnClickCommandAsync(SearchAsync, (_) => !HasErrors);
    }

    public async Task SearchAsync()
    {
        var result = await _searchService.SearchAllAsync(Query);
        if (result.IsError())
        {
            new ToastNotification(result.Errors);
            return;
        }
        
        SearchResults = result.Value;
    }
}