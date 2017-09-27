using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions.Execution;
using codeRR.Client.Contracts;

namespace codeRR.Client.NetStd.Tests
{
    public static class TestExtensions
    {
        public static string GetCollectionProperty(this ErrorReportDTO report, string collectionName,
            string propertyName)
        {
            return report.GetCollection(collectionName).GetProperty(propertyName);
        }

        public static ContextCollectionDTO GetCollection(this ErrorReportDTO report, string name)
        {
            var collection = report.ContextCollections.FirstOrDefault(x => x.Name == name);
            if (collection == null)
            {
                var availableCollections = string.Join(", ", report.ContextCollections.Select(x => x.Name));
                throw new AssertionFailedException($"Collection '{name}' was not found. Available collections are: {availableCollections}.");
            }

            return collection;
        }

        public static string GetProperty(this ContextCollectionDTO collection, string name)
        {
            if (collection.Properties.TryGetValue(name, out string value))
                return value;

            var items = string.Join(", ", collection.Properties.Keys);
            throw new AssertionFailedException($"Property '{name}' was not found. Available properties are: {items}.");
        }
    }
}
