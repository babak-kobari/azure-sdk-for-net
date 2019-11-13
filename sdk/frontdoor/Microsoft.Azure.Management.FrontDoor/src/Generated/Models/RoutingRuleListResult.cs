// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Management.FrontDoor.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Result of the request to list Routing Rules. It contains a list of
    /// Routing Rule objects and a URL link to get the next set of results.
    /// </summary>
    public partial class RoutingRuleListResult
    {
        /// <summary>
        /// Initializes a new instance of the RoutingRuleListResult class.
        /// </summary>
        public RoutingRuleListResult()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the RoutingRuleListResult class.
        /// </summary>
        /// <param name="value">List of Routing Rules within a Front
        /// Door.</param>
        /// <param name="nextLink">URL to get the next set of RoutingRule
        /// objects if there are any.</param>
        public RoutingRuleListResult(IList<RoutingRule> value = default(IList<RoutingRule>), string nextLink = default(string))
        {
            Value = value;
            NextLink = nextLink;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets list of Routing Rules within a Front Door.
        /// </summary>
        [JsonProperty(PropertyName = "value")]
        public IList<RoutingRule> Value { get; private set; }

        /// <summary>
        /// Gets or sets URL to get the next set of RoutingRule objects if
        /// there are any.
        /// </summary>
        [JsonProperty(PropertyName = "nextLink")]
        public string NextLink { get; set; }

    }
}