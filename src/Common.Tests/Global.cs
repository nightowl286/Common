global using Microsoft.VisualStudio.TestTools.UnitTesting;

#if DEBUG
[assembly: Parallelize(Scope = ExecutionScope.ClassLevel, Workers = 1)]
#else
[assembly: Parallelize(Scope = ExecutionScope.MethodLevel, Workers = 0)]
#endif

internal static class Category
{
   public const string Extensions = nameof(Extensions);
   public const string Reflection = nameof(Reflection);
   public const string Serialisation = nameof(Serialisation);
   public const string Observers = nameof(Observers);
}