// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.DataApiBuilder.Config;

namespace Microsoft.DataApiBuilderSDK.Models
{
    /// <summary>
    /// FindRequestContext provides the major components of a REST query
    /// corresponding to the FindById or FindMany operations.
    /// </summary>
    public class FindRequestContext : RestRequestContext
    {
        /// <summary>
        /// Constructor.
        /// </summary>

        public FindRequestContext(string entityName, DatabaseObject dbo, bool isList)
            : base(entityName, dbo)
        {
            FieldsToBeReturned = new();
            PrimaryKeyValuePairs = new();
            FieldValuePairsInBody = new();
            IsMany = isList;
            OperationType = Operation.Read;
        }

    }
}
