// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MicroElements.Swashbuckle.NodaTime
{
    /// <summary>
    /// Resolves property name by <see cref="NodaTimeSchemaSettings"/>.
    /// </summary>
    internal class NamingPolicyParameterFilter : IParameterFilter
    {
        private readonly NodaTimeSchemaSettings _nodaTimeSchemaSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamingPolicyParameterFilter"/> class.
        /// </summary>
        /// <param name="nodaTimeSchemaSettings">Settings that controls serialization aspects.</param>
        public NamingPolicyParameterFilter(NodaTimeSchemaSettings nodaTimeSchemaSettings)
        {
            _nodaTimeSchemaSettings = nodaTimeSchemaSettings;
        }

        /// <inheritdoc />
        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            parameter.Name = _nodaTimeSchemaSettings.ResolvePropertyName(parameter.Name);
        }
    }
}
