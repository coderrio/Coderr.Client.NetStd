using System.Collections.Generic;
using System.Linq;
using Coderr.Client.NetStd.Contracts;
using Coderr.Client.NetStd.Reporters;

namespace Coderr.Client.NetStd.ContextCollections
{
    /// <summary>
    /// Extensions to get the Coderr collection that we store meta data in
    /// </summary>
    public static class CoderrContextCollectionExtensions
    {
        /// <summary>
        /// Get or create our collection
        /// </summary>
        /// <param name="context">context to find the collection in</param>
        /// <returns>collection</returns>
        public static ContextCollectionDTO GetCoderrCollection(this IErrorReporterContext context)
        {
            var collection = context.ContextCollections.FirstOrDefault(x => x.Name == "Coderr");
            if (collection != null) 
                return collection;

            collection = new ContextCollectionDTO("Coderr");
            context.ContextCollections.Add(collection);
            return collection;
        }

        /// <summary>
        /// Get or create our collection
        /// </summary>
        /// <param name="collections">Collections array</param>
        /// <returns>collection</returns>
        public static ContextCollectionDTO GetCoderrCollection(this IList<ContextCollectionDTO> collections)
        {
            var collection = collections.FirstOrDefault(x => x.Name == "Coderr");
            if (collection != null) 
                return collection;

            collection = new ContextCollectionDTO("Coderr");
            collections.Add(collection);
            return collection;
        }
    }
}
