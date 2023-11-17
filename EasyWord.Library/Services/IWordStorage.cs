﻿using EasyWord.Library.Models;
using System.Linq.Expressions;

namespace EasyWord.Library.Services;

public interface IWordStorage
{
    bool IsInitialized { get; }

    Task InitializeAsync();

    //CET4_1查找take个单词
    Task<IEnumerable<Word>> GetFromCET4_1Async(int take);
}