using System;
using AchillesRest.Helpers;

namespace AchillesRest.Models.CodeGenerations;

public static class CodeGeneratorFactory
{
    public static CodeGenerator GetCodeGenerator(SupportedLanguagesGeneration language)
    {
        return language switch
        {
            SupportedLanguagesGeneration.CSharp => new CSharpCodeGenerator(),
            _ => throw new NotSupportedException("Invalid language generation.")
        };
    }
}