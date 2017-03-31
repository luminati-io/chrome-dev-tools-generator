﻿namespace BaristaLabs.ChromeDevTools.RemoteInterface.DebuggerProtocol
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Dynamic;
    using System.Linq;

    public sealed class DomainDefinition : ProtocolDefinitionItem, ICodeGenerator
    {
        public DomainDefinition()
        {
            Dependencies = new HashSet<string>();

            Types = new Collection<TypeDefinition>();
            Events = new Collection<EventDefinition>();
            Commands = new Collection<CommandDefinition>();
        }

        [JsonProperty(PropertyName = "domain")]
        public override string Name
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "types")]
        public ICollection<TypeDefinition> Types
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "commands")]
        public ICollection<CommandDefinition> Commands
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "events")]
        public ICollection<EventDefinition> Events
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "dependencies")]
        public ICollection<string> Dependencies
        {
            get;
            set;
        }

        public IDictionary<string, string> GenerateCode(CodeGenerationSettings settings, dynamic options)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var command in Commands)
            {
                command.GenerateCode(settings, new { domain = this })
                    .ToList()
                    .ForEach(x => result.Add(x.Key, x.Value));
            }

            return result;
        }
    }
}
