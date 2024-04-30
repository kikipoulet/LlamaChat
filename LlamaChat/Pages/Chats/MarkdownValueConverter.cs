using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace LlamaChat.Pages.Chats;

public class MarkdownConverter : IValueConverter
{
    public static string RemplacerCodeBlocks(string input)
    {
        // Utiliser une expression régulière pour rechercher et remplacer les motifs
        string pattern = @"```[^\n ]*";  // Correspond à "```" suivi de tout jusqu'au prochain espace

        // Remplacer toutes les occurrences de ce motif par "```"
        string replaced = Regex.Replace(input, pattern, "\r\n```\r\n");

        return replaced;
    }
    
    public static string FermerCodeBlocks(string input)
    {
        string pattern = @"```[^\n ]*";  // Expression pour détecter les blocs ouverts

        // Rechercher tous les blocs ouverts dans la chaîne
        MatchCollection matches = Regex.Matches(input, pattern);

        if (matches.Count % 2 != 0)
        {
            // Nombre impair de blocs ouverts => un bloc n'a pas été refermé
            input += "\r\n```\r\n";  // Ajouter "```" pour fermer le bloc ouvert
        }

        return input;
    }
    
    public object? Convert(object? value, Type targetType, object? parameter, 
        CultureInfo culture)
    {
       
        return FermerCodeBlocks(RemplacerCodeBlocks((string)value));
    }

    public object ConvertBack(object? value, Type targetType, 
        object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}