using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tomefico.Models;

public partial class LetterItem : ObservableObject
{
    [ObservableProperty] private string letter = "";
    [ObservableProperty] private bool isEnabled = true;
}
