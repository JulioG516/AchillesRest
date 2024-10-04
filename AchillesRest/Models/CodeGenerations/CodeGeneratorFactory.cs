using System;
using AchillesRest.Helpers;
using AchillesRest.Models.Enums;

namespace AchillesRest.Models.CodeGenerations;

public static class CodeGeneratorFactory
{
    public static CodeGenerator GetCodeGenerator(SupportedLanguagesGeneration language)
    {
        return language switch
        {
            SupportedLanguagesGeneration.CSharp => new CSharpCodeGenerator(),
            SupportedLanguagesGeneration.Java => new JavaCodeGenerator(),
            SupportedLanguagesGeneration.CSharpRestSharp => new CSharpCodeGenerator(),
            _ => throw new NotSupportedException("Invalid language generation.")
        };
    }
}