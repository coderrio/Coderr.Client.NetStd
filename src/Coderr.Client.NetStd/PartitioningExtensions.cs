using System.Linq;
using codeRR.Client.ContextCollections;
using codeRR.Client.Reporters;

namespace codeRR.Client
{
    public static class PartitioningExtensions
    {
        public static void AddPartition(this IErrorReporterContext context, string partitionName, string partitionKey)
        {
            var collection = context.ContextCollections.FirstOrDefault(x => x.Name == ErrPartitions.NAME);
            if (collection == null)
            {
                collection = new ErrPartitions();
                context.ContextCollections.Add(collection);
            }

            collection.Properties.Add(partitionName, partitionKey);
        }
    }
}