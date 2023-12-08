﻿using BootstrapBlazor.Components;
using EasyWord.Library.Models;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using EasyWord.Library.Services;

namespace EasyWord.Library.Pages;

public partial class TodayWords
{
    private List<Word> _words = new();

    private List<Word> _todayWords = new();

    private int goal = 5;

    private int count = 0;

    private int pageSize = 5;

    private int totalWords = 0;

    private Expression<Func<Word, bool>> _where = p => p.Status == 0 & p.BookId == "CET4_1";

    private static string nowDate = DateTime.Now.ToString("yyyy-MM-dd");

    private Expression<Func<Word, bool>> _whereTodayWords = p => p.DateRecite == nowDate & p.BookId == "CET4_1";

    private bool soLittleWords = false;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }
        var todayWords = await _wordStorage.GetWordsAsync(_whereTodayWords, 0, 1500);
        _todayWords.Clear();
        _todayWords.AddRange(todayWords);
        count = _words.Count;

        pageSize = goal - count;
        StateHasChanged();

        var words = await _wordStorage.GetWordsAsync(_where, 0, pageSize);
        _words.Clear();
        _words.AddRange(words);

        StateHasChanged();
    }


    private async void IncrementGoal()
    {
        ++goal;
        pageSize = goal - count;
        if (goal > 20)
        {
            soLittleWords = true;
            --goal;
            pageSize = goal - count;
            await ToastService.Error("不能再加了", "背这么多要复习不完喽", autoHide: true);

        }
        StateHasChanged();
        var words = await _wordStorage.GetWordsAsync(_where, 0, pageSize);
        _words.Clear();
        _words.AddRange(words);
        StateHasChanged();

    }
    private async void DecrementGoal()
    {
        --goal;
        pageSize = goal - count;
        if (goal < 1)
        {
            soLittleWords = true;
            ++goal;
            pageSize = goal - count;
            await ToastService.Error("不能再减了", "至少要背一个吧", autoHide: true);

        }
        StateHasChanged();
        var words = await _wordStorage.GetWordsAsync(_where, 0, pageSize);
        _words.Clear();
        _words.AddRange(words);
        StateHasChanged();

    }

    //认识
    public async Task<int> KnowWord(int wordRank)
    {
        await _wordStorage.KnowWord(wordRank);
        --goal;
        pageSize = goal - count;
        StateHasChanged();
        var words = await _wordStorage.GetWordsAsync(_where, 0, pageSize);
        _words.Clear();
        _words.AddRange(words);
        StateHasChanged();
        return 1;
    }

    //不认识单词
    public async Task<int> UnknownWord(int wordRank)
    {
        await _wordStorage.UnknownWord(wordRank);
        --goal;
        pageSize = goal - count;
        StateHasChanged();
        var words = await _wordStorage.GetWordsAsync(_where, 0, pageSize);
        _words.Clear();
        _words.AddRange(words);
        StateHasChanged();
        return 1;
    }

    private async void GenerateReading()
    {
        _navigationService.NavigateTo(
            $"{NavigationServiceConstants.Reading}");
    }




    private void OnClick(Word word) =>
        _navigationService.NavigateTo(
            $"{NavigationServiceConstants.WordDetail}/{word.WordRank}");
}