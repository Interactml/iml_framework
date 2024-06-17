using System;
namespace Abrusle
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AssetPathAttribute : Attribute
    {
        public string Path { get; }

        public AssetPathAttribute(string filePath)
        {
            Path = filePath;
        }
    }
}